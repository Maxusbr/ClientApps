using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Xml;
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
            _storage.LoadWorkPartsListComplete += Instance_LoadWorkPartsListComplete;
            Cars.Add(new CarListBaseDataModel { Mark = AllCar, ID = 0, Model = "" });
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                Cars.Add(new CarListBaseDataModel { CarNumber = "qqqq", Mark = "Audio", Model = "A3" });
                CarWorks.Add(new WorksInfoDataCostViewModel { Name = "To All", Model = "", Mark = AllCar, ID = 0 });
                CarWorks.Add(new WorksInfoDataCostViewModel { Name = "To Audi", Model = "A4", Mark = "Audi", ID = 1 });
            }
        }

        private const string AllCar = "Для всех авто";
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
                if (value != null)
                    Mark = Marks.FirstOrDefault(o => o.Name.Equals(value.Mark));
            }
        }

        private CollectionViewSource _carWorksView;
        public ICollectionView CarWorksView
        {
            get
            {
                if (_carWorksView == null)
                {
                    _carWorksView = new CollectionViewSource { Source = CarWorks };
                }
                return _carWorksView.View;
            }
        }

        private void _carstorage_LoadComplete(object sender, EventArgs e)
        {
            OnPropertyChanged("Cars");
        }

        private readonly ObservableCollection<CarListBaseDataModel> _cars = new ObservableCollection<CarListBaseDataModel>();
        public ObservableCollection<CarListBaseDataModel> Cars
        {
            get
            {
                return _cars;
            }
        }

        private CarListBaseDataModel _selectedCar = null;
        public CarListBaseDataModel SelectedCar
        {
            get { return _selectedCar; }
            set
            {
                if (_selectedCar == value) return;
                _selectedCar = value;
                OnPropertyChanged("SelectedCar");
                if (value == null) return;
                Mark = Model = null;
                FiltrMark();
            }
        }

        private void FiltrMark()
        {
            Mark = Marks.FirstOrDefault(o => o.Name.Equals(SelectedCar.Mark));
            CarWorksView.Filter = o =>
            {
                var el = o as WorksInfoDataCostViewModel;
                if (el == null) return false;
                if (SelectedCar.Mark.Equals(AllCar) || el.Mark.Equals(AllCar)) return true;
                return el.Mark.Equals(SelectedCar.Mark) && el.Model.Equals(SelectedCar.Model);
            };
            CarWorksView.Refresh();
        }


        private void _storage_LoadModelsComplete(object sender, EventArgs e)
        {
            if (SelectedCar != null && !SelectedCar.Mark.Equals(AllCar))
                Model = Models.FirstOrDefault(o => o.Name.Equals(SelectedCar.Model));
            if (SelectedCarWorks != null)
                Model = Models.FirstOrDefault(o => o.Name.Equals(SelectedCarWorks.Model));
        }

        public KVPBase Mark
        {
            get { return _storage.SelectedMark; }
            set
            {
                if (_storage.SelectedMark == value) return;
                _storage.SelectedMark = value;
                OnPropertyChanged("Mark");
                SelectedAllCars = value == null;
                SelectedModelCars = !SelectedAllCars;
            }
        }

        public KVPBase Model
        {
            get { return _storage.SelectedModel; }
            set
            {
                if (_storage.SelectedModel == value) return;
                _storage.SelectedModel = value;
                OnPropertyChanged("Model");
                UpdateCarWork();
            }
        }

        private void UpdateCarWork()
        {
            if (SelectedCarWorks == null) return;
            if (Mark != null)
            {
                SelectedCarWorks.Mark = Mark.Name;
                SelectedCarWorks.IsRoot = SelectedCarWorks.Mark.Equals(AllCar);
            }
            if (Model != null) SelectedCarWorks.Model = Model.Name;
        }

        private bool _selectedAllCars = true;
        public bool SelectedAllCars
        {
            get { return _selectedAllCars; }
            set
            {
                _selectedAllCars = value;
                OnPropertyChanged("SelectedAllCars");
                if (value) Model = Mark = null;
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
                NHCost = 0;
                NH = "";
                if (value != null)
                    SpecificationDataStorage.Instance.GetWorkParts(value.id);
            }
        }

        private void Instance_LoadWorkPartsListComplete(List<WorksInfoDataModel> data)
        {
            var sum = data.Sum(model => model.NH)/10.0;
            NH = sum > 0 ? sum.ToString(): "";
            NHCost = (int)(sum * UserSettingsStorage.Instance.UserSettings.PersonModel.NHCost);
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
            foreach (var el in CarWorks.Where(o => o.IsChanged))
            {
                el.IsRoot = el.Mark.Equals(AllCar);
                var car = Cars.FirstOrDefault(o => o.Mark.Equals(el.Mark) && o.Model.Equals(el.Model));
                if (car == null) Cars.Add(new CarListBaseDataModel { Mark = el.Mark, Model = el.Model, ID = Cars.Count });
                var model = Newtonsoft.Json.JsonConvert.SerializeObject(el);
                var res = model;
            }
            CarWorks.Where(w => w.IsChanged).ToList().ForEach(o => o.IsChanged = false);
            SelectedCarWorks = null;
        }

        private RelayCommand _addWorkCommand;
        public RelayCommand AddWorkCommand
        {
            get { return _addWorkCommand ?? (_addWorkCommand = new RelayCommand(AddWork)); }
        }

        private void AddWork(object parameter)
        {
            var model = new WorksInfoDataCostViewModel
            {
                Name = SelectedWork.Name,
                IdWork = SelectedWork.idWork,
                IsChanged = true,
                Mark = Mark != null ? Mark.Name : AllCar,
                Model = Model != null ? Model.Name : "",
                ID = CarWorks.Count,
                CostWork = NHCost
            };
            model.IsRoot = model.Mark.Equals(AllCar);
            var exist = CarWorks.FirstOrDefault(o => o.Equals(model));
            CompleteSaveEnabled = true;
            if (exist != null)
            {
                SelectedCarWorks = exist;
                return;
            }
            model.PropertyChanged += model_PropertyChanged;
            CarWorks.Add(model);
            SelectedCarWorks = model;
        }

        private void model_PropertyChanged(object sender, EventArgs e)
        {
            var model = sender as WorksInfoDataCostViewModel;
            if (model == null) return;
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

        private string _nh;
        public string NH
        {
            get
            {
                return string.IsNullOrEmpty(_nh) || _nh.Equals("0") ? "": string.Format("количество нормо-часов - {0}", _nh);
            }
            set
            {
                _nh = value;
                OnPropertyChanged("NH");
            }
        }

        private int _nhCost;
        public int NHCost
        {
            get { return _nhCost; }
            set
            {
                _nhCost = value;
                
                OnPropertyChanged("NHCost");
            }
        }
    }
}
