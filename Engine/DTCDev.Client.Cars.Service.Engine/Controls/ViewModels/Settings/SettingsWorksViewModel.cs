using DTCDev.Client.Cars.Service.Engine.Handlers;
using DTCDev.Client.ViewModel;
using DTCDev.Models.CarBase.CarStatData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace DTCDev.Client.Cars.Service.Engine.Controls.ViewModels.Settings
{
    public class SettingsWorksViewModel : ViewModelBase
    {
        public SettingsWorksViewModel()
        {
            _storage.Update();
            _storage.UpdateWorks();
            _storage.UpdateWorkTypes();
        }

        private readonly CarsHandler _carHandler = CarsHandler.Instance;
        private readonly SpecificationDataStorage _storage = SpecificationDataStorage.Instance;

        public ObservableCollection<KVPBase> Marks { get { return _storage.Marks; } }
        public ObservableCollection<KVPBase> Models { get { return _storage.Models; } }
        public ObservableCollection<KVPBase> EngineTypes { get { return _storage.EngineTypes; } }
        public ObservableCollection<KVPBase> EngineVolumes { get { return _storage.EngineVolumes; } }
        public ObservableCollection<KVPBase> TransmissionTypes { get { return _storage.TransTypes; } }
        public ObservableCollection<KVPBase> Bodies { get { return _storage.BodyTypes; } }
        public ObservableCollection<WorksInfoExemplarDataModel> CarWorks { get { return _storage.CarWorkList; } }
        public ObservableCollection<WorksInfoDataModel> Works { get { return _storage.WorkList; } }
        public ObservableCollection<KVPBase> WorkTypes { get { return _storage.WorkTypes; } }

        public KVPBase Mark
        {
            get { return _storage.SelectedMark; }
            set
            {
                if (value != null)
                    MarkView = Visibility.Collapsed;
                else
                    MarkView = Visibility.Visible;
                if (_storage.SelectedMark == value) return;
                _storage.SelectedMark = value;
                OnPropertyChanged("Mark");
            }
        }

        public KVPBase Model
        {
            get { return _storage.SelectedModel; }
            set
            {
                if (value != null)
                    ModelView = Visibility.Collapsed;
                else
                    ModelView = Visibility.Visible;
                if (_storage.SelectedModel == value) return;
                _storage.SelectedModel = value;
                OnPropertyChanged("Model");
            }
        }

        public KVPBase Body
        {
            get { return _storage.SelectedBody; }
            set
            {
                if (value != null)
                    BodyView = Visibility.Collapsed;
                else
                    BodyView = Visibility.Visible;
                if (_storage.SelectedBody == value) return;
                _storage.SelectedBody = value;
                OnPropertyChanged("Body");
            }
        }

        public KVPBase EngineType
        {
            get { return _storage.SelectedEngineType; }
            set
            {
                if (value != null)
                    EngineView = Visibility.Collapsed;
                else
                    EngineView = Visibility.Visible;
                if (_storage.SelectedEngineType == value) return;
                _storage.SelectedEngineType = value;
                OnPropertyChanged("EngineType");
            }
        }

        public KVPBase EngineVolume
        {
            get { return _storage.SelectedEngineVolume; }
            set
            {
                if (value != null)
                    EngineVolView = Visibility.Collapsed;
                else
                    EngineVolView = Visibility.Visible;
                if (_storage.SelectedEngineVolume == value) return;
                _storage.SelectedEngineVolume = value;
                OnPropertyChanged("EngineVolume");
            }
        }

        public KVPBase TransmissionType
        {
            get { return _storage.SelectedTransmission; }
            set
            {
                if (value != null)
                    TransmissionView = Visibility.Collapsed;
                else
                    TransmissionView = Visibility.Visible;
                if (_storage.SelectedTransmission == value) return;
                _storage.SelectedTransmission = value;
                OnPropertyChanged("TransmissionType");
            }
        }

        private KVPBase _selectedWorkType;
        public KVPBase SelectedWorkType
        {
            get { return _selectedWorkType; }
            set
            {
                _selectedWorkType = value;
                this.OnPropertyChanged("SelectedWorkType");
            }
        }

        private Visibility _markView = Visibility.Visible;
        private Visibility _modelView = Visibility.Visible;
        private Visibility _bodyView = Visibility.Visible;
        private Visibility _engineView = Visibility.Visible;
        private Visibility _engineVolView = Visibility.Visible;
        private Visibility _transmissionView = Visibility.Visible;

        public Visibility MarkView
        {
            get { return _markView; }
            set
            {
                _markView = value;
                this.OnPropertyChanged("MarkView");
            }
        }
        public Visibility ModelView
        {
            get { return _modelView; }
            set
            {
                _modelView = value;
                this.OnPropertyChanged("ModelView");
            }
        }
        public Visibility BodyView
        {
            get { return _bodyView; }
            set
            {
                _bodyView = value;
                this.OnPropertyChanged("BodyView");
            }
        }
        public Visibility EngineView
        {
            get { return _engineView; }
            set
            {
                _engineView = value;
                this.OnPropertyChanged("EngineView");
            }
        }
        public Visibility EngineVolView
        {
            get { return _engineVolView; }
            set
            {
                _engineVolView = value;
                this.OnPropertyChanged("EngineVolView");
            }
        }
        public Visibility TransmissionView
        {
            get { return _transmissionView; }
            set
            {
                _transmissionView = value;
                this.OnPropertyChanged("TransmissionView");
            }
        }


        private bool _saveButtonEnabled = false;
        public bool SaveButtonEnabled
        {
            get { return _saveButtonEnabled; }
            set
            {
                _saveButtonEnabled = value;
                this.OnPropertyChanged("SaveButtonEnabled");
            }
        }

        private WorksInfoDataModel _selectedWork;
        public WorksInfoDataModel SelectedWork
        {
            get { return _selectedWork; }
            set
            {
                _selectedWork = value;
                this.OnPropertyChanged("SelectedWork");
                CheckSelectedWork();
            }
        }

        private string _selectedWorkText = "Выберите работу в списке справа";
        public string SelectedWorkText
        {
            get { return _selectedWorkText; }
            set
            {
                _selectedWorkText = value;
                this.OnPropertyChanged("SelectedWorkText");
            }
        }

        private int _addedPeriodic = 12;
        public int AddedPeriodic
        {
            get { return _addedPeriodic; }
            set
            {
                _addedPeriodic = value;
                this.OnPropertyChanged("AddedPeriodic");
            }
        }

        private int _addedDistance = 10000;
        public int AddedDistance
        {
            get { return _addedDistance; }
            set
            {
                _addedDistance = value;
                this.OnPropertyChanged("AddedDistance");
            }
        }








        private RelayCommand _addWorkToCarCommand;
        public RelayCommand AddWorkToCarCommand
        {
            get { return _addWorkToCarCommand ?? (_addWorkToCarCommand = new RelayCommand(AddWorkToCar)); }
        }

        private void AddWorkToCar(object sender)
        {
            _storage.AddWorkToCar(Model.id, TransmissionType.id, Body.id, EngineVolume.id, EngineType.id, SelectedWork.Name, AddedPeriodic, AddedDistance);
        }


        private void CheckSelectedWork()
        {
            if (SelectedWork == null)
                return;
            WorksInfoExemplarDataModel carTestWork = CarWorks.Where(p => p.Name == SelectedWork.Name).FirstOrDefault();
            if (carTestWork != null)
            {
                SelectedWorkText = "Эта работа уже указана";
                return;
            }
            else
            {
                SelectedWorkText = SelectedWork.Name;
            }
        }





    }
}
