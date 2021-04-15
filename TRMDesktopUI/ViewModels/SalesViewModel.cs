using AutoMapper;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TRMDesktopUI.Library.API;
using TRMDesktopUI.Library.Helpers;
using TRMDesktopUI.Library.Models;
using TRMDesktopUI.Models;

namespace TRMDesktopUI.ViewModels
{
    /// <summary>
    /// This Model will be called by ShellViewModel and hence SalesView will be displayed on ShellView.
    /// </summary>
    public class SalesViewModel : Screen
    {
        //Declared private properties.
        private BindingList<ProductDisplayModel> _products;
        private BindingList<CartItemDisplayModel> _cart = new BindingList<CartItemDisplayModel>();
        private int _itemQuantity = 1;
        private IProductEndpoint _productEndpoint;
        private IConfigHelper _configHelper;
        private ISaleEndpoint _saleEndpoint;
        private IMapper _mapper;
        private readonly StatusInfoViewModel _status;
        private readonly IWindowManager _windowManager;
        private ProductDisplayModel _selectedProduct;
        private CartItemDisplayModel _selectedCartItem;

        //This constructor is pulling in Interfaces and storing in private variables for the life span of this class.
        public SalesViewModel(IProductEndpoint productEndpoint, IConfigHelper configHelper, 
                              ISaleEndpoint saleEndpoint, IMapper mapper, StatusInfoViewModel status, IWindowManager windowManager)
        {
            _productEndpoint = productEndpoint;
            _configHelper = configHelper;
            _saleEndpoint = saleEndpoint;
            _mapper = mapper;
            _status = status;
            _windowManager = windowManager;
        }

        //ListBox Products
        //Getter and Setter for declared private properties and raise property change event.
        public BindingList<ProductDisplayModel> Products
        {
            get { return _products; }
            set //Conditions can be applied here.
            {
                _products = value;
                NotifyOfPropertyChange(() => Products);
            }
        }

        //ListBox Cart
        //Getter and Setter for declared private properties and raise property change event.
        public BindingList<CartItemDisplayModel> Cart
        {
            get { return _cart; }
            set //Conditions can be applied here.
            {
                _cart = value;
                NotifyOfPropertyChange(() => Cart);
            }
        }

        //Getter and Setter for declared private properties and raise property change event.
        public int ItemQuantity
        {
            get { return _itemQuantity; }
            set //Conditions can be applied here.
            {
                _itemQuantity = value;
                NotifyOfPropertyChange(() => ItemQuantity);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        //property that gets the value.
        public string SubTotal
        {
            get
            {
                return CalculateSubTotal().ToString("C"); //returns in currency format
            }
        }

        //property that gets the value.
        public string Tax
        {
            get
            {
                return CalculateTax().ToString("C");
            }
        }

        //property that gets the value.
        public string Total
        {
            get
            {
                decimal Total = CalculateSubTotal() + CalculateTax();
                return Total.ToString("C");
            }
        }

        //Button AddToCart toggling visibility validation
        public bool CanAddToCart //Property that gets.
        {
            get
            {
                bool output = false;

                if (ItemQuantity > 0 && SelectedProduct?.QuantityInStock >= ItemQuantity)
                {
                    output = true;
                }

                return output;
            }
        }

        //This method will check first if an item selected already exists in cart and if it exists than it's existing
        //quantityincart will be updated otherwise new item will be added to the cart.
        //Then the selected product quantity will be deducted by the quantity added to the cart and item quantity box
        //will be updated to 1.
        //Then subtotal, tax and total will be updated by raising event.
        public void AddToCart()
        {
            CartItemDisplayModel existingItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);
            if (existingItem != null)
            {
                existingItem.QuantityInCart += ItemQuantity;
            }
            else
            {
                CartItemDisplayModel item = new CartItemDisplayModel
                {
                    Product = SelectedProduct, //property of CartItemDisplayModel.
                    QuantityInCart = ItemQuantity //property of CartItemDisplayModel.
                };
                Cart.Add(item);
            }
            
            SelectedProduct.QuantityInStock -= ItemQuantity;
            ItemQuantity = 1;
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckOut);
        }

        //Button RemoveFromCart toggling visibility validation.
        public bool CanRemoveFromCart //Property that gets.
        {
            get
            {
                bool output = false;

                if (SelectedCartItem != null && SelectedCartItem.QuantityInCart > 0)
                {
                    output = true;
                }

                return output;
            }
        }

        //Button RemoveFromCart.
        public void RemoveFromCart()
        {
            SelectedCartItem.Product.QuantityInStock += 1;

            if (SelectedCartItem.QuantityInCart > 1)
            {
                SelectedCartItem.QuantityInCart -= 1;
            }
            else
            {
                Cart.Remove(SelectedCartItem);
            }

            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckOut);
            NotifyOfPropertyChange(() => CanAddToCart);
        }

        //Button CheckOut toggling visibility validation
        public bool CanCheckOut //Property that gets.
        {
            get
            {
                bool output = false;

                if (Cart.Count > 0)
                {
                    output = true;
                }

                return output;
            }
        }

        //Button CheckOut.
        public async Task CheckOut()
        {
            SaleModel sale = new SaleModel();

            foreach (var item in Cart)
            {
                sale.SaleDetails.Add(new SaleDetailModel
                {
                    ProductId = item.Product.Id,
                    Quantity = item.QuantityInCart
                });
            }

            await _saleEndpoint.PostSale(sale);
            await ResetSaleViewModel();
        }

        private async Task ResetSaleViewModel()
        {
            Cart = new BindingList<CartItemDisplayModel>();
            await LoadProducts();

            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckOut);
        }

        //This will call the GetAll() method from ProductEndpoint class and store the received data from API into a variable
        //and then this variable will be used for mapping into display model and store in another variable and than this
        // variable will be binded to Listbox with data.
        private async Task LoadProducts()
        {
            var productlist = await _productEndpoint.GetAll();
            var products = _mapper.Map<List<ProductDisplayModel>>(productlist);
            Products = new BindingList<ProductDisplayModel>(products);
        }

        //This is overriding the default and creating an await call to LoadProducts method.
        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            try
            {
                await LoadProducts();
            }
            catch (Exception ex)
            {
                //These are settings of dialogbox.
                dynamic settings = new ExpandoObject();
                settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                settings.ResizeMode = ResizeMode.NoResize;
                settings.Title = "Sysem Error";

                //var info = IoC.Get<StatusInfoViewModel>(); for instance of viewmodel instead of constructor injection.

                if (ex.Message == "Unauthorized")
                {
                    //This is setting the values of StatusInfoViewModel.
                    _status.UpdateMessage("Unauthorized", "You do not have permission to interact with Sales Form.");
                    _windowManager.ShowDialogAsync(_status, null, settings); //This will bring StatusInfoview as dialogbox
                }
                else
                {
                    //This is setting the values of StatusInfoViewModel.
                    _status.UpdateMessage("Fatal Exception", ex.Message);
                    _windowManager.ShowDialogAsync(_status, null, settings); //This will bring StatusInfoview as dialogbox
                }
                TryCloseAsync();
            }
        }

        //Getter and Setter for declared private properties and raise property change event.
        public ProductDisplayModel SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                _selectedProduct = value;
                NotifyOfPropertyChange(() => SelectedProduct);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        //Getter and Setter for declared private properties and raise property change event.
        public CartItemDisplayModel SelectedCartItem
        {
            get { return _selectedCartItem; }
            set
            {
                _selectedCartItem = value;
                NotifyOfPropertyChange(() => SelectedCartItem);
                NotifyOfPropertyChange(() => CanRemoveFromCart);
            }
        }

        //Method for calculation of subtotal
        private decimal CalculateSubTotal()
        {
            decimal subTotal = 0;

            foreach (var item in Cart)
            {
                subTotal += (item.Product.RetailPrice * item.QuantityInCart);
            }

            return subTotal;
        }

        //Method for calculation of Tax
        private decimal CalculateTax()
        {
            decimal taxAmount = 0;
            decimal taxRate = _configHelper.GetTaxRate()/100;

            //New way using LINQ statement
            taxAmount = Cart
                .Where(x => x.Product.IsTaxable)
                .Sum(x => x.Product.RetailPrice * x.QuantityInCart * taxRate);

            //Old way
            //foreach (var item in Cart)
            //{
            //    if (item.Product.IsTaxable)
            //    {
            //        taxAmount += (item.Product.RetailPrice * item.QuantityInCart * taxRate); 
            //    }
            //}

            return taxAmount;
        }
    }
}
