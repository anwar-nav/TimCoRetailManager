using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRMDesktopUI.ViewModels
{
    /// <summary>
    /// This Model will be called by ShellViewModel and hence SalesView will be displayed on ShellView.
    /// </summary>
    public class SalesViewModel : Screen
    {
        //Declared private properties.
        private BindingList<string> _products;
        private BindingList<string> _cart;
        private int _itemQuantity;

        //ListBox Products
        //Getter and Setter for declared private properties and raise property change event.
        public BindingList<string> Products
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
        public BindingList<string> Cart
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
            }
        }

        //property that gets the value.
        public string SubTotal
        {
            get
            {
                //TODO calculations
                return "$0.00";
            }
        }

        //property that gets the value.
        public string Tax
        {
            get
            {
                //TODO calculations
                return "$0.00";
            }
        }

        //property that gets the value.
        public string Total
        {
            get
            {
                //TODO calculations
                return "$0.00";
            }
        }

        //Button Checking AddToCart.
        public bool CanAddToCart //Property that gets.
        {
            get
            {
                bool output = false;

                //add something

                return output;
            }
        }

        //Button AddToCart.
        public void AddToCart()
        {
            //add something
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
    }
}
