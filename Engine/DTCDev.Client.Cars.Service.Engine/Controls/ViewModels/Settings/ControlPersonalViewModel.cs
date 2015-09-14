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
using DTCDev.Models;

namespace DTCDev.Client.Cars.Service.Engine.Controls.ViewModels.Settings
{
    public class ControlPersonalViewModel : ViewModelBase
    {
        private readonly PersonalHandler _handler = PersonalHandler.Instance;
        public ControlPersonalViewModel()
        {
            _handler.EmployeeDataLoadComplete += Instance_PersonsDataLoadComplete;
            _handler.UserAddComplete += Instance_UserAddComplete;
            _handler.GetRoles();
            _handler.GetPersons();
            _handler.EmployeeRolesLoaded += _handler_EmployeeRolesLoaded;
        }

        void _handler_EmployeeRolesLoaded(List<DicDataModel> data)
        {
            _listRoles.Clear();
            if(data == null) return;
            foreach (var el in data)
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
            _listUsers.Clear();
            List<EmployeeModel> data = (List<EmployeeModel>)sender;
            foreach (var item in data)
            {
                _listUsers.Add(new EmployeeViewModel(item));
            }
        }

        private readonly ObservableCollection<EmployeeViewModel> _listUsers = new ObservableCollection<EmployeeViewModel>();
        private EmployeeViewModel _selectedUser;
        private readonly ObservableCollection<DicDataModel> _listRoles = new ObservableCollection<DicDataModel>();
        private DicDataModel _selectedRole;

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
                    SelectedRole = ListRoles.FirstOrDefault(o => o.ID == value.IdRole);
                CompleteSaveEnabled = false;

                if (value == null)
                {
                    VisEdit = Visibility.Collapsed;
                }
                else
                {
                    VisEdit = Visibility.Visible;
                    EditText = "РЕДАКТИРОВАНИЕ ПОЛЬЗОВАТЕЛЯ";
                }
            }
        }

        private void SelectedUserOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            CompleteSaveEnabled = SelectedUser != null && !string.IsNullOrEmpty(SelectedUser.Name);
        }

        /// <summary>
        /// Список должностей
        /// </summary>
        public ObservableCollection<DicDataModel> ListRoles { get { return _listRoles; } }
        public DicDataModel SelectedRole
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

        private Visibility _visEdit = Visibility.Collapsed;
        public Visibility VisEdit
        {
            get { return _visEdit; }
            set
            {
                _visEdit = value;
                this.OnPropertyChanged("VisEdit");
            }
        }

        private string _editText = "РЕДАКТИРОВАНИЕ ПОЛЬЗОВАТЕЛЯ";

        public string EditText
        {
            get { return _editText; }
            set
            {
                _editText = value;
                this.OnPropertyChanged("EditText");
            }
        }





        private RelayCommand _completeSaveCommand;
        public RelayCommand CompleteSaveCommand
        {
            get { return _completeSaveCommand ?? (_completeSaveCommand = new RelayCommand(CompleteSave)); }
        }

        private void CompleteSave(object obj)
        {
            //Завершение процесса построения
            if (SelectedUser == null) return;
            _handler.EditUser(SelectedUser.Model);
            VisEdit = Visibility.Collapsed;
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
            VisEdit = Visibility.Visible;
            EditText = "ДОБАВЛЕНИЕ ПОЛЬЗОВАТЕЛЯ";
        }

    }
}
