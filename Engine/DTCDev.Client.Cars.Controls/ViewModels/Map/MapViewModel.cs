using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using DTCDev.Client.Cars.Controls.Helpers;
using DTCDev.Client.Cars.Controls.Models;
using DTCDev.Client.Cars.Engine.AppLogic;
using DTCDev.Client.Cars.Engine.DisplayModels;
using DTCDev.Client.Cars.Engine.Handlers.Cars;
using DTCDev.Client.Controls.Map;
using DTCDev.Client.ViewModel;

namespace DTCDev.Client.Cars.Controls.ViewModels.Map
{
    public class MapViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

        public MapViewModel()
        {
            MapCenter = MapCenterUser = new Location(55.75, 37.62);

            Zones.Add(new VmPolyline(Zones.Count));
            
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
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
                zoneHandler = ZonesHandler.Instance;
                mapHandler = CarsHandler.Instance;
                EnableTracking = true;
                zoneHandler.LoadCompleted += zoneHandler_LoadCompleted;
                zoneHandler.Update();
                CarSelector.OnCarChanged += CarSelector_OnCarChanged;
            //}
        }

        void CarSelector_OnCarChanged(DISP_Car car)
        {
            if (car != SelectedMapObject)
            {
                SelectedMapObject = car;
            }
        }

        private Location mapCenter;
        public Location MapCenter
        {
            get { return mapCenter; }
            set
            {
                if (mapCenter != value)
                {
                    mapCenter = value;
                    OnPropertyChanged("MapCenter");
                }
            }
        }

        private Location mapCenterUser;
        public Location MapCenterUser
        {
            get { return mapCenterUser; }
            set
            {
                if (mapCenterUser != value)
                {
                    mapCenterUser = value;
                    OnPropertyChanged("MapCenterUser");
                }
            }
        }

        public ObservableCollection<DISP_Car> Points
        {
            get { return mapHandler.Cars; }
        }
        
        DISP_Car selectedMapObject;
        public DISP_Car SelectedMapObject
        {
            get
            {
                return selectedMapObject;
            }
            set
            {
                if (this.selectedMapObject != value)
                {
                    if (this.selectedMapObject != null)
                    {
                        this.selectedMapObject.PropertyChanged -= selectedMapObject_PropertyChanged;
                        this.selectedMapObject.ZoneData.InZone = true;
                    }

                    this.selectedMapObject = value;
                    if (value != null)
                    {
                        MapCenter = MapCenterUser = this.selectedMapObject.Navigation.LocationPoint;
                        this.selectedMapObject.PropertyChanged += selectedMapObject_PropertyChanged;
                        if (SelectedZone != null)
                            GetMoreInfo(this.selectedMapObject);
                    }
                    OnPropertyChanged("SelectedMapObject");
                }
                CarSelector.SelectedCar = value;
            }
        }

        void GetMoreInfo(DISP_Car obj)
        {
            obj.ZoneData.InZone = CalcLeavingZone.Instance.FillContains(this.selectedMapObject.Navigation.LocationPoint, SelectedZone.MovedLocations);
            obj.Adress = GeoAdress.Instance.GetAdress(obj.Navigation.LocationPoint);
        }

        void selectedMapObject_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Location"))
            {
                DISP_Car obj = sender as DISP_Car;
                if (obj != null)
                {
                    MapCenter = MapCenterUser = obj.Navigation.LocationPoint;
                    if (SelectedZone != null)
                        GetMoreInfo(this.selectedMapObject);
                }
            }
        }

        private bool enableTracking = true;
        public bool EnableTracking
        {
            get
            {
                return this.enableTracking;
            }
            set
            {
                if (value != this.enableTracking)
                {
                    this.enableTracking = value;
                    OnPropertyChanged("EnableTracking");
                }
                if (!this.enableTracking)
                {
                    SelectedMapObject = null;
                }

            }
        }

        ZonesHandler zoneHandler = ZonesHandler.Instance;
        CarsHandler mapHandler = CarsHandler.Instance;

        void zoneHandler_LoadCompleted(string ResultOperation)
        {
            switch (ResultOperation)
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
                return zoneHandler.Zones; 
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
            get { return this._selectedZone; }
            set
            {
                if (this._selectedZone != value)
                {
                    this._selectedZone = value;
                    ChangeSelectedZone();
                    OnPropertyChanged("SelectedZone");
                }
            }
        }

        private void ChangeSelectedZone()
        {
            ZoneSelect.Clear();
            if (_selectedZone == null) return;
            ZoneSelect.Add(_selectedZone);
            if (_selectedZone.MovedLocations != null && _selectedZone.MovedLocations.Count >= 0)
            {
                var centerLocation = _selectedZone.MovedLocations.GetCenter();
                if (centerLocation.Latitude != 0 && centerLocation.Longitude != 0)
                    MapCenterUser = centerLocation;
            }
        }

        private double _intervalRefresh = 5;
        public double IntervalRefresh
        {
            get
            {
                return this._intervalRefresh;
            }
            set
            {
                double val = Math.Max(value - value % 5, 5);
                if (val != this._intervalRefresh)
                {
                    this._intervalRefresh = val;
                    mapHandler.SetInterval(val * 1000);
                    OnPropertyChanged("IntervalRefresh");
                }
            }
        }

        private void mapHandler_LoadCompleted(DISP_Car[] ListMapObjects)
        {

        }

        private Location currentLocation;
        public Location CurrentLocation
        {
            get { return currentLocation; }
            set
            {
                if (currentLocation != value)
                {
                    currentLocation = value;
                    OnPropertyChanged("CurrentLocation");
                }
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
            get
            {
                if (_showListZoneCommand == null)
                {
                    _showListZoneCommand = new RelayCommand(ShowListZone);
                }
                return _showListZoneCommand;
            }
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
            get
            {
                if (_showLogCommand == null)
                {
                    _showLogCommand = new RelayCommand(ShowLog);
                }
                return _showLogCommand;
            }
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

        private string house = string.Empty;
        public string House
        {
            get { return house; }
            set
            {
                if (house != value)
                {
                    house = value;
                    OnPropertyChanged("House");
                }
            }
        }

        private string street = string.Empty;
        public string Street
        {
            get { return street; }
            set
            {
                if (street != value)
                {
                    street = value;
                    OnPropertyChanged("Street");
                }
            }
        }

        private string city = string.Empty;
        public string City
        {
            get { return city; }
            set
            {
                if (city != value)
                {
                    city = value;
                    OnPropertyChanged("City");
                }
            }
        }

        private Location coordinate;
        public Location Coordinate
        {
            get { return coordinate; }
            set
            {
                if (coordinate != value)
                {
                    coordinate = value;
                    if (!this.EnableTracking)
                        this.MapCenter = MapCenterUser = value;
                    OnPropertyChanged("Coordinate");
                }
            }
        }

        private ObservableCollection<Location> coordinates = new ObservableCollection<Location>();
        public ObservableCollection<Location> Coordinates
        {
            get { return coordinates; }
        }
        private ICommand _SearchGeoCommand;
        

        public ICommand SearchGeoCommand
        {
            get
            {
                if (_SearchGeoCommand == null)
                {
                    _SearchGeoCommand = new RelayCommand(this.SearchGeo);
                }
                return _SearchGeoCommand;
            }
        }

        private void SearchGeo(object obj)
        {
            System.Collections.Generic.List<Location> res = new System.Collections.Generic.List<Location>();
            if (!string.IsNullOrEmpty(this.House) && !string.IsNullOrEmpty(this.Street) && !string.IsNullOrEmpty(this.City))
                res = GeoAdress.Instance.GetCoordinat(this.House, this.Street, this.City);
            if (res != null)
            {
                Coordinates.Clear();
                foreach (Location el in res)
                    Coordinates.Add(el);
                if (Coordinates.Count > 0)
                    Coordinate = Coordinates[0];

            }
        }


        #endregion
    }
}
