using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRMDesktopUI.Library.API;

namespace TRMDesktopUI.ViewModels
{
    /// <summary>
    /// This Model will be called by ShellViewModel and hence LoginView will be displayed on ShellView.
    /// </summary>
    public class LoginViewModel : Screen
    {
        //Declared private properties.
        private string _username;
        private string _password;
        private IAPIHelper _apiHelper;
        private string _errorMessage;


        //This will set the value of _apiHelper (Dependency injection).
        public LoginViewModel(IAPIHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        //Getter and Setter for declared private properties and raise property change event.
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
        public bool CanLogIn //Property that gets.
        {
            get
            {
                bool output = false;

                if (UserName?.Length > 0 && Password?.Length > 0)//using ? at end of property name is checking if it's not null.
                {
                    output = true;
                }

                return output;
            }
        }

        //Validating the Error Message Length.
        //Name should be same as defined in Binding of Error Message Text Block
        public bool IsErrorVisible
        {
            get
            {
                bool output = false;
                if (ErrorMessage?.Length > 0)//using ? at end of property name is checking if it's not null.
                {
                    output = true;
                }
                return output;
            }
        }

        //property that gets and sets the value of _errorMessage and raise property change event.
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                NotifyOfPropertyChange(() => IsErrorVisible);
                NotifyOfPropertyChange(() => ErrorMessage);
            }
        }

        //Name of method should be same as defined in xaml.
        //Setting the ErrorMessage value.
        //Calling _apiHelper Authenticate method to get the token and storing it in result.
        //Calling _apiHelper GetLoggedInUser method by passing the token value to get the info Logged in user 
        //and creating an instance of LoggedInUserModel which will store all the info.
        public async Task LogIn()
        {
            try
            {
                ErrorMessage = "";
                var result = await _apiHelper.Authenticate(UserName, Password);

                await _apiHelper.GetLoggedInUserInfo(result.Access_Token);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }
    }
}
