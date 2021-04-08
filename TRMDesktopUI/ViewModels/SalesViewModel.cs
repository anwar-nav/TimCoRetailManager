using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRMDesktopUI.Library.API;
using TRMDesktopUI.Library.Helpers;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.ViewModels
{
    /// <summary>
    /// This Model will be called by ShellViewModel and hence SalesView will be displayed on ShellView.
    /// </summary>
    public class SalesViewModel : Screen
    {
        //Declared private properties.
        private BindingList<ProductModel> _products;
        private BindingList<CartItemModel> _cart = new BindingList<CartItemModel>();
        private int _itemQuantity = 1;
        private IProductEndpoint _productEndpoint;
        private IConfigHelper _configHelper;
        private ProductModel _selectedProduct;

        //This constructor is pulling in IProductEndpoint and storing in _productEndpoint for the life span of this class.
        public SalesViewModel(IProductEndpoint productEndpoint, IConfigHelper configHelper)
        {
            _productEndpoint = productEndpoint;
            _configHelper = configHelper;
        }

        //ListBox Products
        //Getter and Setter for declared private properties and raise property change event.
        public BindingList<ProductModel> Products
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
        public BindingList<CartItemModel> Cart
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
        //Then subtotal will be updated by raising event.
        public void AddToCart()
        {
            CartItemModel existingItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);
            if (existingItem != null)
            {
                existingItem.QuantityInCart += ItemQuantity;
                //Hack - There should be a better way of refreshing the cart display
                Cart.Remove(existingItem);
                Cart.Add(existingItem);
            }
            else
            {
                CartItemModel item = new CartItemModel
                {
                    Product = SelectedProduct, //property of CartItemModel.
                    QuantityInCart = ItemQuantity //property of CartItemModel.
                };
                Cart.Add(item);
            }
            
            SelectedProduct.QuantityInStock -= ItemQuantity;
            ItemQuantity = 1;
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
        }

        //Button Checking RemoveFromCart.
        public bool CanRemoveFromCart //Property that gets.
        {
            get
            {
                bool output = false;

                //add something

                return output;
            }
        }

        //Button RemoveFromCart.
        public void RemoveFromCart()
        {
            //add something
        }

        //Button Checking CheckOut.
        public bool CanCheckOut //Property that gets.
        {
            get
            {
                bool output = false;

                //add something

                return output;
            }
        }

        //Button CheckOut.
        public void CheckOut()
        {
            //add something
        }

        //This will call the GetAll() method from ProductEndpoint class and store the received data from API into a variable
        //and then this variable will be used for instantiation of the ListBox binded with data.
        private async Task LoadProducts()
        {
            var productlist = await _productEndpoint.GetAll();
            Products = new BindingList<ProductModel>(productlist);
        }

        //This is overriding the default and creating an await call to LoadProducts method.
        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            await LoadProducts();
        }

        //Getter and Setter for declared private properties and raise property change event.
        public ProductModel SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                _selectedProduct = value;
                NotifyOfPropertyChange(() => SelectedProduct);
                NotifyOfPropertyChange(() => CanAddToCart);
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
