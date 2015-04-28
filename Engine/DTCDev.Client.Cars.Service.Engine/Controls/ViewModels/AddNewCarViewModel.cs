using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DTCDev.Client.Cars.Service.Engine.Handlers;
using DTCDev.Client.ViewModel;
using DTCDev.Models.CarBase.CarList;
using DTCDev.Models.CarBase.CarStatData;
using DTCDev.Models.Date;
using DTCDev.Models.CarsSending.Car;
using System.Windows;

namespace DTCDev.Client.Cars.Service.Engine.Controls.ViewModels
{
    public class AddNewCarViewModel : ViewModelBase, ICancelHandler
    {
        private string _carNumber;
        private int _distance;
        private KVPBase _transmissionType;
        private readonly ObservableCollection<ServiceViewModel> _serviceWorks = new ObservableCollection<ServiceViewModel>();
        private readonly CarsHandler _carHandler = CarsHandler.Instance;
        private readonly SpecificationDataStorage _storage = SpecificationDataStorage.Instance;
        private RelayCommand _saveCommand;
        private RelayCommand _cancelCommand;
        private DateTime _dateProduce = DateTime.Now;
        private DateTime _datePurchase = DateTime.Now;
        private bool _validateCarNumber;

        public AddNewCarViewModel()
        {
            _storage.Update();
            CarsHandler.Instance.ServiceControllerRefreshed += Instance_ServiceControllerRefreshed;
            CarsHandler.Instance.UpdateServiceControllers();
        }

        List<CarWithSettingDevice> _deviceData = new List<CarWithSettingDevice>();

        void Instance_ServiceControllerRefreshed(List<CarWithSettingDevice> data)
        {
            _deviceData = data;
            FillDevices();
        }

        public event EventHandler CancelHandler;
        protected virtual void OnCancelHandler()
        {
            if (CancelHandler != null) CancelHandler(this, EventArgs.Empty);
        }

        public void OnPropertyChange(string property)
        {
            OnPropertyChanged(property);
            OnPropertyChanged("EnableButtonNext");
        }

        public ObservableCollection<KVPBase> Marks { get { return _storage.Marks; } }
        public ObservableCollection<KVPBase> Models { get { return _storage.Models; } }
        public ObservableCollection<KVPBase> EngineTypes { get { return _storage.EngineTypes; } }
        public ObservableCollection<KVPBase> EngineVolumes { get { return _storage.EngineVolumes; } }
        public ObservableCollection<KVPBase> TransmissionTypes { get { return _storage.TransTypes; } }
        public ObservableCollection<KVPBase> Bodies { get { return _storage.BodyTypes; } }
        public ObservableCollection<ServiceViewModel> ServiceWorks { get { return _serviceWorks; } }

        private ObservableCollection<CarWithSettingDevice> _devices = new ObservableCollection<CarWithSettingDevice>();
        public ObservableCollection<CarWithSettingDevice> Devices { get { return _devices; } }

        public string CarNumber
        {
            get { return _carNumber; }
            set
            {
                if (_carNumber == value) return;
                _carNumber = value;
                OnPropertyChange("CarNumber");
                IsValidateCarNumber();
            }
        }

        private string _vin;
        public string VIN
        {
            get { return _vin; }
            set
            {
                _vin = value;
                this.OnPropertyChanged("VIN");
            }
        }


        public bool ValidateCarNumber
        {
            get { return _validateCarNumber; }
            set
            {
                if (_validateCarNumber == value) return;
                _validateCarNumber = value;
                OnPropertyChange("ValidateCarNumber");
            }
        }

        public KVPBase Mark
        {
            get { return _storage.SelectedMark; }
            set
            {
                if (_storage.SelectedMark == value) return;
                _storage.SelectedMark = value;
                OnPropertyChange("Mark");
            }
        }

        public KVPBase Model
        {
            get { return _storage.SelectedModel; }
            set
            {
                if (_storage.SelectedModel == value) return;
                _storage.SelectedModel = value;
                OnPropertyChange("Model");
            }
        }

        public KVPBase Body
        {
            get { return _storage.SelectedBody; }
            set
            {
                if (_storage.SelectedBody == value) return;
                _storage.SelectedBody = value;
                OnPropertyChange("Body");
            }
        }

        public KVPBase EngineType
        {
            get { return _storage.SelectedEngineType; }
            set
            {
                if (_storage.SelectedEngineType == value) return;
                _storage.SelectedEngineType = value;
                OnPropertyChange("EngineType");
            }
        }

        public KVPBase EngineVolume
        {
            get { return _storage.SelectedEngineVolume; }
            set
            {
                if (_storage.SelectedEngineVolume == value) return;
                _storage.SelectedEngineVolume = value;
                OnPropertyChange("EngineVolume");
            }
        }

        private int intEngineVolume
        {
            get
            {
                var res = 0;
                if (_storage.SelectedEngineVolume != null)
                    int.TryParse(_storage.SelectedEngineVolume.Name, out res);
                return res;
            }
        }

        public KVPBase TransmissionType
        {
            get { return _transmissionType; }
            set
            {
                if (_transmissionType == value) return;
                _transmissionType = value;
                OnPropertyChange("TransmissionType");
            }
        }

        public int Distance
        {
            get { return _distance; }
            set
            {
                if (_distance == value) return;
                _distance = value;
                OnPropertyChange("Distance");
            }
        }
        public bool EnableButtonNext
        {
            get { return IsCompleteForm(); }
        }

        private bool IsCompleteForm()
        {
            return CarNumber != null && Mark != null && Model != null &&
                Body != null && EngineType != null && EngineVolume != null &&
                TransmissionType != null && ValidateCarNumber && ClientName.Length>2 
                && IsPhoneValid();
        }

        public DateTime DateProduce
        {
            get { return _dateProduce; }
            set
            {
                if (_dateProduce == value) return;
                _dateProduce = value;
                OnPropertyChanged("DateProduce");
            }
        }

        public DateTime DatePurchase
        {
            get { return _datePurchase; }
            set
            {
                if (_datePurchase == value) return;
                _datePurchase = value;
                OnPropertyChanged("DatePurchase");
            }
        }

        private string _clientName = "";
        public string ClientName
        {
            get { return _clientName; }
            set
            {
                _clientName = value;
                this.OnPropertyChanged("ClientName");
            }
        }

        private string _phoneNumber="+7";
        public string PhoneHumber
        {
            get { return _phoneNumber; }
            set
            {
                _phoneNumber = value;
                this.OnPropertyChanged("PhoneHumber");
            }
        }

        private string _mail = "";
        public string Mail
        {
            get { return _mail; }
            set
            {
                _mail = value;
                this.OnPropertyChanged("Mail");
            }
        }

        private string _login = "";
        public string Login
        {
            get { return _login; }
            set
            {
                _login = value;
                this.OnPropertyChanged("Login");
            }
        }

        private CarWithSettingDevice _selectedDevice;
        public CarWithSettingDevice SelectedDevice
        {
            get { return _selectedDevice; }
            set
            {
                _selectedDevice = value;
                this.OnPropertyChanged("SelectedDevice");
                if (value == null)
                    ErrorMessageVisibility = Visibility.Collapsed;
                else if (value.IsPlug == 0)
                    ErrorMessageVisibility = Visibility.Collapsed;
                else
                    ErrorMessageVisibility = Visibility.Visible;
            }
        }

        private Visibility _errorMessageVisibility = Visibility.Collapsed;
        public Visibility ErrorMessageVisibility
        {
            get { return _errorMessageVisibility; }
            set
            {
                _errorMessageVisibility = value;
                this.OnPropertyChanged("ErrorMessageVisibility");
            }
        }

        private bool _showOnlyFree = false;
        public bool ShowOnlyFree
        {
            get { return _showOnlyFree; }
            set
            {
                _showOnlyFree = value;
                this.OnPropertyChanged("ShowOnlyFree");
                FillDevices();
            }
        }






        public RelayCommand SaveCommand
        {
            get { return _saveCommand ?? (_saveCommand = new RelayCommand(Save)); }
        }

        private void Save(object obj)
        {
            _carHandler.AddCar(GetCarServiceModel());
            //TODO Получить список работ
        }

        void Instance_GetServiceWorksComplete(object sender, EventArgs e)
        {
            //TODO заполнить ServiceWorks
        }

        public RelayCommand CancelCommand
        {
            get { return _cancelCommand ?? (_cancelCommand = new RelayCommand(Cancel)); }
        }


        private void Cancel(object obj)
        {
            SaveWorks();
            OnCancelHandler();
        }

        private void SaveWorks()
        {
            if (ServiceWorks.Count < 1) return;
            var list = ServiceWorks.Select(o => o.GetServiceModel(o)).ToList();
            //TODO сохранить список работ
        }

        private CarServiceDataModel GetCarServiceModel()
        {
            CarServiceDataModel model = new CarServiceDataModel
            {
                CarNumber = CarNumber,
                Mark = Mark != null ? Mark.Name : "",
                id_Mark = Mark != null ? Mark.id : 0,
                Model = Model != null ? Model.Name : "",
                id_Model = Model != null ? Model.id : 0,
                Body = Body != null ? Body.Name : "",
                id_Body = Body != null ? Body.id : 0,
                EngineType = EngineType != null ? EngineType.Name : "",
                id_EngineType = EngineType != null ? EngineType.id : 0,
                EngineVolume = intEngineVolume,
                id_Engine = EngineVolume != null ? EngineVolume.id : 0,
                TransmissionType = TransmissionType != null ? TransmissionType.Name : "",
                id_Transmission = TransmissionType != null ? TransmissionType.id : 0,
                CurrentDistance = Distance,
                DateProduce = new DateDataModel(DateProduce),
                DatePurchase = new DateDataModel(DatePurchase),
                DID = SelectedDevice != null ? SelectedDevice.DID : "",
                ClientLogin = Login,
                ClientMail = Mail,
                ClientName = ClientName,
                ClientPhone = PhoneHumber
            };
            return model;
        }

        private void FillDevices()
        {
            Devices.Clear();
            if (ShowOnlyFree == false)
                _deviceData.ForEach(x => Devices.Add(x));
            else
            {
                List<CarWithSettingDevice> temp = _deviceData.Where(p => p.IsPlug == 0).ToList();
                temp.ForEach(x => Devices.Add(x));
            }
        }

        private bool isValidEmail(string email)
        {
            string pattern = "[.\\-_a-z0-9]+@([a-z0-9][\\-a-z0-9]+\\.)+[a-z]{2,6}";
            Match isMatch = Regex.Match(email, pattern, RegexOptions.IgnoreCase);
            return isMatch.Success;
        }

        private void IsValidateCarNumber()
        {
            var rg = new Regex(@"[A-ZА-Я]\d{3}[A-ZА-Я]{2}\d{2,3}");
            ValidateCarNumber = !rg.IsMatch(CarNumber);
        }

        private bool IsPhoneValid()
        {
            string pattern = "^((8|\\+7)[\\- ]?)?(\\(?\\d{3}\\)?[\\- ]?)?[\\d\\- ]{7,10}$";
            Match isMatch = Regex.Match(PhoneHumber, pattern, RegexOptions.IgnoreCase);
            return isMatch.Success;
        }
    }
}
