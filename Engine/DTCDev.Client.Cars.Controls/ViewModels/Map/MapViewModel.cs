using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Threading;
using DTCDev.Client.Cars.Controls.Helpers;
using DTCDev.Client.Cars.Controls.Models;
using DTCDev.Client.Cars.Engine.AppLogic;
using DTCDev.Client.Cars.Engine.DisplayModels;
using DTCDev.Client.Cars.Engine.Handlers.Cars;
using DTCDev.Client.Controls.Map;
using DTCDev.Client.ViewModel;

namespace DTCDev.Client.Cars.Controls.ViewModels.Map
{
    public class MapViewModel : ViewModelBase
    {
        public MapViewModel()
        {
            _zoneHandler = ZonesHandler.Instance;
            _mapHandler = CarsHandler.Instance;
            MapCenter = MapCenterUser = new Location(55.75, 37.62);
            Zones.Add(new VmPolyline(Zones.Count));
            
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
                this.SelectedZone = Zones[0];

                DISP_Car car = new DISP_Car();
                car.Navigation.LocationPoint = new Location(55.758, 37.76);
                car.Name = "obj-1";
                Points.Add(car);

                DISP_Car car1 = new DISP_Car();
                car1.Navigation.LocationPoint = new Location(55.75, 37.5);
                car1.Name = "obj-2";
                Points.Add(car1);

                DISP_Car car2 = new DISP_Car();
                car2.Navigation.LocationPoint = new Location(55.8, 37.625);
                car2.Name = "obj-3";
                Points.Add(car2);

                Zones[0].AddLocation(new Location(55.758, 37.76), false);
                Zones[0].AddLocation(new Location(55.75, 37.5), false);
                Zones[0].AddLocation(new Location(55.8, 37.625), false);
                //ZoneSelect.Add(Zones[0]);

            }
            //else
            //{
                //mapHandler.LoadCompleted += mapHandler_LoadCompleted;
                
                EnableTracking = true;
                _zoneHandler.LoadCompleted += zoneHandler_LoadCompleted;
                _zoneHandler.Update();
                CarSelector.OnCarChanged += CarSelector_OnCarChanged;
            //}
        }


        void CarSelector_OnCarChanged(DISP_Car car)
        {
            if (SelectedCar != car)
            {
                SelectedCar = car;
            }
        }

        private Location _mapCenter;
        public Location MapCenter
        {
            get { return _mapCenter; }
            set
            {
                if (_mapCenter == value) return;
                _mapCenter = value;
                OnPropertyChanged("MapCenter");
            }
        }

        private Location _mapCenterUser;
        public Location MapCenterUser
        {
            get { return _mapCenterUser; }
            set
            {
                if (_mapCenterUser == value) return;
                _mapCenterUser = value;
                OnPropertyChanged("MapCenterUser");
            }
        }

        public ObservableCollection<DISP_Car> Points
        {
            get { return _mapHandler.Cars; }
        }
        
        DISP_Car _selectedCar;
        public DISP_Car SelectedCar
        {
            get
            {
                return _selectedCar;
            }
            set
            {
                if (_selectedCar == value) return;
                if (_selectedCar != null)
                {
                    _selectedCar.PropertyChanged -= selectedMapObject_PropertyChanged;
                    _selectedCar.ZoneData.InZone = true;
                    _selectedCar.IsSelected = false;
                }

                _selectedCar = value;
                if (value != null)
                {
                    MapCenter = MapCenterUser = _selectedCar.Navigation.LocationPoint;
                    _selectedCar.IsSelected = true;
                    _selectedCar.PropertyChanged += selectedMapObject_PropertyChanged;
                    if (SelectedZone != null)
                        GetMoreInfo(_selectedCar);
                }
                OnPropertyChanged("SelectedCar");
                //CarSelector.SelectedCar = value;
            }
        }

        void GetMoreInfo(DISP_Car obj)
        {
            obj.ZoneData.InZone = CalcLeavingZone.Instance.FillContains(this._selectedCar.Navigation.LocationPoint, SelectedZone.MovedLocations);
            obj.Adress = GeoAdress.Instance.GetAdress(obj.Navigation.LocationPoint);
        }

        void selectedMapObject_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!e.PropertyName.Equals("Location")) return;
            var obj = sender as DISP_Car;
            if (obj == null || obj.Navigation == null) return;
            MapCenter = MapCenterUser = obj.Navigation.LocationPoint;
            if (SelectedZone != null)
                GetMoreInfo(_selectedCar);
        }

        private bool _enableTracking = true;
        public bool EnableTracking
        {
            get
            {
                return _enableTracking;
            }
            set
            {
                if (value != _enableTracking)
                {
                    _enableTracking = value;
                    OnPropertyChanged("EnableTracking");
                }
                if (!_enableTracking)
                {
                    SelectedCar = null;
                }

            }
        }

        readonly ZonesHandler _zoneHandler;
        readonly CarsHandler _mapHandler;

        void zoneHandler_LoadCompleted(string resultOperation)
        {
            switch (resultOperation)
            {
                case "AddComplete":
                    break;
                case "UpdateComplete":
                    break;
                case "DeleteComplete":
                    break;
                case "GetComplete":
                    OnPropertyChanged("Zones");
                    break;
                case "Error":
                    break;
            }
        }

        public ObservableCollection<VmPolyline> Zones
        {
            get 
            { 
                return _zoneHandler.Zones; 
            }
        }

        private readonly ObservableCollection<VmPolyline> _zoneSelects = new ObservableCollection<VmPolyline>();
        public ObservableCollection<VmPolyline> ZoneSelect
        {
            get
            {
                return _zoneSelects;
            }
        }

        private VmPolyline _selectedZone;
        public VmPolyline SelectedZone
        {
            get { return _selectedZone; }
            set
            {
                if (_selectedZone == value) return;
                _selectedZone = value;
                ChangeSelectedZone();
                OnPropertyChanged("SelectedZone");
            }
        }

        private void ChangeSelectedZone()
        {
            ZoneSelect.Clear();
            if (_selectedZone == null) return;
            ZoneSelect.Add(_selectedZone);
            if (_selectedZone.MovedLocations == null || _selectedZone.MovedLocations.Count < 0) return;
            var centerLocation = _selectedZone.MovedLocations.GetCenter();
            if (centerLocation.Latitude != 0 && centerLocation.Longitude != 0)
                MapCenterUser = centerLocation;
        }

        private double _intervalRefresh = 5;
        public double IntervalRefresh
        {
            get
            {
                return _intervalRefresh;
            }
            set
            {
                double val = Math.Max(value - value % 5, 5);
                if (val != _intervalRefresh)
                {
                    _intervalRefresh = val;
                    _mapHandler.SetInterval(val * 1000);
                    OnPropertyChanged("IntervalRefresh");
                }
            }
        }

        private Location _currentLocation;
        public Location CurrentLocation
        {
            get { return _currentLocation; }
            set
            {
                if (_currentLocation == value) return;
                _currentLocation = value;
                OnPropertyChanged("CurrentLocation");
            }
        }

        private string _nameShowButton = "Зоны";
        public string NameShowButton
        {
            get { return _nameShowButton; }
            set
            {
                if (_nameShowButton == value) return;
                _nameShowButton = value;
                OnPropertyChanged("NameShowButton");
            }
        }
        private ICommand _showListZoneCommand;
        public ICommand ShowListZoneCommand
        {
            get { return _showListZoneCommand ?? (_showListZoneCommand = new RelayCommand(ShowListZone)); }
        }

        private void ShowListZone(object obj)
        {
            if (NameShowButton.Equals("Зоны"))
            {
                NameShowButton = "Закрыть";
                VisableListZone = true;
                PosLB = 0;
            }
            else
            {
                NameShowButton = "Зоны";
                VisableListZone = false;
                PosLB = -190;
                SelectedZone = null;
            }
        }

        private string _nameLogButton = "Показать лог";
        public string NameLogButton
        {
            get { return _nameLogButton; }
            set
            {
                if (_nameLogButton == value) return;
                _nameLogButton = value;
                OnPropertyChanged("NameLogButton");
            }
        }

        private ICommand _showLogCommand;
        public ICommand ShowLogCommand
        {
            get { return _showLogCommand ?? (_showLogCommand = new RelayCommand(ShowLog)); }
        }

        private void ShowLog(object obj)
        {
            if (NameLogButton.Equals("Показать лог"))
            {
                NameLogButton = "Скрыть лог";
                OnPropertyChanged("ShowErrorLog");
            }
            else
            {
                NameLogButton = "Показать лог";
                OnPropertyChanged("HideErrorLog");
            }
        }

        private double _posLb = -190;
        public double PosLB
        {
            get { return _posLb; }
            set
            {
                if (_posLb == value) return;
                _posLb = value;
                OnPropertyChanged("PosLB");
            }
        }

        private bool _visableListZone = false;
        public bool VisableListZone
        {
            get { return _visableListZone; }
            set
            {
                if (_visableListZone == value) return;
                _visableListZone = value;
                OnPropertyChanged("VisableListZone");
            }
        }

        //private ICommand _SelectionPointsChange;
        //public ICommand SelectionPointsChange
        //{
        //    get
        //    {
        //        if (_SelectionPointsChange == null)
        //        {
        //            _SelectionPointsChange = new RelayCommand(this.PointSelection);
        //        }
        //        return _SelectionPointsChange;
        //    }
        //}

        //private void PointSelection(object obj)
        //{

        //}

        //private ICommand _LeftMouseButtonClickCommand;
        //public ICommand LeftMouseButtonClickCommand
        //{
        //    get
        //    {
        //        if (_LeftMouseButtonClickCommand == null)
        //        {
        //            _LeftMouseButtonClickCommand = new RelayCommand(this.LeftMouseButtonClick);
        //        }
        //        return _LeftMouseButtonClickCommand;
        //    }
        //}

        //private void LeftMouseButtonClick(object obj)
        //{
        //    if (SelectedZone != null && EnableEditZone)
        //    {
        //        MouseButtonEventArgs e = obj as MouseButtonEventArgs;
        //        DTCDev.Client.Controls.Map.Map sender = e.Source as DTCDev.Client.Controls.Map.Map;
        //        if (sender != null)
        //        {
        //            System.Windows.Point mousePosition = e.GetPosition(sender);
        //            CurrentLocation = sender.ViewportPointToLocation(mousePosition);
        //            SelectedZone.AddLocation(CurrentLocation, true);
        //        }
        //    }
        //}

        //private bool enableEditZone = false;
        //public bool EnableEditZone
        //{
        //    get { return enableEditZone; }
        //    set
        //    {
        //        if (this.enableEditZone != value)
        //        {
        //            this.enableEditZone = value;
        //            if (!value)
        //            {
        //                foreach (var el in Zones.Where(o => o.EnableEdit))
        //                    el.EnableEdit = false;
        //                SelectedZone = null;
        //            }
        //            else
        //            {
        //                if (Zones.Count > 0)
        //                    SelectedZone = Zones[0];
        //            }

        //            OnPropertyChanged("EnableEditZone");
        //        }
        //    }
        //}


        //private ICommand _AddZoneCommand;
        //public ICommand AddZoneCommand
        //{
        //    get
        //    {
        //        if (_AddZoneCommand == null)
        //        {
        //            _AddZoneCommand = new RelayCommand(this.AddZone);
        //        }
        //        return _AddZoneCommand;
        //    }
        //}

        //private void AddZone(object obj)
        //{
        //    Zones.Add(new VmPolyline(Zones.Count));
        //}

        //private ICommand _RemoveZoneCommand;
        //public ICommand RemoveZoneCommand
        //{
        //    get
        //    {
        //        if (_RemoveZoneCommand == null)
        //        {
        //            _RemoveZoneCommand = new RelayCommand(this.RemoveZone);
        //        }
        //        return _RemoveZoneCommand;
        //    }
        //}

        //private void RemoveZone(object obj)
        //{
        //    if (SelectedZone != null && Zones.IndexOf(SelectedZone) >= 0 && Zones.Count > 1)
        //    {
        //        //zoneHandler.DeleteZone(SelectedZone);
        //        //Zones.Remove(SelectedZone);
        //    }
        //}

        //private ICommand _SaveZoneCommand;
        //public ICommand SaveZoneCommand
        //{
        //    get
        //    {
        //        if (_SaveZoneCommand == null)
        //        {
        //            _SaveZoneCommand = new RelayCommand(this.SaveZone);
        //        }
        //        return _SaveZoneCommand;
        //    }
        //}

        //private void SaveZone(object obj)
        //{
        //    foreach (var el in Zones.Where(w => w.IsChanged))
        //        if (el.ID != 0)
        //            zoneHandler.EditZone(el);
        //        else
        //            zoneHandler.AddZone(el);
        //}

        #region Helpers Seach Geo Coordinates

        private string _house = string.Empty;
        public string House
        {
            get { return _house; }
            set
            {
                if (_house == value) return;
                _house = value;
                OnPropertyChanged("House");
            }
        }

        private string _street = string.Empty;
        public string Street
        {
            get { return _street; }
            set
            {
                if (_street == value) return;
                _street = value;
                OnPropertyChanged("Street");
            }
        }

        private string _city = string.Empty;
        public string City
        {
            get { return _city; }
            set
            {
                if (_city == value) return;
                _city = value;
                OnPropertyChanged("City");
            }
        }

        private Location _coordinate;
        public Location Coordinate
        {
            get { return _coordinate; }
            set
            {
                if (_coordinate == value) return;
                _coordinate = value;
                if (!EnableTracking)
                    MapCenter = MapCenterUser = value;
                OnPropertyChanged("Coordinate");
            }
        }

        private readonly ObservableCollection<Location> _coordinates = new ObservableCollection<Location>();
        public ObservableCollection<Location> Coordinates
        {
            get { return _coordinates; }
        }

        private ICommand _searchGeoCommand;
        public ICommand SearchGeoCommand
        {
            get { return _searchGeoCommand ?? (_searchGeoCommand = new RelayCommand(this.SearchGeo)); }
        }

        private void SearchGeo(object obj)
        {
            var res = new List<Location>();
            if (!string.IsNullOrEmpty(this.House) && !string.IsNullOrEmpty(this.Street) && !string.IsNullOrEmpty(this.City))
                res = GeoAdress.Instance.GetCoordinat(this.House, this.Street, this.City);
            if (res == null) return;
            Coordinates.Clear();
            foreach (var el in res)
                Coordinates.Add(el);
            if (Coordinates.Count > 0)
                Coordinate = Coordinates[0];
        }


        #endregion
    }
}
