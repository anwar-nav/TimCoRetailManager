using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TRMDesktopUI.Helpers;
using TRMDesktopUI.Library.API;
using TRMDesktopUI.Library.Models;
using TRMDesktopUI.ViewModels;

namespace TRMDesktopUI
{
    /// <summary>
    /// This class will be used for setting up caliburn.micro
    /// </summary>

    public class Bootstrapper : BootstrapperBase
    {
        //This container will handle all the instantiating of classes aka Dependency Injection(DI).
        private SimpleContainer _container = new SimpleContainer();

        //Constructor used for base class method to run.
        public Bootstrapper()
        {
            Initialize();

            //Using the PasswordBoxHelper class and binding the property with Caliburn.micro
            ConventionManager.AddElementConvention<PasswordBox>(
            PasswordBoxHelper.BoundPasswordProperty,
            "Password",
            "PasswordChanged");
        }

        //This is where actual instantiation happens or container knows what to connect to what. The container holds the 
        //instance of itself to pass out when SimpleContainer is called. This container is required in order to manipulate
        //or to change or to get information out of it besides from constructor. This configure runs once at start of application.
        //Added PerRequest for instances of ProductEndpoint class.
        //Added Singleton for Window Manager and Event Aggregator based in caliburn.micro. for managing windows and passing event messages.
        //Added Singleton for APIHelper from UI.Library for managing instance of it.
        //Added Singleton for LoggedInUserModel from UI.Library for managing instance of it.
        //Added reflection (It's slow so string builder is better to use).
        protected override void Configure()
        {
            _container.Instance(_container)
                .PerRequest<IProductEndpoint, ProductEndpoint>(); //per request instance.

            _container
                .Singleton<IWindowManager, WindowManager>() //Interface tied with implementation.
                .Singleton<IEventAggregator, EventAggregator>() //Interface tied with implementation.
                .Singleton<ILoggedInUserModel, LoggedInUserModel>() //Interface tied with implementation.
                .Singleton<IAPIHelper, APIHelper>(); //Interface tied with implementation.

            GetType().Assembly.GetTypes() //reflection gettype of running assembly and get all the types for our current instance.
                .Where(type => type.IsClass) //limit to type of class.
                .Where(type => type.Name.EndsWith("ViewModel")) //limit to type of class with name ending ViewModel.
                .ToList() // create a list of the found types
                .ForEach(viewModelType => _container.RegisterPerRequest( // registers the each found class in container.
                    viewModelType, viewModelType.ToString(), viewModelType));

        }

        //This method will override the base class method and be used on startup of application.
        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            //This will launch ShellViewModel on startup as our base view.
            //Then this ShellViewModel will launch the ShellView.
            DisplayRootViewFor<ShellViewModel>();
        }

        //This method will override the base class method and be used for getting the instance.
        protected override object GetInstance(Type service, string key)
        {
            //This will return the instance present in _container by having the type(service) of instance and name(key).
            return _container.GetInstance(service, key);
        }

        //This method will override the base class method and be used for getting all the instances.
        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            //This will return all the instances present in _container by having the type(service) of instance.
            return _container.GetAllInstances(service);
        }

        //This is where things get constructed
        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }

    }
}
