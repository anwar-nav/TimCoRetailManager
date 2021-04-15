using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TRMDesktopUI.EventModels;
using TRMDesktopUI.Library.API;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.ViewModels
{
    /// <summary>
    /// This is inheriting Conductor class of Caliburn.micro with object as it's type.
    /// This conductor will hold the activated item and can hold only one at a time.
    /// </summary>
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        //An object of type IEventAggregator is declared with access modifier as private.
        //An object of type SalesViewModel is declared with access modifier as private.
        //An object of type ILogInUserModel is declared with access modifier as private.
        //An object of type IAPIHelper is declared with access modifier as private.
        //It is a type of holder.
        private IEventAggregator _events;
        private SalesViewModel _salesVM;
        private ILoggedInUserModel _user;
        private IAPIHelper _apiHelper;

        //Constructor to load LoginViewModel and bring it upfront using await ActivateItemAsync method from
        //base class Conductor. This is using constructor injection.
        //When ShellViewModel class will be instantiated then this constructor will be called 
        //and the constructor will ask for this parameter of type IEventAggregator and SalesViewModel; 
        //IEventAggregator and SalesViewModel would have been already registered in the container created
        //by Bootstrapper class so the instance of IEventAggregator and SalesViewMode in the container will
        //be provided back by the container and these instances will be saved in private properties. Only ViewModels are
        //registered per request i.e. new every time requested.
        //In await ActivateItemAsync method everytime a new instance of LoginViewModel will be provided by container. This is required
        //because when LoginViewModel will get Activated again it will hold the previous values of username and password
        //so a new instance is required.
        public  ShellViewModel(IEventAggregator events, SalesViewModel salesVM, ILoggedInUserModel user, IAPIHelper aPIHelper)
        {
            _events = events; //setting private properties
            _salesVM = salesVM; //setting private properties
            _user = user; //setting private properties
            _apiHelper = aPIHelper; //setting private properties
            _events.Subscribe(this); //Subscribing to the event.

            ActivateItemAsync(IoC.Get<LoginViewModel>()); //Activating new instance every time.
        }

        public bool IsLoggedIn
        {
            get
            {
                bool output = false;

                if (string.IsNullOrWhiteSpace(_user.Token) == false)
                {
                    output = true;
                }

                return output;
            }
        }

        //This will close the application.
        public void ExitApplication()
        {
            TryCloseAsync();
        }

        //This will logout.
        public async Task LogOut()
        {
            _user.ResetLoggedInUserModel();
            _apiHelper.LogOfUser();
            await ActivateItemAsync(IoC.Get<LoginViewModel>()); //Activating new instance every time.
            NotifyOfPropertyChange(() => IsLoggedIn);
        }

        public async Task UserManagement()
        {
            await ActivateItemAsync(IoC.Get<UserDisplayViewModel>()); //Activating new instance every time.
        }

        //This method will be called when an event of LogOnEvent is raised.
        public async Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
        {
            await ActivateItemAsync(_salesVM); //Avtivating the SalesView.
            NotifyOfPropertyChange(() => IsLoggedIn);
        }
    }
}
