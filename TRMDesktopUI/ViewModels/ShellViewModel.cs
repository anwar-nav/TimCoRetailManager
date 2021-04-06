using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRMDesktopUI.ViewModels
{
    /// <summary>
    /// This is inheriting Conductor class of Caliburn.micro with object as it's type
    /// </summary>
    public class ShellViewModel : Conductor<object>
    {
        //An object of type LoginViewModel is declared with access modifier as private.
        //It is a type of holder.
        private LoginViewModel _loginVM;

        //Constructor to load LoginViewModel and bring it upfront using ActivateItem method from
        //base class Conductor. This is using constructor injection.
        //When ShellViewModel class will be instantiated then this constructor will be called 
        //and the constructor will ask for this parameter of type LoginViewModel; LoginViewModel would
        //have been already registered in the container created by Bootstrapper class so the new instance of
        //LoginViewModel in the container will be provided back by the container everytime requested to the constructor of LoginViewModel.
        public ShellViewModel(LoginViewModel loginVM)
        {
            _loginVM = loginVM;
            ActivateItem(_loginVM);
        }
    }
}
