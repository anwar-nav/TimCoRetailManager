using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRMDesktopUI.Helpers;

namespace TRMDesktopUI.ViewModels
{
    public class LoginViewModel : Screen
    {
        //Declared private properties.
        private string _username;
        private string _password;
        private IAPIHelper _apiHelper;

        //This will set the value of _apiHelper (Dependency injection).
        public LoginViewModel(IAPIHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        //Getter and Setter for declared private properties.
        public string UserName
        {
            get { return _username; }
            set //Conditions can be applied here.
            {
                _username = value;
                NotifyOfPropertyChange(() => UserName);
                NotifyOfPropertyChange(() => CanLogIn);
            }
        }

        //Getter and Setter for declared private properties.
        public string Password
        {
            get { return _password; }
            set //Conditions can be applied here.
            {
                _password = value;
                NotifyOfPropertyChange(() => Password);
                NotifyOfPropertyChange(() => CanLogIn);
            }
        }

        //Validating the Username and Password.
        public bool CanLogIn //Property that gets
        {
            get
            {
                bool output = false;

                if (UserName?.Length > 0 && Password?.Length > 0)//using ? at end of property name is checking if it's not null
                {
                    output = true;
                }

                return output;
            }
        }


        //Using camel case for parameter naming
        //Name should be same as defined in xaml
        public async Task LogIn()
        {
            try
            {
                var result = await _apiHelper.Authenticate(UserName, Password);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


    }
}
