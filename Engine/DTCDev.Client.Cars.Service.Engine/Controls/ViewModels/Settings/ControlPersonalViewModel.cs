using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using DTCDev.Client.Cars.Service.Engine.Handlers;
using DTCDev.Client.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCDev.Models.CarBase.CarStatData;
using DTCDev.Models.Service;

namespace DTCDev.Client.Cars.Service.Engine.Controls.ViewModels.Settings
{
    public class ControlPersonalViewModel : ViewModelBase
    {
        private readonly PersonalHandler _handler = PersonalHandler.Instance;
        public ControlPersonalViewModel()
        {
            _handler.EmployeeDataLoadComplete += Instance_PersonsDataLoadComplete;
            _handler.UserAddComplete += Instance_UserAddComplete;
            _handler.RolesLoadComplete += Instance_RolesLoadComplete;
            _handler.GetRoles();
            _handler.GetPersons();
            //if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                _listRoles.Add(new KVPBase { id = 1, Name = "Role 1" });
                _listRoles.Add(new KVPBase { id = 2, Name = "Role 2" });
                _listRoles.Add(new KVPBase { id = 3, Name = "Role 3" });
                _listUsers.Add(new EmployeeViewModel(new EmployeeModel { Name = "User 1", Phone = "+7 789 7", Login = "User", Password = "pass", Mail = "w@e" }) { Role = _listRoles[1] });
                _listUsers.Add(new EmployeeViewModel(new EmployeeModel { Name = "User 2", Phone = "+7 789 789 78 78", Login = "User2", Password = "pass2", Mail = "w2@e" }) { Role = _listRoles[0] });
                SelectedUser = _listUsers[0];
            }
        }

        private void Instance_RolesLoadComplete(List<KVPBase> list)
        {
            _listRoles.Clear();
            foreach (var el in list)
                _listRoles.Add(el);
        }

        private void Instance_UserAddComplete(EmployeeModel model)
        {
            var user = ListUsers.FirstOrDefault(o => o.Id == model.id);
            if (user == null)
                ListUsers.Add(new EmployeeViewModel(model));
        }

        void Instance_PersonsDataLoadComplete(object sender, EventArgs e)
        {

        }

        private readonly ObservableCollection<EmployeeViewModel> _listUsers = new ObservableCollection<EmployeeViewModel>();
        private EmployeeViewModel _selectedUser;
        private readonly ObservableCollection<KVPBase> _listRoles = new ObservableCollection<KVPBase>();
        private KVPBase _selectedRole;

        /// <summary>
        /// Список пользователей
        /// </summary>
        public ObservableCollection<EmployeeViewModel> ListUsers { get { return _listUsers; } }

        public EmployeeViewModel SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                if (_selectedUser != null) _selectedUser.PropertyChanged -= SelectedUserOnPropertyChanged;
                _selectedUser = value;
                if (_selectedUser != null) _selectedUser.PropertyChanged += SelectedUserOnPropertyChanged;
                OnPropertyChanged("SelectedUser");
                if (value != null && value.IdRole > 0)
                    SelectedRole = ListRoles.FirstOrDefault(o => o.id == value.IdRole);
                CompleteSaveEnabled = false;
            }
        }

        private void SelectedUserOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            CompleteSaveEnabled = SelectedUser != null && !string.IsNullOrEmpty(SelectedUser.Name);
        }

        /// <summary>
        /// Список должностей
        /// </summary>
        public ObservableCollection<KVPBase> ListRoles { get { return _listRoles; } }
        public KVPBase SelectedRole
        {
            get { return _selectedRole; }
            set
            {
                _selectedRole = value;
                OnPropertyChanged("SelectedRole");
                if (SelectedUser != null)
                    SelectedUser.Role = value;
            }
        }


        private bool _completeSaveEnabled = false;
        public bool CompleteSaveEnabled
        {
            get { return _completeSaveEnabled; }
            set
            {
                _completeSaveEnabled = value;
                OnPropertyChanged("CompleteSaveEnabled");
            }
        }

        private RelayCommand _completeSaveCommand;
        public RelayCommand CompleteSaveCommand
        {
            get { return _completeSaveCommand ?? (_completeSaveCommand = new RelayCommand(CompleteSave)); }
        }

        private void CompleteSave(object obj)
        {
            if (SelectedUser == null) return;
            _handler.EditUser(SelectedUser.Model);
            //Временно пока не работает запрос
            if (SelectedUser.Model.id != 0) return;
            SelectedUser.Model.id = ListUsers.Count;
            ListUsers.Add(SelectedUser);
        }

        private RelayCommand _addUserCommand;
        public RelayCommand AddUserCommand
        {
            get { return _addUserCommand ?? (_addUserCommand = new RelayCommand(AddUser)); }
        }

        private void AddUser(object obj)
        {
            SelectedUser = new EmployeeViewModel();
            SelectedRole = null;
        }

    }
}
