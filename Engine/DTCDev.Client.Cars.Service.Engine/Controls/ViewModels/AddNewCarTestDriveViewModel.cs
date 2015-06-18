using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DTCDev.Client.Cars.Service.Engine.Handlers;
using DTCDev.Client.Cars.Service.Engine.Storage;
using DTCDev.Client.ViewModel;
using DTCDev.Models.CarBase.CarList;
using DTCDev.Models.CarBase.CarStatData;
using DTCDev.Models.Date;
using DTCDev.Models.CarsSending.Car;
using System.Windows;

namespace DTCDev.Client.Cars.Service.Engine.Controls.ViewModels
{
    public class AddNewCarTestDriveViewModel : ViewModelBase
    {
        private string _carNumber;
        private int _distance;
        private KVPBase _transmissionType;
        private readonly TestDriveCarStorage _carHandler = TestDriveCarStorage.Instance;
        private readonly SpecificationDataStorage _storage = SpecificationDataStorage.Instance;
        private RelayCommand _saveCommand;
        private RelayCommand _addCommand;
        private DateTime _dateProduce = DateTime.Now;
        private bool _validateCarNumber;
        private CarListDetailsDataModel _carDetail;

        public AddNewCarTestDriveViewModel()
        {
            _storage.Update();
            _carHandler.OnGetCarDetailCompleteOnlyFill += Instance_OnGetCarDetailComplete;
            _storage.LoadMarksComplete += _storage_LoadMarksComplete;
            _storage.LoadModelsComplete +=_storage_LoadModelsComplete;
            _storage.LoadBodiesComplete += _storage_LoadBodiesComplete;
            _storage.LoadEngineTypesComplete += _storage_LoadEngineTypesComplete;
            _storage.LoadEnginsComplete+=_storage_LoadEnginsComplete;
            _storage.LoadTransmissionsComplete+=_storage_LoadTransmissionsComplete;
            _carHandler.LoadComplete += Instance_LoadComplete;
            UpdateCar();
        }


        private void _storage_LoadTransmissionsComplete(object sender, EventArgs e)
        {
            if (_carDetail == null) return;
            TransmissionType = TransmissionTypes.FirstOrDefault(o => o.Name.Equals(_carDetail.TransmissionType));
        }

        private void _storage_LoadEnginsComplete(object sender, EventArgs e)
        {
            if (_carDetail == null) return;
            EngineVolume = EngineVolumes.FirstOrDefault(o => o.Name.Equals(_carDetail.EngineVolume));
        }

        void _storage_LoadEngineTypesComplete(object sender, EventArgs e)
        {
            if (_carDetail == null) return;
            EngineType = EngineTypes.FirstOrDefault(o => o.Name.Equals(_carDetail.EngineType));
        }

        private void _storage_LoadModelsComplete(object sender, EventArgs e)
        {
            if (_carDetail == null) return;
            Model = Models.FirstOrDefault(o => o.Name.Equals(_carDetail.Model));
        }

        private void Instance_OnGetCarDetailComplete(CarListDetailsDataModel carDetail)
        {
            _carDetail = carDetail;
            Distance = carDetail.CurrentDistance;
            DateProduce = carDetail.DateProduce.ToDate;
            Mark = Marks.FirstOrDefault(o => o.Name.Equals(carDetail.Mark));
        }

        void _storage_LoadBodiesComplete(object sender, EventArgs e)
        {
            if (_carDetail == null) return;
            Body = Bodies[0];
        }

        void _storage_LoadMarksComplete(object sender, EventArgs e)
        {
            if (_carDetail == null) return;
            if (!string.IsNullOrEmpty(_carDetail.Model))
                Model = Models.FirstOrDefault(o => o.Name.Equals(_carDetail.Model));
        }

        private void Instance_LoadComplete(object sender, EventArgs e)
        {
            UpdateCar();
        }

        public event EventHandler OnSelectedCarChange;
        private void SelectedCarChange(DISP_Car car)
        {
            VIN = car.CarModel.DID;
            CarNumber = car.CarModel.CarNumber;
            _carHandler.GetCarDetails(CarNumber);
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

        private readonly ObservableCollection<DISP_Car> _cars = new ObservableCollection<DISP_Car>();
        public ObservableCollection<DISP_Car> Cars
        {
            get { return _cars; }
        }

        private DISP_Car _selectedCar;
        public DISP_Car SelectedCar
        {
            get { return _selectedCar; }
            set
            {
                if (_selectedCar == value) return;
                _selectedCar = value;
                OnPropertyChanged("SelectedCar");
                if (value != null)
                    SelectedCarChange(value);
            }
        }

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
                _storage.SelectedMark = value;
                OnPropertyChange("Mark");
            }
        }

        public KVPBase Model
        {
            get { return _storage.SelectedModel; }
            set
            {
                _storage.SelectedModel = value;
                OnPropertyChange("Model");
            }
        }

        public KVPBase Body
        {
            get { return _storage.SelectedBody; }
            set
            {
                _storage.SelectedBody = value;
                OnPropertyChange("Body");
            }
        }

        public KVPBase EngineType
        {
            get { return _storage.SelectedEngineType; }
            set
            {
                _storage.SelectedEngineType = value;
                OnPropertyChange("EngineType");
            }
        }

        public KVPBase EngineVolume
        {
            get { return _storage.SelectedEngineVolume; }
            set
            {
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
                TransmissionType != null && ValidateCarNumber;
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

        void UpdateCar()
        {
            Cars.Clear();
            foreach (var item in _carHandler.Cars)
            {
                Cars.Add(item);
            }
        }

        private void IsValidateCarNumber()
        {
            var rg = new Regex(@"[A-ZА-Я]\d{3}[A-ZА-Я]{2}\d{2,3}");
            ValidateCarNumber = string.IsNullOrEmpty(CarNumber) || !rg.IsMatch(CarNumber);
        }





        public RelayCommand SaveCommand
        {
            get { return _saveCommand ?? (_saveCommand = new RelayCommand(Save)); }
        }

        private void Save(object obj)
        {
            //TODO Сохранить изменения
        }

        public RelayCommand AddCommand
        {
            get { return _addCommand ?? (_addCommand = new RelayCommand(AddCar)); }
        }


        private void AddCar(object obj)
        {
            ClearValues();
            SelectedCar = new DISP_Car{CarModel = new CarListBaseDataModel()};
        }

        private void ClearValues()
        {
            CarNumber = VIN = string.Empty;
            TransmissionType = EngineVolume = EngineType = Body = Mark = Model = null;
            Distance = 0;
            DateProduce = DateTime.Now;
        }

        private CarServiceDataModel GetCarServiceModel()
        {
            var model = new CarServiceDataModel
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

            };
            return model;
        }

    }
}
