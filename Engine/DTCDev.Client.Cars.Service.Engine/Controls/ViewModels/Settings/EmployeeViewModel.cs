using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCDev.Client.ViewModel;
using DTCDev.Models.CarBase.CarStatData;
using DTCDev.Models.Service;

namespace DTCDev.Client.Cars.Service.Engine.Controls.ViewModels.Settings
{
    public class EmployeeViewModel : ViewModelBase
    {
        private readonly EmployeeModel _model;
        private KVPBase _role;

        public EmployeeViewModel()
        {
            _model = new EmployeeModel();
        }
        public EmployeeViewModel(EmployeeModel model)
        {
            _model = model;
        }
        public EmployeeModel Model { get { return _model; } }

        public int Id
        {
            get { return _model.id; }
            set { _model.id = value; }
        }

        public string Name
        {
            get { return _model.Name; }
            set
            {
                _model.Name = value;
                OnPropertyChanged("Name");
            }
        }

        public string Mail
        {
            get { return _model.Mail; }
            set
            {
                _model.Mail = value;
                OnPropertyChanged("Mail");
            }
        }

        public string Phone
        {
            get { return _model.Phone; }
            set
            {
                _model.Phone = value;
                OnPropertyChanged("Phone");
            }
        }

        public string Login
        {
            get { return _model.Login; }
            set
            {
                _model.Login = value;
                OnPropertyChanged("Login");
            }
        }

        public string Password
        {
            get { return _model.Password; }
            set
            {
                _model.Password = value;
                OnPropertyChanged("Password");
            }
        }

        public List<int> DepartmentsIDs
        {
            get { return _model.DepartmentsIDs; }
        }

        public List<int> FunctionsAccess
        {
            get { return _model.FunctionsAccess; }
        }

        public string AddedName
        {
            get { return _model.AddedName; }
            set
            {
                _model.AddedName = value;
                OnPropertyChanged("AddedName");
            }
        }

        public int Locked
        {
            get { return _model.Locked; }
            set
            {
                _model.Locked = value;
                OnPropertyChanged("Locked");
            }
        }

        public int IdRole
        {
            get { return _model.idRole; }
        }

        public string AddedCuid
        {
            get { return _model.AddedCUID; }
            set
            {
                _model.AddedCUID = value;
                OnPropertyChanged("AddedCuid");
            }
        }

        public string Uid
        {
            get { return _model.UID; }
            set
            {
                _model.UID = value;
                OnPropertyChanged("Uid");
            }
        }

        public KVPBase Role
        {
            get { return _role; }
            set
            {
                _role = value;
                _model.idRole = value != null ? value.id : 0;
                OnPropertyChanged("Role");
            }
        }
    }
}
