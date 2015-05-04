using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using DTCDev.Client.Cars.Service.Engine.Handlers;
using DTCDev.Client.Cars.Service.Engine.Storage;
using DTCDev.Client.ViewModel;
using DTCDev.Models.CarBase.CarList;
using DTCDev.Models.CarBase.CarStatData;

namespace DTCDev.Client.Cars.Service.Engine.Controls.ViewModels.Settings
{
    public class SlideWorksSettingsViewModel : ViewModelBase
    {
        public SlideWorksSettingsViewModel()
        {
            _storage.Update();
            _storage.UpdateWorks();
            _storage.UpdateWorkTypes();
            _carstorage.LoadComplete += _carstorage_LoadComplete;

            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                Cars.Add(new DISP_Car{ CarModel = new CarListBaseDataModel{CarNumber = "qqqq", Mark = "Audio", Model = "A3"}});
            }
        }


        private readonly SpecificationDataStorage _storage = SpecificationDataStorage.Instance;
        private readonly CarStorage _carstorage = CarStorage.Instance;

        public ObservableCollection<KVPBase> Marks { get { return _storage.Marks; } }
        public ObservableCollection<KVPBase> Models { get { return _storage.Models; } }
        
        public ObservableCollection<KVPBase> WorkTypes { get { return _storage.WorkTypes; } }

        public ObservableCollection<WorksInfoDataModel> Works
        {
            get
            {
                if (SelectedWorkType == null)
                    return _storage.WorkList;
                var works = new ObservableCollection<WorksInfoDataModel>();
                _storage.WorkList.Where(o => o.id_Class == SelectedWorkType.id).ToList().ForEach(works.Add);
                return works;
            }
        }

        private readonly ObservableCollection<WorksInfoDataCostModel> _carWorks = new ObservableCollection<WorksInfoDataCostModel>();
        public ObservableCollection<WorksInfoDataCostModel> CarWorks { get { return _carWorks; } }

        private WorksInfoDataCostModel _selectedCarWorks = null;
        public WorksInfoDataCostModel SelectedCarWorks
        {
            get { return _selectedCarWorks; }
            set
            {
                if (_selectedCarWorks == value) return;
                _selectedCarWorks = value;
                OnPropertyChanged("SelectedCarWorks");
                OnPropertyChanged("EnableEditCarWorks");
            }
        }

        private void _carstorage_LoadComplete(object sender, EventArgs e)
        {
            OnPropertyChanged("Cars");
        }

        public List<DISP_Car> Cars
        {
            get
            {
                return _carstorage.Cars;
            }
        }

        private DISP_Car _selectedCar = null;
        public DISP_Car SelectedCar
        {
            get { return _selectedCar; }
            set
            {
                if (_selectedCar == value) return;
                _selectedCar = value;
                OnPropertyChanged("SelectedCar");
                OnPropertyChanged("CompleteSaveEnabled");
            }
        }

        public KVPBase Mark
        {
            get { return _storage.SelectedMark; }
            set
            {
                //if (value != null)
                //    MarkView = Visibility.Collapsed;
                //else
                //    MarkView = Visibility.Visible;
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
                //if (value != null)
                //    ModelView = Visibility.Collapsed;
                //else
                //    ModelView = Visibility.Visible;
                if (_storage.SelectedModel == value) return;
                _storage.SelectedModel = value;
                OnPropertyChanged("Model");
            }
        }

        private bool _selectedAllCars = true;
        public bool SelectedAllCars
        {
            get { return _selectedAllCars; }
            set
            {
                _selectedAllCars = value;
                OnPropertyChanged("SelectedAllCars");
                //SetCarEnabled();
            }
        }

        private bool _selectedModelCars = false;
        public bool SelectedModelCars
        {
            get { return _selectedModelCars; }
            set
            {
                _selectedModelCars = value;
                OnPropertyChanged("SelectedModelCars");
                //SetCarEnabled();
            }
        }

        private bool _enableAddModel = false;
        public bool EnableAddModel
        {
            get { return _enableAddModel; }
            set
            {
                _enableAddModel = value;
                OnPropertyChanged("EnableAddModel");
            }
        }

        private bool _visAllowAddAuto = true;
        public bool VisAllowAddAuto
        {
            get { return _visAllowAddAuto; }
            set
            {
                _visAllowAddAuto = value;
                OnPropertyChanged("VisAllowAddAuto");
            }
        }

        private KVPBase _selectedWorkType = null;
        public KVPBase SelectedWorkType
        {
            get { return _selectedWorkType; }
            set
            {
                _selectedWorkType = value;
                OnPropertyChanged("SelectedWorkType");
                OnPropertyChanged("Works");
            }
        }

        private WorksInfoDataModel _selectedWork = null;
        public WorksInfoDataModel SelectedWork
        {
            get { return _selectedWork; }
            set
            {
                _selectedWork = value;
                OnPropertyChanged("SelectedWork");
                OnPropertyChanged("IsEnableAddWork");
            }
        }

        public bool IsEnableAddWork
        {
            get { return SelectedWork != null; }
        }

        public bool CompleteSaveEnabled
        {
            get { return CarWorks.Count > 0; }
        }

        private const string NameAddCar = "Добавить новый авто";
        private const string NameAdd = "Свернуть";

        private string _nameAddCommand = NameAddCar;
        public string NameAddCommand
        {
            get { return _nameAddCommand; }
            set
            {
                _nameAddCommand = value;
                OnPropertyChanged("NameAddCommand");
            }
        }

        public bool EnableEditCarWorks
        {
            get { return SelectedCarWorks != null; }
        }

        private RelayCommand _completeSaveCommand;
        public RelayCommand CompleteSaveCommand
        {
            get { return _completeSaveCommand ?? (_completeSaveCommand = new RelayCommand(Save)); }
        }

        private void Save(object parameter)
        {

        }

        private RelayCommand _addWorkCommand;
        public RelayCommand AddWorkCommand
        {
            get { return _addWorkCommand ?? (_addWorkCommand = new RelayCommand(AddWork)); }
        }

        private void AddWork(object parameter)
        {
            var model = new WorksInfoDataCostModel {Name = SelectedWork.Name, IdWork = SelectedWork.idWork};
            if(SelectedCar != null)
            {
                model.Mark = SelectedCar.CarModel.Mark;
                model.Mark = SelectedCar.CarModel.Model;
            }
            CarWorks.Add(model);
            SelectedCarWorks = model;
        }

        private RelayCommand _addCarCommand;
        public RelayCommand AddCarCommand
        {
            get { return _addCarCommand ?? (_addCarCommand = new RelayCommand(AddCar)); }
        }

        private void AddCar(object obj)
        {
            VisAllowAddAuto = NameAddCommand.Equals(NameAddCar);
            NameAddCommand = NameAddCommand.Equals(NameAddCar) ? NameAdd : NameAddCar;
        }
        
    }
}
