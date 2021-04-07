using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRMDesktopUI.EventModels;

namespace TRMDesktopUI.ViewModels
{
    /// <summary>
    /// This is inheriting Conductor class of Caliburn.micro with object as it's type.
    /// This conductor will hold the activated item and can hold only one at a time.
    /// </summary>
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        //An object of type SimpleContainer is declared with access modifier as private.
        //An object of type IEventAggregator is declared with access modifier as private.
        //An object of type SalesViewModel is declared with access modifier as private.
        //It is a type of holder.
        private SimpleContainer _container;
        private IEventAggregator _events;
        private SalesViewModel _salesVM;

        //Constructor to load LoginViewModel and bring it upfront using ActivateItem method from
        //base class Conductor. This is using constructor injection.
        //When ShellViewModel class will be instantiated then this constructor will be called 
        //and the constructor will ask for this parameter of type SimpleContainer, IEventAggregator and SalesViewModel; 
        //SimpleContainer, IEventAggregator and SalesViewModel would have been already registered in the container created
        //by Bootstrapper class so the instance of SimpleContainer, IEventAggregator and SalesViewMode in the container will
        //be provided back by the container and these instances will be saved in private properties. Only ViewModels are
        //registered per request i.e. new every time requested.
        //In ActivateItem method everytime a new instance of LoginViewModel will be provided by container. This is required
        //because when LoginViewModel will get Activated again it will hold the previous values of username and password
        //so a new instance is required.
        public ShellViewModel(SimpleContainer container, IEventAggregator events, SalesViewModel salesVM)
        {
            _container = container; //setting private properties
            _events = events; //setting private properties
            _salesVM = salesVM; //setting private properties

            _events.Subscribe(this); //Subscribing to the event.

            ActivateItem(_container.GetInstance<LoginViewModel>()); //Activating new instance every time.
        }

        //This method will be called when an event of LogOnEvent is raised.
        public void Handle(LogOnEvent message)
        {
            ActivateItem(_salesVM); //Avtivating the SalesView.
        }
    }
}
