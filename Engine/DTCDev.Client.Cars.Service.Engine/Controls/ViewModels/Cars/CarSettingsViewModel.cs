using DTCDev.Client.Cars.Service.Engine.Handlers;
using DTCDev.Client.Cars.Service.Engine.Storage;
using DTCDev.Client.ViewModel;
using DTCDev.Models.CarBase.CarStatData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace DTCDev.Client.Cars.Service.Engine.Controls.ViewModels.Cars
{
    public class CarSettingsViewModel : ViewModelBase
    {
        public CarSettingsViewModel()
        {
            //Если марки не загружены - загружаем марки и только потом настройки
            //так как по факту завершения загрузки настроек начинается процесс выставления текущих значений
            if (SpecificationDataStorage.Instance.Marks.Count() < 1)
            {
                SpecificationDataStorage.Instance.Update();
                SpecificationDataStorage.Instance.Marks.CollectionChanged += Marks_CollectionChanged;
            }
            else
                LoadCarSettings();
            CarStorage.Instance.UpdateProtocolTypeComplete += Instance_UpdateProtocolTypeComplete;
        }

        void Instance_UpdateProtocolTypeComplete(object sender, EventArgs e)
        {
            ProtocolType = CarStorage.Instance.NewProtocolType;
            Years = CarStorage.Instance.NewYears;
        }

        void Marks_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            SpecificationDataStorage.Instance.Marks.CollectionChanged -= Marks_CollectionChanged;
            LoadCarSettings();
        }

        private void LoadCarSettings()
        {
            CarStorage.Instance.GetCarSettings();
            CarStorage.Instance.LoadCurrentCarSettingsComplete += Instance_LoadCurrentCarSettingsComplete;
        }

        public ObservableCollection<KVPBase> Marks { get { return SpecificationDataStorage.Instance.Marks; } }
        public ObservableCollection<KVPBase> Models { get { return SpecificationDataStorage.Instance.Models; } }
        public ObservableCollection<KVPBase> EngineTypes { get { return SpecificationDataStorage.Instance.EngineTypes; } }
        public ObservableCollection<KVPBase> EngineVolumes { get { return SpecificationDataStorage.Instance.EngineVolumes; } }
        public ObservableCollection<KVPBase> TransmissionTypes { get { return SpecificationDataStorage.Instance.TransTypes; } }
        public ObservableCollection<KVPBase> Bodies { get { return SpecificationDataStorage.Instance.BodyTypes; } }


        public KVPBase Mark
        {
            get { return SpecificationDataStorage.Instance.SelectedMark; }
            set
            {
                if (SpecificationDataStorage.Instance.SelectedMark == value) return;
                SpecificationDataStorage.Instance.SelectedMark = value;
                this.OnPropertyChanged("Mark");
                CheckChanges();
            }
        }

        public KVPBase Model
        {
            get { return SpecificationDataStorage.Instance.SelectedModel; }
            set
            {
                if (SpecificationDataStorage.Instance.SelectedModel == value) return;
                SpecificationDataStorage.Instance.SelectedModel = value;
                this.OnPropertyChanged("Model");
                CheckChanges();
            }
        }

        public KVPBase Body
        {
            get { return SpecificationDataStorage.Instance.SelectedBody; }
            set
            {
                if (SpecificationDataStorage.Instance.SelectedBody == value) return;
                SpecificationDataStorage.Instance.SelectedBody = value;
                this.OnPropertyChanged("Body");
                CheckChanges();
            }
        }

        public KVPBase EngineType
        {
            get { return SpecificationDataStorage.Instance.SelectedEngineType; }
            set
            {
                if (SpecificationDataStorage.Instance.SelectedEngineType == value) return;
                SpecificationDataStorage.Instance.SelectedEngineType = value;
                this.OnPropertyChanged("EngineType");
                CheckChanges();
            }
        }

        public KVPBase EngineVolume
        {
            get { return SpecificationDataStorage.Instance.SelectedEngineVolume; }
            set
            {
                if (SpecificationDataStorage.Instance.SelectedEngineVolume == value) return;
                SpecificationDataStorage.Instance.SelectedEngineVolume = value;
                this.OnPropertyChanged("EngineVolume");
                CheckChanges();
            }
        }
        private KVPBase _transmissionType;

        public KVPBase TransmissionType
        {
            get { return _transmissionType; }
            set
            {
                if (_transmissionType == value) return;
                _transmissionType = value;
                this.OnPropertyChanged("TransmissionType");
                CheckChanges();
            }
        }


        void Instance_LoadCurrentCarSettingsComplete(object sender, EventArgs e)
        {
            Years = CarStorage.Instance.CarSettingsExemplar.Years;
            ProtocolType = CarStorage.Instance.CarSettingsExemplar.ProtocolType;
            Mark = Marks.Where(p => p.id == CarStorage.Instance.CarSettingsExemplar.IDMark).FirstOrDefault();
            SpecificationDataStorage.Instance.LoadModelsComplete += Instance_LoadModelsComplete;
        }

        void Instance_LoadModelsComplete(object sender, EventArgs e)
        {
            SpecificationDataStorage.Instance.LoadModelsComplete -= Instance_LoadModelsComplete;
            Model = Models.Where(p => p.id == CarStorage.Instance.CarSettingsExemplar.IDModel).FirstOrDefault();
            SpecificationDataStorage.Instance.LoadBodiesComplete += Instance_LoadBodiesComplete;
        }

        void Instance_LoadBodiesComplete(object sender, EventArgs e)
        {
            SpecificationDataStorage.Instance.LoadBodiesComplete -= Instance_LoadBodiesComplete;
            Body = Bodies.Where(p => p.id == CarStorage.Instance.CarSettingsExemplar.IDBody).FirstOrDefault();
            SpecificationDataStorage.Instance.LoadEngineTypesComplete += Instance_LoadEngineTypesComplete;
        }

        void Instance_LoadEngineTypesComplete(object sender, EventArgs e)
        {
            SpecificationDataStorage.Instance.LoadEngineTypesComplete -= Instance_LoadEngineTypesComplete;
            EngineType = EngineTypes.Where(p => p.id == CarStorage.Instance.CarSettingsExemplar.IDEngineType).FirstOrDefault();
            SpecificationDataStorage.Instance.LoadEnginsComplete += Instance_LoadEnginsComplete;
        }

        void Instance_LoadEnginsComplete(object sender, EventArgs e)
        {
            SpecificationDataStorage.Instance.LoadEnginsComplete -= Instance_LoadEnginsComplete;
            EngineVolume = EngineVolumes.Where(p => p.id == CarStorage.Instance.CarSettingsExemplar.IDEngine).FirstOrDefault();
            SpecificationDataStorage.Instance.LoadTransmissionsComplete += Instance_LoadTransmissionsComplete;
        }

        void Instance_LoadTransmissionsComplete(object sender, EventArgs e)
        {
            SpecificationDataStorage.Instance.LoadTransmissionsComplete -= Instance_LoadTransmissionsComplete;
            TransmissionType = TransmissionTypes.Where(p => p.id == CarStorage.Instance.CarSettingsExemplar.IDTransmission).FirstOrDefault();
        }

        private string _years;
        public string Years
        {
            get { return _years; }
            set
            {
                _years = value;
                this.OnPropertyChanged("Years");
            }
        }

        private string _protocolType;
        public string ProtocolType
        {
            get { return _protocolType; }
            set
            {
                _protocolType = value;
                this.OnPropertyChanged("ProtocolType");
            }
        }

        private Visibility _visChanges = Visibility.Collapsed;

        public Visibility VisChanges
        {
            get { return _visChanges; }
            set
            {
                _visChanges = value;
                this.OnPropertyChanged("VisChanges");
            }
        }

        private bool _enableSave = false;
        public bool EnableSave
        {
            get { return _enableSave; }
            set
            {
                _enableSave = value;
                this.OnPropertyChanged("EnableSave");
            }
        }

        private void CheckChanges()
        {
            if (Mark == null)
                VisChanges = Visibility.Visible;
            else if (Model == null)
                VisChanges = Visibility.Visible;
            else if (Body == null)
                VisChanges = Visibility.Visible;
            else if (EngineType == null)
                VisChanges = Visibility.Visible;
            else if (EngineVolume == null)
                VisChanges = Visibility.Visible;
            else if (TransmissionType == null)
                VisChanges = Visibility.Visible;
            else if (Mark.id != CarStorage.Instance.CarSettingsExemplar.IDMark ||
                Model.id != CarStorage.Instance.CarSettingsExemplar.IDModel ||
                Body.id != CarStorage.Instance.CarSettingsExemplar.IDBody ||
                EngineType.id != CarStorage.Instance.CarSettingsExemplar.IDEngineType ||
                EngineVolume.id != CarStorage.Instance.CarSettingsExemplar.IDEngine ||
                TransmissionType.id != CarStorage.Instance.CarSettingsExemplar.IDTransmission)
                VisChanges = Visibility.Visible;
            else
                VisChanges = Visibility.Collapsed;

            if(VisChanges==Visibility.Visible)
            {
                ProtocolType = "";
                Years = "";
                CheckEnableSave();
            }
            else
            {
                ProtocolType = CarStorage.Instance.CarSettingsExemplar.ProtocolType;
                Years = CarStorage.Instance.CarSettingsExemplar.Years;
            }
        }

        private void CheckEnableSave()
        {
            if (Mark != null && Model != null && Body != null && EngineVolume != null && EngineType != null && TransmissionType != null)
            {
                EnableSave = true;
                CarStorage.Instance.GetProtocolAndYear(new Models.CarsSending.Car.CarSettingsExemplarModel
                {
                    IDBody = Body.id,
                    IDEngine = EngineVolume.id,
                    IDEngineType = EngineType.id,
                    IDMark = Mark.id,
                    IDModel = Model.id,
                    IDTransmission = TransmissionType.id
                });
            }
            else
                EnableSave = false;
        }

        private RelayCommand _saveCommand;

        public RelayCommand SaveCommand { get { return _saveCommand ?? (_saveCommand = new RelayCommand(a => { StartSave(); })); } }

        private void StartSave()
        {
            CarStorage.Instance.SaveNewSettings(new Models.CarsSending.Car.CarSettingsExemplarModel
            {
                IDBody = Body.id,
                IDEngine = EngineVolume.id,
                IDEngineType = EngineType.id,
                IDMark = Mark.id,
                IDModel = Model.id,
                IDTransmission = TransmissionType.id,
                ProtocolType = CarStorage.Instance.SelectedCar.CarModel.DID
            });
        }
    }
}
