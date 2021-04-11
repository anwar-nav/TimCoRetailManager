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
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.ViewModels
{
    /// <summary>
    /// This Model will be called by ShellViewModel and hence UserDisplayView will be displayed on ShellView.
    /// </summary>
    public class UserDisplayViewModel : Screen
    {
        //Declared private properties.
        private readonly IUserEndpoint _userEndpoint;
        private readonly StatusInfoViewModel _status;
        private readonly IWindowManager _windowManager;
        private BindingList<UserModel> _users;
        private UserModel _selectedUser;
        private string _selectedUserName;
        private BindingList<string> _userRoles = new BindingList<string>();
        private BindingList<string> _availableRoles = new BindingList<string>();
        private string _selectedUserRole;
        private string _selectedAvailableRole;

        //This constructor is pulling in Interfaces and Classes and storing in private variables for the life span of this class.
        public UserDisplayViewModel(IUserEndpoint userEndpoint, StatusInfoViewModel status, IWindowManager windowManager)
        {
            _userEndpoint = userEndpoint;
            _status = status;
            _windowManager = windowManager;
        }

        //This getter and setter of BindingList<UserModel> _users.
        public BindingList<UserModel> Users
        {
            get { return _users; }
            set
            {
                _users = value;
                NotifyOfPropertyChange(() => Users);
            }
        }

        //This getter and setter of UserModel  _selectedUser.
        public UserModel SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                SelectedUserName = value.Email;
                UserRoles = new BindingList<string>(value.Roles.Select(x => x.Value).ToList());
                LoadRoles();
                NotifyOfPropertyChange(() => SelectedUser);
            }
        }

        //This getter and setter of _selectedUserName.
        public string SelectedUserName
        {
            get { return _selectedUserName; }
            set
            {
                _selectedUserName = value;
                NotifyOfPropertyChange(() => SelectedUserName);
            }
        }
        
        //This getter and setter of BindingList<string> _userRoles.
        public BindingList<string> UserRoles
        {
            get { return _userRoles; }
            set
            {
                _userRoles = value;
                NotifyOfPropertyChange(() => UserRoles);
            }
        }

        //This getter and setter of BindingList<string> _availableRoles.
        public BindingList<string> AvailableRoles
        {
            get { return _availableRoles; }
            set
            {
                _availableRoles = value;
                NotifyOfPropertyChange(() => AvailableRoles);
            }
        }

        //This getter and setter of _selectedUserRole.
        public string SelectedUserRole
        {
            get { return _selectedUserRole; }
            set
            {
                _selectedUserRole = value;
                NotifyOfPropertyChange(() => SelectedUserRole);
            }
        }

        //This getter and setter of _selectedAvailableRole.
        public string SelectedAvailableRole
        {
            get { return _selectedAvailableRole; }
            set
            {
                _selectedAvailableRole = value;
                NotifyOfPropertyChange(() => SelectedAvailableRole);
            }
        }


        //This will call the GetAll() method from ProductEndpoint class and store the received data from API into a variable
        //and then this variable will be used for mapping into display model and store in another variable and than this
        // variable will be binded to Listbox with data.
        private async Task LoadUsers()
        {
            var userlist = await _userEndpoint.GetAll();
            Users = new BindingList<UserModel>(userlist);
        }

        //This is overriding the default and creating an await call to LoadProducts method.
        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            try
            {
                await LoadUsers();
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
                    _windowManager.ShowDialog(_status, null, settings); //This will bring StatusInfoview as dialogbox
                }
                else
                {
                    //This is setting the values of StatusInfoViewModel.
                    _status.UpdateMessage("Fatal Exception", ex.Message);
                    _windowManager.ShowDialog(_status, null, settings); //This will bring StatusInfoview as dialogbox
                }
                TryClose();
            }
        }

        //This will get all roles from the API.
        private async Task LoadRoles()
        {
            var roles = await _userEndpoint.GetAllRoles();

            foreach (var role in roles)
            {
                if (UserRoles.IndexOf(role.Value) < 0)
                {
                    AvailableRoles.Add(role.Value);
                }
            }
        }

        //This will add selected role to user in database.
        public async Task AddSelectedRole()
        {
            await _userEndpoint.AddUserToRole(SelectedUser.Id, SelectedAvailableRole);

            UserRoles.Add(SelectedAvailableRole);
            AvailableRoles.Remove(SelectedAvailableRole);
        }

        //This will remove selected role from user in database.
        public async Task RemoveSelectedRole()
        {
            await _userEndpoint.RemoveUserFromRole(SelectedUser.Id, SelectedUserRole);

            AvailableRoles.Add(SelectedUserRole);
            UserRoles.Remove(SelectedUserRole);
        }
    }
}
