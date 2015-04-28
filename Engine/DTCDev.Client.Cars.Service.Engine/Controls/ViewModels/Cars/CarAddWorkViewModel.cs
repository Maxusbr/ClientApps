using DTCDev.Client.Cars.Service.Engine.Handlers;
using DTCDev.Client.Cars.Service.Engine.Storage;
using DTCDev.Client.ViewModel;
using DTCDev.Models.CarBase.CarList;
using DTCDev.Models.CarBase.CarStatData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace DTCDev.Client.Cars.Service.Engine.Controls.ViewModels.Cars
{
    public class CarAddWorkViewModel : ViewModelBase
    {
        public CarAddWorkViewModel()
        {
            Works = new ObservableCollection<ViewCarWorkModel>();
            NotPeriodicWorks = new ObservableCollection<ViewCarWorkModel>();
            OrderWorks = new ObservableCollection<ViewCarWorkModel>();
            WorkParts = new ObservableCollection<WorksInfoDataModel>();
            CarStorage.Instance.CarsWorksUpdated += Instance_CarsWorksUpdated;
            SpecificationDataStorage.Instance.LoadWorkPartsListComplete += Instance_LoadWorkPartsListComplete;
            CarStorage.Instance.UpdateCarWorks();
        }

        void Instance_LoadWorkPartsListComplete(List<WorksInfoDataModel> data)
        {
            FillWorkParts(data);
        }

        void Instance_CarsWorksUpdated(object sender, EventArgs e)
        {
            OrderWorks.Clear();
            FilterWorks();
        }

        public ObservableCollection<ViewCarWorkModel> Works { get; set; }

        public ObservableCollection<ViewCarWorkModel> NotPeriodicWorks { get; set; }

        public ObservableCollection<ViewCarWorkModel> OrderWorks { get; set; }

        public ObservableCollection<WorksInfoDataModel> WorkParts { get; set; }

        private DateTime _dtMake = DateTime.Now;
        public DateTime DTMake
        {
            get { return _dtMake; }
            set
            {
                _dtMake = value;
                this.OnPropertyChanged("DTMake");
            }
        }

        private int _currentDistance = CarStorage.Instance.SelectedCar.CarModel.CurrentDistance;
        public int CurrentDistance
        {
            get { return _currentDistance; }
            set
            {
                _currentDistance = value;
                this.OnPropertyChanged("CurrentDistance");
            }
        }
        
        private bool _deleteEnabled = false;
        public bool DeleteEnabled
        {
            get { return _deleteEnabled; }
            set
            {
                _deleteEnabled = value;
                this.OnPropertyChanged("DeleteEnabled");
            }
        }

        private ViewCarWorkModel _selectedAddedWork;
        public ViewCarWorkModel SelectedAddedWork
        {
            get { return _selectedAddedWork; }
            set
            {
                _selectedAddedWork = value;
                this.OnPropertyChanged("SelectedAddedWork");
                if (value == null)
                    DeleteEnabled = false;
                else
                {
                    DeleteEnabled = true;
                    DisplayWorkParts(value);
                }
            }
        }

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                if (_searchText == value) return;
                _searchText = value;
                OnPropertyChanged("SearchText");
                FilterWorks();
            }
        }




        public event EventHandler CreateOrderComplete;






        private RelayCommand _addToListCommand;
        public RelayCommand AddToListCommand { get { return _addToListCommand ?? (_addToListCommand = new RelayCommand(AddToList)); } }
        private void AddToList(object sender)
        {
            List<ViewCarWorkModel> periodic = Works.Where(p => p.Selected == true).ToList();
            List<ViewCarWorkModel> unperiodic = NotPeriodicWorks.Where(p => p.Selected == true).ToList();
            OrderWorks.Clear();
            foreach (var item in periodic)
            {
                OrderWorks.Add(item);
                LoadWorkData(item);
            }
            foreach (var item in unperiodic)
            {
                OrderWorks.Add(item);
                LoadWorkData(item);
            }
        }

        private RelayCommand _deleteWorkCommand;
        public RelayCommand DeleteWorkCommand { get { return _deleteWorkCommand ?? (_deleteWorkCommand = new RelayCommand(DeleteWork)); } }
        private void DeleteWork(object sender)
        {
            if (SelectedAddedWork == null)
                return;
            Works.Add(SelectedAddedWork);
            SelectedAddedWork.Selected = false;
            OrderWorks.Remove(SelectedAddedWork);
        }

        private RelayCommand _saveOrderCommand;
        public RelayCommand SaveOrderCommand { get { return _saveOrderCommand ?? (_saveOrderCommand = new RelayCommand(SaveOrder)); } }
        private void SaveOrder(object sender)
        {
            TotalSaveOrder(false);
        }

        private RelayCommand _saveAndCloseOrderCommand;
        public RelayCommand SaveAndCloseOrderCommand { get { return _saveAndCloseOrderCommand ?? (_saveAndCloseOrderCommand = new RelayCommand(SaveOrder)); } }
        private void SaveAndCloseOrder(object sender)
        {
            TotalSaveOrder(true);
        }

        private void TotalSaveOrder(bool isClose)
        {
            CarWorksCompleteModel model = new CarWorksCompleteModel();
            model.CarNumber = CarStorage.Instance.SelectedCar.CarModel.CarNumber;
            model.Comment = "";
            model.Date = new Models.Date.DateDataModel { D = DTMake.Day, M = DTMake.Month, Y = DTMake.Year };
            model.DistanceMake = CurrentDistance;
            model.OrderNo = 0;
            model.WorkIds = new List<CarWorksCompleteModel.WorkItemModel>();
            foreach (var item in OrderWorks)
            {
                CarWorksCompleteModel.WorkItemModel m = new CarWorksCompleteModel.WorkItemModel
                {
                    Comment = "",
                    Cost = item.Cost,
                    id = item.WGUID

                };
                model.WorkIds.Add(m);
            }
            if (isClose == false)
                CarsHandler.Instance.AddWorks(model);
            else
                CarsHandler.Instance.AddWorksAndCloseOrder(model);
            if (CreateOrderComplete != null)
                CreateOrderComplete(this, new EventArgs());
        }






        private void FilterWorks()
        {
            Works.Clear();
            NotPeriodicWorks.Clear();
            List<WorksWithFlagDataModel> periodic = CarStorage.Instance.CarWorks.Where(p => p.IsPeriodic == 1).ToList();
            List<WorksWithFlagDataModel> unperiodic = CarStorage.Instance.CarWorks.Where(p => p.IsPeriodic == 0).ToList();



            foreach (var item in periodic)
            {
                bool add = true;
                if (SearchText != null)
                    if (item.Name.ToUpper().IndexOf(SearchText.ToUpper()) == -1)
                        add = false;
                if(add)
                    Works.Add(new ViewCarWorkModel
                    {
                        Periodic = true,
                        Name = item.Name,
                        Id = item.id,
                        id_Class = item.id_Class,
                        NHours = item.NH,
                        WGUID = item.WGUID
                    });
            }

            foreach (var item in unperiodic)
            {
                bool add = true;
                if (SearchText != null)
                    if (item.Name.ToUpper().IndexOf(SearchText.ToUpper()) == -1)
                        add = false;
                if (add)
                    NotPeriodicWorks.Add(new ViewCarWorkModel
                    {
                        Periodic = false,
                        Name = item.Name,
                        Id = item.id,
                        id_Class = item.id_Class,
                        NHours = item.NH,
                        WGUID = item.WGUID
                    });
            }
        }

        private void LoadWorkData(ViewCarWorkModel model)
        {
            if (model.WorkParts.Count() == 0)
                SpecificationDataStorage.Instance.GetWorkParts(model.Id, CarStorage.Instance.SelectedCar.CarModel.CarNumber, model.Periodic);
            else
                DisplayWorkParts(model);
        }

        private void FillWorkParts(List<WorksInfoDataModel> data)
        {
            foreach (var item in data)
            {
                ViewCarWorkModel model = Works.Where(p => p.Id == item.idWork).FirstOrDefault();
                if (model != null)
                    model.AddWorkPart(item);
                else
                {
                    model = OrderWorks.Where(p => p.Id == item.idWork).FirstOrDefault();
                    if (model != null)
                        model.AddWorkPart(item);
                }
            }
        }

        private void DisplayWorkParts(ViewCarWorkModel model)
        {
            WorkParts.Clear();
            if (model == null)
                return;
            foreach (var item in model.WorkParts)
            {
                item.NHD = item.NH / 10.0m;
                WorkParts.Add(item);
            }
        }


       



        public class ViewCarWorkModel : ViewModelBase
        {
            public string Name { get; set; }

            public bool Periodic { get; set; }

            private bool _selected = false;
            public bool Selected
            {
                get { return _selected; }
                set
                {
                    _selected = value;
                    this.OnPropertyChanged("Selected");
                }
            }

            public int Id{get;set;}

            public string WGUID { get; set; }

            public int id_Class{get;set;}

            private decimal _nhours = 0;

            public decimal NHours
            {
                get { return _nhours; }
                set
                {
                    _nhours = value;
                    this.OnPropertyChanged("NHours");
                }
            }

            private int _cost = 0;

            public int Cost
            {
                get { return _cost; }
                set
                {
                    _cost = value;
                    this.OnPropertyChanged("Cost");
                }
            }

            private List<WorksInfoDataModel> _workParts = new List<WorksInfoDataModel>();
            public List<WorksInfoDataModel> WorkParts
            {
                get { return _workParts; }
                set 
                { 
                    _workParts = value;
                    CalcNH();
                }
            }

            public void AddWorkPart(WorksInfoDataModel model)
            {
                WorkParts.Add(model);
                CalcNH();
            }

            private void CalcNH()
            {
                NHours = 0;
                foreach (var item in WorkParts)
                {
                    NHours += item.NH;
                }
                NHours = NHours / 10.0m;
                Cost = (int)(NHours * UserSettingsStorage.Instance.UserSettings.PersonModel.NHCost);
            }
        }
    }
}
