using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Data;
using DTCDev.Client.Cars.Service.Engine.Storage;
using DTCDev.Client.ViewModel;
using DTCDev.Models.CarBase.CarList;
using DTCDev.Models.CarsSending.Car;
using DTCDev.Models.Service;
using DTCDev.Client.Cars.Service.Engine.Handlers;
using DTCDev.Models;
using DTCDev.Models.User;

namespace DTCDev.Client.Cars.Service.Engine.Controls.ViewModels.Settings
{
    public class TestDriveCarViewModel : ViewModelBase
    {
        public TestDriveCarViewModel()
        {

        }
        public event EventHandler IsCompleteSaved;

        protected virtual void CompleteSave(object sender)
        {
            if (IsCompleteSaved != null) IsCompleteSaved(this, EventArgs.Empty);
        }
        private readonly TestDriveCarStorage _handler = TestDriveCarStorage.Instance;
        private string _name = string.Empty;
        private int _endtWorkTime = -1;
        private int _startWorkTime = -1;
        private string _carNumber;
        private TestDriveModel _model ;

        public TestDriveModel Model
        {
            get { return _model ?? (_model = new TestDriveModel()); }
            private set { _model  = value; }
        }

        public CarListBaseDataModel CurrentCar
        {
            get { return Model.Car ?? (Model.Car = new CarListBaseDataModel()); }
            set { Model.Car = value; }
        }

        public TestDriveUserModel User
        {
            get { return Model.User ?? (Model.User = new TestDriveUserModel()); }
            set
            {
                if (Model.User.Equals(value)) return;
                IsChanged = true;
                Model.User = value;
                OnUserChanged();
            }
        }

        private void OnUserChanged()
        {
            OnPropertyChanged("UserName");
            OnPropertyChanged("Phone");
            OnPropertyChanged("Email");
            OnPropertyChanged("DriverId");
        }

        public TestDriveCarViewModel(DateTime dt, CarListBaseDataModel model, int id, bool changed)
        {
            Update(model, id);
            CurrentCar = model;
            _dateWork = dt;
            IsChange = changed;
        }

        public TestDriveCarViewModel(CarListBaseDataModel model, int id)
        {
            Update(model, id);
            CurrentCar = model;
        }

        private void Update(CarListBaseDataModel model, int id)
        {
            ID = id;
            CarNumber = model.DID;
            Name = model.CarNumber;
            StartWorkTime = 8;
            EndWorkTime = 18;
        }

        public TestDriveCarViewModel(TestDriveCarViewModel testdrive)
        {
            Update(testdrive.Model);
            Model.Car = testdrive.CurrentCar;
            _dateWork = testdrive.DateWork;
        }

        public void Update(TestDriveModel model)
        {
            Model = model;
            ID = model.Id;
            CarNumber = model.Car.DID;
            Name = model.Car.CarNumber;
            StartWorkTime = 8;
            EndWorkTime = 18;
        }

        public void Update(TestDriveUserModel user)
        {
            UserName = user.Name;
            Phone = user.Phone;
            Email = user.Email;
            DriverId = user.DriverId;
        }

        public int ID
        {
            get { return Model.Id; }
            set { Model.Id = value; }
        }

        public string CarNumber
        {
            get { return _carNumber; }
            set
            {
                _carNumber = value;
                OnPropertyChanged("CarNumber");
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
                OnPropertyChanged("DisplayName");
            }
        }


        public int StartWorkTime
        {
            get { return _startWorkTime; }
            set
            {
                if(_startWorkTime == value) return;
                _startWorkTime = value;
                OnPropertyChanged("StartWorkTime");
                OnPropertyChanged("DisplayName");
            }
        }


        public int EndWorkTime
        {
            get { return _endtWorkTime; }
            set
            {
                if (_endtWorkTime == value) return;
                _endtWorkTime = value;
                OnPropertyChanged("EndWorkTime");
                OnPropertyChanged("DisplayName");
            }
        }

        public string WorkTime
        {
            get
            {
                return (StartWorkTime != -1 ? string.Format("{0}:00", StartWorkTime.ToString("00")) : "") + "-" + 
                    (EndWorkTime != -1 ? string.Format("{0}:00", EndWorkTime.ToString("00")) : "");
            }
        }

        public bool IsChange { get; set; }

        public string UserName
        {
            get { return User.Name ?? string.Empty; }
            set
            {
                IsChanged = User.Name != value;
                User.Name = value;
                OnPropertyChanged("UserName");
            }
        }

        public string Email
        {
            get { return User.Email ?? string.Empty; }
            set
            {
                IsChanged = User.Email != value;
                User.Email = value;
                OnPropertyChanged("Email");
            }
        }

        public string Phone
        {
            get { return User.Phone ?? string.Empty; }
            set
            {
                IsChanged = User.Phone != value;
                User.Phone = value;
                OnPropertyChanged("Phone");
            }
        }

        public string DriverId
        {
            get { return User.DriverId ?? string.Empty; }
            set
            {
                IsChanged = User.DriverId != value;
                User.DriverId = value;
                OnPropertyChanged("DriverId");
            }
        }


        public int TimeTestDrive
        {
            get { return _handler.TimeTestDrive; }
            set
            {
                _handler.TimeTestDrive = value;
                OnPropertyChanged("DriverId");
            }
        }

        public override string DisplayName
        {
            get
            {
                return Name + (string.IsNullOrEmpty(WorkTime) ? "" : string.Format(" ({0})", WorkTime));
            }
        }

        //public override string Na ()
        //{
        //    return Name + (string.IsNullOrEmpty(WorkTime) ? "": string.Format(" ({0})",WorkTime));
        //}


        public DateTime DateWork
        {
            get { return _dateWork; }
            set
            {
                if(_dateWork.Equals(value)) return;
                IsChanged = true;
                _dateWork = value;
            }
        }

        private bool _isChanged;
        public bool IsChanged
        {
            get { return _isChanged; }
            set
            {
                _isChanged = value;
                OnPropertyChanged("IsChanged");
            }
        }

        private RelayCommand _textChangedCommand;
        public RelayCommand TextChangedCommand
        {
            get { return _textChangedCommand ?? (_textChangedCommand = new RelayCommand(TextChanged)); }
        }

        private void TextChanged(object obj)
        {
            SelectedUser = null;
            VisableUserList = !UserName.Equals(_foundString = obj.ToString());
            if (!VisableUserList) return;
            ListUsers.Refresh();
        }

        private string _foundString = string.Empty;
        private TestDriveUserModel _selectedUser;
        public TestDriveUserModel SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                if (value == null) return;
                User = value;
                TextChanged(value.Name);
            }
        }

        private bool _visableUserList = false;
        public bool VisableUserList
        {
            get { return _visableUserList; }
            set
            {
                _visableUserList = value;
                OnPropertyChanged("VisableUserList");
            }
        }

        private CollectionViewSource _listUsers;
        public ICollectionView ListUsers
        {
            get
            {
                if (_listUsers != null) return _listUsers.View;
                _listUsers = new CollectionViewSource { Source = _handler.Users };
                _listUsers.Filter += (o, e) =>
                {
                    e.Accepted = string.IsNullOrEmpty(_foundString) || ((TestDriveUserModel)e.Item).Name.Contains(_foundString);
                };
                return _listUsers.View;
            }
        }

        private RelayCommand _completeSaveCommand;
        private DateTime _dateWork;

        public RelayCommand SaveCommand
        {
            get { return _completeSaveCommand ?? (_completeSaveCommand = new RelayCommand(CompleteSave)); }
        }

        
    }
}
