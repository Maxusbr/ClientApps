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
            _storage.LoadModelsComplete += _storage_LoadModelsComplete;

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

        private readonly ObservableCollection<WorksInfoDataCostViewModel> _carWorks = new ObservableCollection<WorksInfoDataCostViewModel>();
        public ObservableCollection<WorksInfoDataCostViewModel> CarWorks { get { return _carWorks; } }

        private WorksInfoDataCostViewModel _selectedCarWorks = null;
        public WorksInfoDataCostViewModel SelectedCarWorks
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
                if(value == null)
                {
                    return;
                }
                FiltrMark();
            }
        }

        private void FiltrMark()
        {
            SelectedAllCars = SelectedCar == null;
            SelectedModelCars = !SelectedAllCars;
            if(SelectedCar == null) return;
            Mark = Marks.FirstOrDefault(o => o.Name.Equals(SelectedCar.CarModel.Mark));
        }


        private void _storage_LoadModelsComplete(object sender, EventArgs e)
        {
            if (SelectedCar == null) return;
            Model = Models.FirstOrDefault(o => o.Name.Equals(SelectedCar.CarModel.Model));
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
                if(value) DeselectMarkModel();
            }
        }

        private void DeselectMarkModel()
        {
            SelectedCar = null;
            Model = Mark = null;
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

        public bool IsEnableAddWork
        {
            get { return SelectedWork != null; }
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
            CompleteSaveEnabled = false;
        }

        private RelayCommand _addWorkCommand;
        public RelayCommand AddWorkCommand
        {
            get { return _addWorkCommand ?? (_addWorkCommand = new RelayCommand(AddWork)); }
        }

        private void AddWork(object parameter)
        {
            var model = new WorksInfoDataCostViewModel { Name = SelectedWork.Name, IdWork = SelectedWork.idWork };
            if(SelectedCar != null)
            {
                model.Mark = SelectedCar.CarModel.Mark;
                model.Mark = SelectedCar.CarModel.Model;
            }
            model.PropertyChanged += model_PropertyChanged;
            CarWorks.Add(model);
            CompleteSaveEnabled = true;
            SelectedCarWorks = model;
        }

        private void model_PropertyChanged(object sender, EventArgs e)
        {
            var model = sender as WorksInfoDataCostViewModel;
            if(model == null) return;
            model.IsChanged = CompleteSaveEnabled = true;
        }

        private RelayCommand _addCarCommand;
        public RelayCommand AddCarCommand
        {
            get { return _addCarCommand ?? (_addCarCommand = new RelayCommand(AddCar)); }
        }

        private void AddCar(object obj)
        {
            SelectedCar = null;
            VisAllowAddAuto = NameAddCommand.Equals(NameAddCar);
            NameAddCommand = NameAddCommand.Equals(NameAddCar) ? NameAdd : NameAddCar;
        }
 
    }
}
