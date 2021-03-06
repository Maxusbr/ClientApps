﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using System.Xml.Serialization;
using DTCDev.Client.Cars.Controls.Helpers;
using DTCDev.Client.Cars.Controls.Models;
using DTCDev.Client.Cars.Controls.ViewModels.Car;
using DTCDev.Client.Cars.Engine.AppLogic;
using DTCDev.Client.Cars.Engine.DisplayModels;
using DTCDev.Client.Cars.Engine.Handlers.Cars;
using DTCDev.Client.Controls.Map;
using DTCDev.Client.ViewModel;
using DTCDev.Models.CarsSending.Car;
using DTCDev.Models.CarsSending.Navigation;
using DTCDev.Models.Date;
using Microsoft.Office.Interop.Excel;
using RelayCommand = DTCDev.Client.ViewModel.RelayCommand;
using DTCDev.Models;
using Action = System.Action;
using Application = System.Windows.Application;
using Newtonsoft.Json;

namespace DTCDev.Client.Cars.Controls.ViewModels.History
{
    public class HistoryViewModel : ViewModelBase
    {
        //public HistoryViewModel()
        //{
        //    _zoneHandler = ZonesHandler.Instance;
        //    _mapHandler = CarsHandler.Instance;
        //    MapCenter = MapCenterUser = new Location(55.75, 37.62);
        //    CarSelector.OnCarChanged += CarSelector_OnCarChanged;
        //    HistoryHandler.Instance.LoadCompleted += Instance_LoadCompleted;
        //    HistoryHandler.Instance.DayRefreshed += Instance_DayRefreshed;
        //    HistoryHandler.Instance.LinesLoaded += Instance_LinesLoaded;
        //    HistoryHandler.Instance.OBDLoaded += Instance_OBDLoaded;
        //    HistoryHandler.Instance.AccLoaded += Instance_AccLoaded;

        //    TableHistory.PropertyChanged += TableHistory_PropertyChanged;
        //    if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
        //    {
        //        SelectedHistoryRow = new LoadedHistoryRows();
        //        Position.Navigation.LocationPoint =
        //        MapCenter = MapCenterUser = new Location(55.75, 37.62);

        //        DISP_Car car = new DISP_Car(Dispatcher.CurrentDispatcher);
        //        car.Navigation.LocationPoint = new Location(55.758, 37.76);
        //        car.Name = "obj-1";
        //        Points.Add(car);

        //        DISP_Car car1 = new DISP_Car(Dispatcher.CurrentDispatcher);
        //        car1.Navigation.LocationPoint = new Location(55.75, 37.5);
        //        car1.Name = "obj-2";
        //        Points.Add(car1);

        //        DISP_Car car2 = new DISP_Car(Dispatcher.CurrentDispatcher);
        //        car2.Navigation.LocationPoint = new Location(55.8, 37.625);
        //        car2.Name = "obj-3";
        //        Points.Add(car2);

        //        Position = Points[0];
        //        Route.Add(new Location(55.75, 37.62, true));
        //        Route.Add(new Location(55.73, 37.63));
        //        Route.Add(new Location(55.73, 37.60));
        //        WarningRoute.Add(new Location(55.76, 37.63, true));
        //        WarningRoute.Add(new Location(55.75, 37.63));
        //        ErrorRoute.Add(new Location(55.75, 37.62, true));
        //        ErrorRoute.Add(new Location(55.76, 37.63));
        //        Parkings.Add(new ParkingModel(DateTime.Today, new Location(55.76, 37.63)));
        //    }

        //    if (!_zoneHandler.Zones.Any())
        //        _zoneHandler.Update();
        //}
        

        public HistoryViewModel(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
            //_tableHistory = new HistoryRowsViewModel(dispatcher);
            _zoneHandler = ZonesHandler.Instance;
            _mapHandler = CarsHandler.Instance;
            _handler = HistoryHandler.Instance;
            _mapHandler.CarsRefreshed += _mapHandler_CarsRefreshed;
            MapCenter = MapCenterUser = new Location(55.75, 37.62);
            CarSelector.OnCarChanged += CarSelector_OnCarChanged;
            //_handler.LoadCompleted += Instance_LoadCompleted;
            //_handler.DayRefreshed += Instance_DayRefreshed;
            //_handler.LinesLoaded += Instance_LinesLoaded;
            _handler.OBDLoaded += Instance_OBDLoaded;
            _handler.AccLoaded += Instance_AccLoaded;
            _handler.DayStateChange += _handler_DayStateChange;
            _handler.SetDateTimePosition += _handler_SetDateTimePosition;

            //TableHistory.PropertyChanged += TableHistory_PropertyChanged;
            if (!_zoneHandler.Zones.Any())
                _zoneHandler.Update();

        }

        public void _handler_SetDateTimePosition(DateTime position)
        {
            IsPlayerStart = false;
            var slowTask = new Task(delegate
            {
                Thread.Sleep(50);
            });
            slowTask.ContinueWith(o => {
                if (_dispatcher != null)
                    _dispatcher.BeginInvoke(new Action(() =>
                    {
                        PlayerCurentTime = position;
                    }));
            });
            slowTask.Start();
            //UpdateCurentPosition(position);
        }

        void _handler_DayStateChange(List<CarStateModel> list)
        {
            DayStates = list;
            SortDataByDate(true);
            AccHistory = null;
            //OBDHistory = null;
            IsPlayerStart = false;
            OnPropertyChanged("VisablePlayer");
            OnPropertyChanged("IsCheckedSpeed");
            //if (_distanceCheckActive)
            //    DistanceSelectedDayChanged();
            
        }

        private void _mapHandler_CarsRefreshed(IEnumerable<DISP_Car> data)
        {
            Points.Clear();
            foreach (var el in data)
                Points.Add(el);
        }

        //void TableHistory_PropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    if (!e.PropertyName.Equals("SelectedRow") || TableHistory.SelectedRow == null) return;
        //    IsPlayerStart = false;
        //    Thread.Sleep(10);
        //    var item = DayStates.FirstOrDefault(o => o.Date == TableHistory.SelectedRow.Date);
        //    if (item == null) return;
        //    PlayerCurentTime = item.Date;
        //}

        void Instance_AccLoaded(CarAccHistoryModel model)
        {
            IsPlayerStart = false;
            var acc = new CarAccHistoryModel { DevID = model.DevID };
            var slowTask = new Task(delegate
            {
                for (var i = 0; i < DayStates.Count; i++)
                {
                    var item = DayStates[i];
                    var next = i + 1 < DayStates.Count ? DayStates[i + 1].Date : DateTime.Now;
                    var list = model.Data.Where(o => o.Date.ToDateTime() >= item.Date && o.Date.ToDateTime() < next);
                    foreach (var el in list)
                        acc.Data.Add(el);
                }
            });
            slowTask.ContinueWith(obj =>
            {
                if (_dispatcher != null)
                    _dispatcher.BeginInvoke(new Action(() =>
                    {
                        AccHistory = acc;
                        IsEnabledRadio = true;
                        if (!IsCheckedSpeed) UpdateRoutes(true);
                    }));
            });
            slowTask.Start();
        }

        void Instance_OBDLoaded(OBDHistoryDataModel model)
        {
            IsPlayerStart = false;
            var obd = new OBDHistoryDataModel { DevID = model.DevID, DT = model.DT };
            var converter = new PIDConverter();
            var prms = model.Data.Select(p => p.Code).Distinct().ToList();
            var slowTask = new Task(delegate
            {
                try
                {
                    foreach (var item in Routes)
                    {
                        var dt = new DateTime(item.StartDate.Year, item.StartDate.Month, item.StartDate.Day);
                        var list =
                            model.Data.Where(o => dt + o.Time.ToTimeSpan() >= item.StartDate && dt + o.Time.ToTimeSpan() < item.EndDate);
                        if(!list.Any()) continue;
                        foreach (var el in prms.Select(pr => list.FirstOrDefault(o => o.Code.Equals(pr))).Where(el => el != null))
                        {
                            item.AddTipValue(converter.GetPidInfo(el.Code), el.Vol);
                        }
                        //if(list.Any())
                        //    obd.Data.AddRange(list);
                    }
                }
                catch
                {
                }
            });
            slowTask.ContinueWith(obj =>
            {
                if (_dispatcher != null)
                    _dispatcher.BeginInvoke(new Action(() =>
                    {

                        //OBDHistory = obd;
                        //TableHistory.Update(obd);
                        //OnPropertyChanged("TableHistory");
                    }));
            });
            slowTask.Start();
        }

        //void Instance_LinesLoaded(DTCDev.Models.LinesDataModel model)
        //{
        //    Lines = model;
        //}




        #region Fields

        private readonly Dispatcher _dispatcher;

        private DateTime _dt;

        private DateTime _startDate;

        private DateTime _stopDate;

        private string _selectedDevice = "";

        private List<CarStateModel> _dayStates = new List<CarStateModel>();

        private DISP_Car _position;

        private bool _useAccelleration = true;

        private int _acceleration = 1;

        private CarStateModel _selectedState;

        private CarZoneHistoryErrorViewModel _historyErrorVm = new CarZoneHistoryErrorViewModel();

        private readonly ObservableCollection<VmPolyline> _zoneSelects = new ObservableCollection<VmPolyline>();

        private readonly ZonesHandler _zoneHandler;

        private readonly CarsHandler _mapHandler;

        private readonly HistoryHandler _handler;

        private double SpdNormal = 90;

        private double SpdWarning = 120;

        public Dispatcher Dispather;

        private int _spanMap = 3;

        private bool _enableHistory = false;

        private RoutesModel _routesModel = new RoutesModel();

        private DateTime _displayedHistoryDate;

        private readonly ObservableCollection<RouteLine> _routes = new ObservableCollection<RouteLine>();
        private readonly List<RouteLine> _tempRoutes = new List<RouteLine>();
        //private int _roundedSpeed = 0;

        //private int _maxSpeed = 0;

        //private int _distance = 0;

        //private string _loadedText = "";



        //private Location _currentLocation;

        //private bool _periodSet = false;

        //private bool _tablePackageView = false;

        //private ObservableCollection<LoadedHistoryRows> _historyRows = new ObservableCollection<LoadedHistoryRows>();

        //private LoadedHistoryRows _selectedRow;

        //private readonly HistoryRowsViewModel _tableHistory;
        #endregion




        #region Properties

        //public HistoryRowsViewModel TableHistory
        //{
        //    get { return _tableHistory; }
        //}

        //public string LoadedText
        //{
        //    get { return _loadedText; }
        //    set
        //    {
        //        _loadedText = value;
        //        this.OnPropertyChanged("LoadedText");
        //    }
        //}

        public bool EnableHistory
        {
            get
            {
                return _enableHistory;
            }
            set
            {
                _enableHistory = value;
                if (value && CarSelector.SelectedCar != null)
                {

                    //TODO: чего здесь вообще происходит? )))))
                    Position = new DISP_Car()
                    {
                        Car = new SCarModel()
                        {
                            CarNumber = CarSelector.SelectedCar.Car.CarNumber,
                            Id = CarSelector.SelectedCar.Car.Id
                        },
                        HistoryDetailView = true
                    };
                    Position.UpdateNavigation(CarSelector.SelectedCar.Navigation);
                    Position.UpdateZone(CarSelector.SelectedCar.ZoneData);
                }
                OnPropertyChanged("EnableHistory");
            }
        }

        private readonly ObservableCollection<DISP_Car> _cars = new ObservableCollection<DISP_Car>();
        public ObservableCollection<DISP_Car> Points
        {
            get { return _cars; }
        }

        ///// <summary>
        ///// Список загруженных дат
        ///// </summary>
        //public ObservableCollection<LoadedHistoryRows> HistoryRows
        //{
        //    get { return _historyRows; }
        //}

        //public LoadedHistoryRows SelectedHistoryRow
        //{
        //    get { return _selectedRow; }
        //    set
        //    {
        //        _selectedRow = value;
        //        TableHistory.Update(value);
        //        IsPlayerStart = false;
        //        OnPropertyChanged("SelectedHistoryRow");
        //        OnPropertyChanged("VisablePlayer");
        //        OnPropertyChanged("TableHistory");
        //        OnPropertyChanged("IsCheckedSpeed");
        //        SortData();
        //        if (_distanceCheckActive)
        //            DistanceSelectedDayChanged();
        //    }
        //}

        public bool VisablePlayer { get { return DayStates != null && DayStates.Any(); } }

        private MovedLocationCollection _roteOpacited = new MovedLocationCollection();
        public MovedLocationCollection RouteOpacited
        {
            get { return _roteOpacited; }
            set
            {
                _roteOpacited = value;
                this.OnPropertyChanged("RouteOpacited");
            }
        }

        //private LocationCollection _route = new LocationCollection();
        //public LocationCollection Route
        //{
        //    get { return _route; }
        //    set
        //    {
        //        _route = value;
        //        this.OnPropertyChanged("Route");
        //    }
        //}

        private LinesDataModel _lines = new LinesDataModel();

        public LinesDataModel Lines
        {
            get { return _lines; }
            set
            {
                _lines = value;
                OnPropertyChanged("Lines");
            }
        }

        //private OBDHistoryDataModel _obdHistory = new OBDHistoryDataModel();
        //public OBDHistoryDataModel OBDHistory
        //{
        //    get { return _obdHistory; }
        //    set
        //    {
        //        _obdHistory = value;
        //        OnPropertyChanged("OBDHistory");
        //    }
        //}

        private CarAccHistoryModel _accHistory = new CarAccHistoryModel();
        public CarAccHistoryModel AccHistory
        {
            get { return _accHistory; }
            set
            {
                _accHistory = value;
                OnPropertyChanged("AccHistory");
            }
        }

        //private LocationCollection _warningRoute = new LocationCollection();
        //public LocationCollection WarningRoute
        //{
        //    get { return _warningRoute; }
        //    set
        //    {
        //        _warningRoute = value;
        //        this.OnPropertyChanged("WarningRoute");
        //    }
        //}

        //private LocationCollection _errorRoute = new LocationCollection();
        //public LocationCollection ErrorRoute
        //{
        //    get { return _errorRoute; }
        //    set
        //    {
        //        _errorRoute = value;
        //        this.OnPropertyChanged("ErrorRoute");
        //    }
        //}

        //public LocationCollection _offlineRoute = new LocationCollection();
        //public LocationCollection OfflineRoute
        //{
        //    get { return _offlineRoute; }
        //    set
        //    {
        //        _offlineRoute = value;
        //        this.OnPropertyChanged("OfflineRoute");
        //    }
        //}

        private ObservableCollection<ParkingModel> _parkings = new ObservableCollection<ParkingModel>();
        public ObservableCollection<ParkingModel> Parkings
        {
            get { return _parkings; }
            set
            {
                _parkings = value;
                this.OnPropertyChanged("Parkings");
            }
        }


        public CarZoneHistoryErrorViewModel HistoryErrorVm
        {
            get { return _historyErrorVm; }
            set
            {
                _historyErrorVm = value;
                this.OnPropertyChanged("HistoryErrorVm");
            }
        }

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
                //if (this._selectedZone != value)
                {
                    this._selectedZone = value;
                    OnPropertyChanged("SelectedZone");
                    ChangeZoneSelected();
                }
            }
        }

        private void ChangeCarSelected()
        {
            if (Position == null || !EnableHistory) return;
            ClearSelect();
            var zone = _zoneHandler.Zones.FirstOrDefault(o => o.ID == Position.ZoneData.ZoneId);
            if (zone == null) return;
            if (!ZoneSelect.Contains(zone))
                ZoneSelect.Add(zone);
            SelectedZone = zone;
            zone.IsSelected = true;
            //GetMoreInfo(Position);
        }

        private void ChangeZoneSelected()
        {
            if (!EnableHistory) return;
            ClearSelect();
            if (_selectedZone == null) return;
            Points.Where(o => o.ZoneData.ZoneId == _selectedZone.ID).ToList().ForEach(GetMoreInfo);
            //MapCenterUser = _selectedZone.MovedLocations.GetCenter();
        }

        void GetMoreInfo(DISP_Car obj)
        {
            if (SelectedZone == null || obj == null) return;
            obj.ZoneData.InZone = CalcLeavingZone.Instance.FillContains(obj.Navigation.LocationPoint, SelectedZone.MovedLocations);
            //obj.Adress = GeoAdress.Instance.GetAdress(obj.Location);
        }

        private void ClearSelect()
        {
            ZoneSelect.ToList().ForEach(x => x.IsSelected = false);
            Points.ToList().ForEach(x => x.ZoneData.InZone = true);
            //OnPropertyChanged("IsCarZoneCanLink");
            //OnPropertyChanged("IsCarZoneCanUnLink");
        }

        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                _startDate = value;
                this.OnPropertyChanged("StartDate");
            }
        }

        public DateTime StopDate
        {
            get { return _stopDate; }
            set
            {
                _stopDate = value;
                this.OnPropertyChanged("StopDate");
            }
        }


        //public void SetDates(DateTime first, DateTime last)
        //{
        //    if (first <= last)
        //    {
        //        StartDate = first;
        //        StopDate = last;
        //    }
        //    else
        //    {
        //        StartDate = last;
        //        StopDate = first;
        //    }
        //    SelectedDatesRangeChanged();
        //    LoadData();
        //}

        /// <summary>
        /// displaying selected device name
        /// </summary>
        public string SelectedDevice
        {
            get { return _selectedDevice; }
            set
            {
                _selectedDevice = value;
                OnPropertyChanged("SelectedDevice");
                LoadData();
            }
        }

        /// <summary>
        /// current date for history
        /// </summary>
        public DateTime DisplayedHistoryDate
        {
            get { return _displayedHistoryDate; }
            set
            {
                _displayedHistoryDate = value;
                this.OnPropertyChanged("DisplayedHistoryDate");
            }
        }

        public List<CarStateModel> DayStates
        {
            get { return _dayStates; }
            set
            {
                _dayStates = value;
                OnPropertyChanged("DayStates");
                //if (TablePackageView)
                //    OnPropertyChanged("PackagesList");
            }
        }

        //public List<PackageViewModel> PackagesList
        //{
        //    get
        //    {
        //        //List<PackageViewModel> answer = new List<PackageViewModel>();

        //        return DayStates.Select(o => new PackageViewModel(o)).ToList();
        //    }
        //}

        private Location _mapCenter = new Location();
        public Location MapCenter
        {
            get
            {
                return _mapCenter;
            }
            set
            {
                _mapCenter = value;
                this.OnPropertyChanged("MapCenter");
            }
        }

        private Location _mapCenterUser = new Location();
        public Location MapCenterUser
        {
            get
            {
                return _mapCenterUser;
            }
            set
            {
                _mapCenterUser = value;
                OnPropertyChanged("MapCenterUser");
            }
        }

        public DISP_Car Position
        {

            get { return _position ?? (_position = new DISP_Car()); }
            set
            {
                if (_position != null)
                {
                    _position.UpdateVisableMap(false);
                    _position.IsSelected = false;
                    _position.PropertyChanged -= PositionOnPropertyChanged;
                }
                _position = value;
                OnPropertyChanged("Position");
                if (value == null) return;
                //_position.UpdateVisableMap(true);
                _position.IsSelected = true;
                //if (value.Navigation.LocationPoint != null)
                //    MapCenter = MapCenterUser = value.Navigation.LocationPoint;
                _position.PropertyChanged += PositionOnPropertyChanged;

            }
        }

        private void PositionOnPropertyChanged(object sender, PropertyChangedEventArgs arg)
        {
            if (arg.PropertyName.Equals("Navigation"))
                MapCenter = MapCenterUser = Position.Navigation.LocationPoint;
            if (arg.PropertyName.Equals("ZoneData"))
                ChangeCarSelected();
        }

        public bool UseAccelleration
        {
            get { return _useAccelleration; }
            set
            {
                _useAccelleration = value;
                OnPropertyChanged("UseAccelleration");
            }
        }

        //public bool TablePackageView
        //{
        //    get { return _tablePackageView; }
        //    set
        //    {
        //        _tablePackageView = value;
        //        OnPropertyChanged("TablePackageView");
        //        OnPropertyChanged("PackagesList");
        //    }
        //}
        //public int Accelleration
        //{
        //    get { return _acceleration; }
        //    set
        //    {
        //        _acceleration = value;
        //        OnPropertyChanged("Accelleration");
        //    }
        //}

        public CarStateModel SelectedState
        {
            get { return _selectedState; }
            set
            {
                _selectedState = value;
                OnPropertyChanged("SelectedState");
                Update();
            }
        }

        void Update()
        {
            if (SelectedState == null) return;
            Position.Data = new SCarData() { Navigation = new SNaviData() { Speed = SelectedState.Spd } };
            Position.Update();
        }

        //public int RoundedSpeed
        //{
        //    get { return _roundedSpeed; }
        //    set
        //    {
        //        _roundedSpeed = value;
        //        this.OnPropertyChanged("RoundedSpeed");
        //    }
        //}

        //public int MaxSpeed
        //{
        //    get { return _maxSpeed; }
        //    set
        //    {
        //        _maxSpeed = value;
        //        this.OnPropertyChanged("MaxSpeed");
        //    }
        //}

        //public int Distance
        //{
        //    get { return _distance; }
        //    set
        //    {
        //        _distance = value;
        //        this.OnPropertyChanged("Distance");
        //    }
        //}

        //private double _distanceAll = 0;
        //public double DistanceAll
        //{
        //    get { return Math.Round(_distanceAll, 2); }
        //    set
        //    {
        //        _distanceAll = value;
        //        this.OnPropertyChanged("DistanceAll");
        //    }
        //}

        //public Location CurrentLocation
        //{
        //    get { return _currentLocation; }
        //    set
        //    {
        //        _currentLocation = value;
        //        this.OnPropertyChanged("CurrentLocation");
        //    }
        //}

        //public bool PeriodSet
        //{
        //    get { return _periodSet; }
        //    set
        //    {
        //        _periodSet = value;
        //        OnPropertyChanged("PeriodSet");
        //        //SortData();
        //    }
        //}

        private int _zoomLevel = 11;

        public int ZoomLevel
        {
            get { return _zoomLevel; }
            set
            {
                _zoomLevel = value;
                this.OnPropertyChanged("ZoomLevel");
            }
        }

        //public int SpanMap
        //{
        //    get { return _spanMap; }
        //    set
        //    {
        //        _spanMap = value;
        //        this.OnPropertyChanged("SpanMap");
        //    }
        //}


        private decimal _leftValue = 80;
        public decimal LeftValue
        {
            get { return _leftValue; }
            set
            {
                _leftValue = value;
                OnPropertyChanged("LeftValue");
            }
        }

        private decimal _rightValue = 120;
        public decimal RightValue
        {
            get { return _rightValue; }
            set
            {
                _rightValue = value;
                OnPropertyChanged("RightValue");
            }
        }

        private decimal _minValue = 0;
        public decimal MinValue
        {
            get { return _minValue; }
            set
            {
                _minValue = value;
                OnPropertyChanged("MinValue");
            }
        }

        private decimal _maxValue = 150;
        public decimal MaxValue
        {
            get { return _maxValue; }
            set
            {
                _maxValue = value;
                OnPropertyChanged("MaxValue");
            }
        }

        private bool _isCheckedSpeed = true;
        public bool IsCheckedSpeed
        {
            get { return _isCheckedSpeed; }
            set
            {
                try
                {
                    if (_isCheckedSpeed == value) return;
                    _isCheckedSpeed = value;
                    if (!value) return;
                    _maxValue = DayStates != null ? Math.Max(DayStates.Max(o => o.Spd) / 10.0m, 150) : 150;
                    _minValue = 0;
                    _leftValue = 90;
                    _rightValue = 120;
                    OnPropertyChanged("IsCheckedSpeed");
                    UpdateRouteSpeed();
                }
                catch { }
            }
        }

        private bool _isCheckedWay;
        public bool IsCheckedWay
        {
            get { return _isCheckedWay; }
            set
            {
                if (_isCheckedWay == value) return;
                _isCheckedWay = value;

                if (!value || AccHistory == null || AccHistory.Data.Count == 0) return;
                Iswaiting = true;
                var el = AccHistory.Data.FirstOrDefault();
                _maxValue = el != null ? Math.Max(Math.Abs(el.MaxX) / 100.0m, 2) : 2;
                _minValue = 0;
                _leftValue = .5m;
                _rightValue = 1;
                OnPropertyChanged("IsCheckedWay");
                UpdateRouteWay();
            }
        }

        private bool _isCheckedAccelerate;
        public bool IsCheckedAccelerate
        {
            get { return _isCheckedAccelerate; }
            set
            {
                if (_isCheckedAccelerate == value) return;
                _isCheckedAccelerate = value;

                if (!value || AccHistory == null || AccHistory.Data.Count == 0) return;
                Iswaiting = true;
                var el = AccHistory.Data.FirstOrDefault();
                _maxValue = el != null ? Math.Max(Math.Max(Math.Abs(el.MaxY), Math.Abs(el.MaxZ)) / 100.0m, 2) : 2;
                _minValue = 0;
                _leftValue = .5m;
                _rightValue = 1;
                OnPropertyChanged("IsCheckedAccelerate");
                UpdateRouteAccelerate();
            }
        }

        private bool _isEnabledRadio;
        public bool IsEnabledRadio
        {
            get { return _isEnabledRadio; }
            set
            {
                if (_isEnabledRadio == value) return;
                _isEnabledRadio = value;
                OnPropertyChanged("IsEnabledRadio");
            }
        }

        private bool _iswaiting = false;
        public bool Iswaiting
        {
            get { return _iswaiting; }
            set
            {
                if (_iswaiting == value) return;
                _iswaiting = value;
                OnPropertyChanged("Iswaiting");
            }
        }

        private DateTime _playerStartTime = DateTime.Now;
        public DateTime PlayerStartTime
        {
            get { return _playerStartTime; }
            set
            {
                if (_playerStartTime == value) return;
                _playerStartTime = value;
                OnPropertyChanged("PlayerStartTime");
            }
        }

        private DateTime _playerEndTime = DateTime.Now;
        public DateTime PlayerEndTime
        {
            get { return _playerEndTime; }
            set
            {
                if (_playerEndTime == value) return;
                _playerEndTime = value;
                OnPropertyChanged("PlayerEndTime");
            }
        }

        private DateTime _playerCurentTime = DateTime.Now;
        public DateTime PlayerCurentTime
        {
            get { return _playerCurentTime; }
            set
            {
                if (_playerCurentTime == value) return;
                _playerCurentTime = value;
                OnPropertyChanged("PlayerCurentTime");
                UpdateCurentPosition(value);
            }
        }

        private bool _isPlayerStart;
        

        public bool IsPlayerStart
        {
            get { return _isPlayerStart; }
            set
            {
                if (_isPlayerStart == value) return;
                _isPlayerStart = value;
                OnPropertyChanged("IsPlayerStart");
            }
        }

        public ObservableCollection<RouteLine> Routes
        {
            get { return _routes; }
        }

        #endregion

        #region Режимы отображения траектории



        /// <summary>
        /// Обновление трека
        /// </summary>
        internal void UpdateRoutes(bool recalcValues)
        {
            ClearRoutes(false);
            if (AccHistory.Data.Count == 0) return;
            var el = AccHistory.Data.FirstOrDefault();
            if (IsCheckedSpeed)
            {
                Iswaiting = true;
                if (recalcValues)
                {
                    _maxValue = DayStates != null ? Math.Max(DayStates.Max(o => o.Spd) / 10.0m, 150) : 150;
                    _minValue = 0;
                    _leftValue = 90;
                    _rightValue = 120;
                    OnPropertyChanged("IsCheckedSpeed");
                }
                UpdateRouteSpeed();
            }
            if (IsCheckedWay)
            {
                Iswaiting = true;
                if (recalcValues)
                {
                    _maxValue = el != null ? Math.Max(Math.Abs(el.MaxX) / 100.0m, 2) : 2;
                    _minValue = 0;
                    _leftValue = .5m;
                    _rightValue = 1;
                    OnPropertyChanged("IsCheckedWay");
                }
                UpdateRouteWay();
            }
            if (IsCheckedAccelerate)
            {
                Iswaiting = true;
                if (recalcValues)
                {
                    _maxValue = el != null ? Math.Max(Math.Max(Math.Abs(el.MaxY), Math.Abs(el.MaxZ)) / 100.0m, 2) : 2;
                    _minValue = 0;
                    _leftValue = .5m;
                    _rightValue = 1;
                    OnPropertyChanged("IsCheckedAccelerate");
                }
                UpdateRouteAccelerate();
            }
        }

        /// <summary>
        /// для градации используются показания акселерометра по осям YZ
        /// </summary>
        private void UpdateRouteAccelerate()
        {
            ClearRoutes(false);
            if (AccHistory == null) return;
            SpdNormal = (double)LeftValue;
            SpdWarning = (double)RightValue;
            var slowTask = new Task(delegate
            {
                if (AccHistory == null) return;
                var firstLoc = DayStates.OrderBy(o => o.Date).FirstOrDefault();
                if (firstLoc == null) return;
                var first = AccHistory.Data.OrderBy(o => o.Date.ToDateTime()).FirstOrDefault();
                if (first == null) return;
                var curroute = RouteSelect.None;
                var prev = new Location
                {
                    Latitude = firstLoc.Lt / 10000.0,
                    Longitude = firstLoc.Ln / 10000.0,
                    FirstPoint = true
                };
                if (Position != null)
                    Position.Navigation.LocationPoint = prev;
                foreach (var itemLoc in DayStates.OrderBy(o => o.Date))
                {
                    if (!Iswaiting || AccHistory == null) break;
                    var item = AccHistory.Data.LastOrDefault(o => o.Date.ToDateTime() > firstLoc.Date && o.Date.ToDateTime() <= itemLoc.Date) ?? first;
                    var loc = new Location
                    {
                        Latitude = itemLoc.Lt / 10000.0,
                        Longitude = itemLoc.Ln / 10000.0
                    };
                    if (itemLoc.Spd > 0)
                    {
                        var el = SortLocation(prev, loc, Math.Max(Math.Abs(item.Y), Math.Abs(item.Z)) / 100.0, firstLoc.Date, item.Date.ToDateTime());
                        el.AddTipValue("Скорость", firstLoc.Spd / 10.0);
                        el.AddTipValue("Количество спутников", firstLoc.St);
                        _tempRoutes.Add(el);
                    }
                    prev = loc;
                    first = item;
                    firstLoc = itemLoc;
                }
            });
            slowTask.ContinueWith(o =>
            {
                if (_dispatcher != null)
                    _dispatcher.BeginInvoke(new Action(UpdateCenter));
            });
            slowTask.Start();
        }

        /// <summary>
        /// вместо скорости для цветовой градации используются показания акселерометра
        /// </summary>
        private void UpdateRouteWay()
        {
            ClearRoutes(false);
            SpdNormal = (int)LeftValue;
            SpdWarning = (int)RightValue;
            var slowTask = new Task(delegate
            {
                if (AccHistory == null) return;
                var firstLoc = DayStates.OrderBy(o => o.Date).FirstOrDefault();
                if (firstLoc == null) return;
                var first = AccHistory.Data.OrderBy(o => o.Date.ToDateTime()).FirstOrDefault();
                if (first == null) return;
                var prev = new Location
                {
                    Latitude = firstLoc.Lt / 10000.0,
                    Longitude = firstLoc.Ln / 10000.0,
                    FirstPoint = true
                };
                if (Position != null)
                    Position.Navigation.LocationPoint = prev;
                foreach (var itemLoc in DayStates.OrderBy(o => o.Date))
                {
                    if (!Iswaiting || AccHistory == null) break;
                    var item = AccHistory.Data.LastOrDefault(o => o.Date.ToDateTime() > firstLoc.Date && o.Date.ToDateTime() <= itemLoc.Date) ?? first;
                    var loc = new Location
                    {
                        Latitude = itemLoc.Lt / 10000.0,
                        Longitude = itemLoc.Ln / 10000.0
                    };
                    if (itemLoc.Spd > 0)
                    {
                        var el = SortLocation(prev, loc, Math.Abs(item.X) / 100.0, firstLoc.Date, item.Date.ToDateTime());
                        el.AddTipValue("Скорость", firstLoc.Spd / 10.0);
                        el.AddTipValue("Количество спутников", firstLoc.St);
                        _tempRoutes.Add(el);
                    }
                    prev = loc;
                    first = item;
                    firstLoc = itemLoc;
                }
            });
            slowTask.ContinueWith(o =>
            {
                if (_dispatcher != null)
                    _dispatcher.BeginInvoke(new Action(UpdateCenter));
            });
            slowTask.Start();
        }

        /// <summary>
        /// текущая модель отображения трека
        /// </summary>
        private void UpdateRouteSpeed()
        {
            SpdNormal = (int)LeftValue;
            SpdWarning = (int)RightValue;
            SortDataByDate(false);
        }

        /// <summary>
        /// Clear all routes for map
        /// </summary>
        private void ClearRoutes(bool clearParcing = true)
        {
            Routes.Clear(); _tempRoutes.Clear();
            _routesModel = new RoutesModel();
            //Route.Clear();
            //WarningRoute.Clear();
            //ErrorRoute.Clear();
            RouteOpacited.Clear();
            if (clearParcing)
                Parkings.Clear();
        }
        #endregion


        #region Events

        public delegate void UpdateCenterEvent(Location southWest, Location northEast);
        public event UpdateCenterEvent CenterUpdates;
        protected virtual void OnCenterUpdates(Location southwest, Location northeast)
        {
            if (CenterUpdates != null) CenterUpdates(southwest, northeast);
        }

        #endregion




        #region Commands

        //private RelayCommand _loadNext10Command;

        //public RelayCommand LoadNext10Command { get { return _loadNext10Command ?? (_loadNext10Command = new RelayCommand(a => LoadNext10Days())); } }




        //private RelayCommand _showMaxSpeedCommand;

        //public RelayCommand ShowMaxSpeedCommad
        //{
        //    get
        //    {
        //        if (_showMaxSpeedCommand == null)
        //            _showMaxSpeedCommand = new RelayCommand(a => this.ShowMaxSpeed());
        //        return _showMaxSpeedCommand;
        //    }
        //}



        #endregion








        #region Private Functions

        /// <summary>
        /// Метод запроса истории еще за 10 дней
        /// </summary>
        //private void LoadNext10Days()
        //{
        //    if (!EnableHistory) return;

        //    if (CarSelector.SelectedCar != null && CarSelector.SelectedCar.Car.Id != String.Empty)
        //    {
        //        int days = 10;
        //        for (int i = 0; i < days; i++)
        //        {
        //            GetCache(_lastLoadedDate - TimeSpan.FromDays(i + 1));
        //        }
        //        HistoryHandler.Instance.StartLoadHistory(CarSelector.SelectedCar.Car.Id, _lastLoadedDate - TimeSpan.FromDays(days + 1), _lastLoadedDate - TimeSpan.FromDays(1), UseAccelleration);
        //    }
        //}

        /// <summary>
        /// Update visual states when selected date is changed
        /// </summary>
        //private void SelectedDatesRangeChanged()
        //{
        //    string displayedDate = "";
        //    if (_startDate != null)
        //        displayedDate = _startDate.ToString("dd.MM.yyyy");
        //    displayedDate += " - ";
        //    if (_stopDate != null)
        //        displayedDate += _stopDate.ToString("dd.MM.yyyy");
        //}





        void CarSelector_OnCarChanged(DISP_Car car)
        {
            if (!EnableHistory) return;
            IsPlayerStart = false;
            ZoneSelect.Clear();
            if (car != null)
            {
                Position = new DISP_Car()
                {
                    Car = new SCarModel()
                    {
                        CarNumber = car.Car.CarNumber,
                        Id = car.Car.Id
                    },
                    HistoryDetailView = true
                };
                Position.UpdateNavigation(car.Navigation);
                Position.UpdateZone(car.ZoneData);
                if (car.Car != null)
                    SelectedDevice = car.Car.CarNumber;
                //TODO Раскоментировать и заменить после привязки машин к зонам 
                //var zone = _zoneHandler.Zones.FirstOrDefault(o => o.ID == car.ZoneData.ZoneId);
            }
            else
                SelectedDevice = "";
            UpdateErrorControl();
        }

        private void UpdateErrorControl()
        {
            if (Position == null || ZoneSelect.Count == 0 || RouteOpacited.Count == 0) return;
            HistoryErrorVm.Update(Position, ZoneSelect[0], RouteOpacited);
        }

        /// <summary>
        /// Create request for history data
        /// </summary>
        public void LoadData()
        {
            if (!EnableHistory) return;

            if (CarSelector.SelectedCar != null && CarSelector.SelectedCar.Car.Id != String.Empty)
            {
                Iswaiting = IsEnabledRadio = false;
                //SelectedHistoryRow = null;
                //HistoryRows.Clear();
                //DistanceAll = 0;
                DayStates.Clear();
                //Route.Clear();
                //WarningRoute.Clear();
                //OfflineRoute.Clear();
                //ErrorRoute.Clear();
                Routes.Clear();
                Parkings.Clear();
                //int days = 30;
                //for (int i = 0; i < 30; i++)
                //{
                //    GetCache(DateTime.Now - TimeSpan.FromDays(i));
                //}
                //SortDataByDate(false);
                //Загружаем историю за последние 30 дней
                //HistoryHandler.Instance.StartLoadHistory(CarSelector.SelectedCar.Car.Id, DateTime.Now - TimeSpan.FromDays(days), DateTime.Now, UseAccelleration);
            }
        }



        ////loading data of history completed
        //void Instance_LoadCompleted(object sender, EventArgs e)
        //{
        //    LoadedText = "Обновление завершено";
        //}

        /// <summary>
        /// Дата, на каоторой остановилась загрузка
        /// </summary>
        //private DateTime _lastLoadedDate = DateTime.Now;

        //refreshed one day history
        //void Instance_DayRefreshed(DateTime day, List<CarStateModel> data)
        //{
        //    BuildHistoryRow(data, day);
        //    if (CarSelector.SelectedCar != null)
        //        CacheRoute(string.Format("[{0}]-{1}-{2}-{3}", CarSelector.SelectedCar.ID, day.Day, day.Month, day.Year), data);
        //    _lastLoadedDate = day;
        //    LoadedText = "Обновляю " + day.ToString("dd.MM.yy");
        //}


        //private void SortData()
        //{
        //    if (SelectedHistoryRow == null)
        //        return;
        //    else
        //    {
        //        DayStates = SelectedHistoryRow.Data;
        //        SortDataByDate(true);
        //    }
        //    AccHistory = null;
        //    OBDHistory = null;
        //    //HistoryHandler.Instance.StartLoadDayLines(Position.Car.Id, SelectedHistoryRow.Date);
        //    //HistoryHandler.Instance.StartLoadOBD(Position.Car.Id, SelectedHistoryRow.Date);
        //    //HistoryHandler.Instance.StartLoadAcc(Position.Car.Id, SelectedHistoryRow.Date);

        //}

        private void SortDataByDate(bool savecache)
        {
            ClearRoutes();

            var first = DayStates.OrderBy(o => o.Date).FirstOrDefault();
            if (first != null)
            {
                var curroute = RouteSelect.None;
                var prev = new Location
                {
                    Latitude = first.Lt / 10000.0,
                    Longitude = first.Ln / 10000.0,
                    FirstPoint = true
                };

                if (Position != null)
                    Position.Navigation.LocationPoint = prev;

                double dist = 0;
                var dc = new DistanceCalculator();
                var slowTask = new Task(delegate
                {
                    lock (_routesModel)
                    {
                        var foundParking = true;
                        var nullDate = first.Spd < 6 || StateDateTimeHelper.EqualInterval(DisplayedHistoryDate, first)
                            ? first : null;
                        int indx;
                        foreach (var item in DayStates.OrderBy(o => o.Date))
                        {
                            var itemtime = StateDateTimeHelper.GetTime(item);
                            var spantime = itemtime - StateDateTimeHelper.GetTime(first);
                            var loc = new Location
                            {
                                Latitude = item.Lt / 10000.0,
                                Longitude = item.Ln / 10000.0
                            };
                            if (DayStates.IndexOf(item) > 0)
                            {
                                nullDate = nullDate ?? first;
                                var firsttime = StateDateTimeHelper.GetTime(nullDate);
                                var isParking = StateDateTimeHelper.IsParking(item, ref nullDate);
                                if (foundParking && isParking)
                                {
                                    var selectedParkingModel = new ParkingModel(firsttime, itemtime, prev);
                                    var trash = _routesModel.Parkings.FirstOrDefault(o => o.Equals(selectedParkingModel));
                                    if (trash == null)
                                    {
                                        _routesModel.Parkings.Add(selectedParkingModel);
                                        foundParking = false;
                                    }
                                }
                                else if (!foundParking && !isParking)
                                {
                                    foundParking = true;
                                    nullDate = null;
                                    if (_routesModel.Parkings.Any())
                                        _routesModel.Parkings[_routesModel.Parkings.Count - 1].SetEndDates(itemtime);
                                }
                                var curdist = dc.Calculate(first, item);
                                if (item.Spd > 0 && Math.Abs(curdist) > .001 && !(foundParking && isParking))
                                {
                                    if (IsCheckedSpeed)
                                    {
                                        var el = SortLocation(prev, loc, first.Spd / 10.0, first.Date, item.Date);
                                        el.AddTipValue("Скорость", first.Spd/10.0);
                                        el.AddTipValue("Количество спутников", first.St);
                                        _tempRoutes.Add(el);
                                    }
                                    dist += curdist;
                                    prev = loc;
                                }
                                first = item;
                            }
                            RouteOpacited.Add(new MovedLocation(loc)
                            {
                                Dates = new DateTime(item.yy, item.Mnth, item.dd, item.hh, item.mm, item.ss)
                            });

                        }
                        var last = DayStates.LastOrDefault();
                        if (last == null) return;
                        if (!StateDateTimeHelper.EqualInterval(DateTime.Now, last)) return;
                        prev = new Location
                        {
                            Latitude = last.Lt / 10000.0,
                            Longitude = last.Ln / 10000.0,
                        };
                        if (StateDateTimeHelper.EqualInterval(StopDate, last))
                            _routesModel.Parkings.Add(new ParkingModel(StateDateTimeHelper.GetTime(last), StopDate, prev));
                            
                    }
                });
                slowTask.ContinueWith(o =>
                {
                    ContinueSortData((int)dist);
                    if (!savecache) return;
                    var strt = DayStates.First();
                });
                slowTask.Start();
            }
            //else
            //{
            //    MaxSpeed = 0;
            //    RoundedSpeed = 0;
            //    Distance = 0;
            //}
        }

        private void ContinueSortData(int dist)
        {
            DispatherThreadRun(delegate {
                UpdateCenter();
                UpdateErrorControl();
            });
            
            //MaxSpeed = DayStates.Max(p => p.Spd) / 10;
            //RoundedSpeed = DayStates.Sum(p => p.Spd) / (10 * DayStates.Count());

            //Distance = dist;

            //var strt = DayStates.First();
            //var stp = DayStates.Last();
            //var dtStrt = new DateTime(1, 1, 1, strt.hh, strt.mm, 0);
            //var stStp = new DateTime(1, 1, 1, stp.hh, stp.mm, 0);
        }

        //private void CacheRoute(string name, List<CarStateModel> data)
        //{
        //    var slowTask = new Task(delegate
        //    {
        //        var myDocs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\M2B\\Cache\\";
        //        try
        //        {

        //            if (Directory.Exists(myDocs) == false)
        //                Directory.CreateDirectory(myDocs);
        //            if (File.Exists(myDocs + name))
        //                File.Delete(myDocs + name);

        //            using (var writer = new StreamWriter(myDocs + name))
        //            {
        //                string row = JsonConvert.SerializeObject(data);
        //                writer.WriteLine(row);
        //            }

        //        }
        //        catch (Exception)
        //        {

        //        }
        //    });
        //    slowTask.Start();
        //}

        //private void GetCache(DateTime date)
        //{
        //    var slowTask = new Task(delegate
        //    {
        //        try
        //        {

        //            var myDocs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\M2B\\Cache\\";
        //            var name = string.Format("[{0}]-{1}-{2}-{3}", CarSelector.SelectedCar.ID, date.Day, date.Month,
        //                    date.Year);
        //            if (!File.Exists(myDocs + name)) return;
        //            using (var reader = new StreamReader(myDocs + name))
        //            {
        //                var cr = JsonConvert.DeserializeObject<List<CarStateModel>>(reader.ReadToEnd());
        //                if (cr == null) return;
        //                BuildHistoryRow(cr, date);

        //            }

        //        }
        //        catch { }
        //    }); slowTask.Start();
        //}

        //private void BuildHistoryRow(List<CarStateModel> data, DateTime date)
        //{
        //    var r = new LoadedHistoryRows { Date = date, Data = data };
        //    var slowTask = new Task(delegate
        //    {
        //        try
        //        {
        //            var dc = new DistanceCalculator();
        //            for (var i = 0; i < data.Count() - 1; i++)
        //            {
        //                r.Mileage += dc.Calculate(data[i], data[i + 1]);
        //            }
        //        }
        //        catch
        //        {
        //        }
        //        if (!data.Any()) return;
        //        try
        //        {
        //            r.MiddleSpeed = data.Sum(p => p.Spd) / 10 / data.Count();
        //            var moving = data.Where(p => p.Spd > 10).ToList();
        //            if (!moving.Any()) return;
        //            var hStart = moving.Min(p => p.hh);
        //            var minStart = moving.Where(p => p.hh == hStart).Min(p => p.mm);
        //            var hStop = moving.Max(p => p.hh);
        //            var mStop = moving.Where(p => p.hh == hStop).Max(p => p.mm);

        //            r.Start = hStart.ToString() + ":" + minStart.ToString();
        //            r.Stop = hStop.ToString() + ":" + mStop.ToString();


        //        }
        //        catch
        //        {
        //        }
        //    });
        //    slowTask.ContinueWith(delegate
        //    {
        //        if (_dispatcher != null)
        //            _dispatcher.BeginInvoke(new Action(() =>
        //            {
        //                var item =
        //                    HistoryRows.OrderBy(s => s.Date)
        //                        .LastOrDefault(o => o.StringDate.Equals(r.StringDate) || o.Date < r.Date);
        //                if (item != null)
        //                {
        //                    var indx = HistoryRows.IndexOf(item);
        //                    var selected = HistoryRows[indx].Equals(SelectedHistoryRow);
        //                    DistanceAll -= HistoryRows[indx].Mileage;
        //                    HistoryRows[indx] = r;
        //                    if (selected) SelectedHistoryRow = r;
        //                    DistanceAll += r.Mileage;
        //                }
        //                else
        //                {
        //                    HistoryRows.Add(r);
        //                    DistanceAll += r.Mileage;
        //                }
        //            }));
        //    });
        //    slowTask.Start();
        //    //bool replaced = false;
        //    //for (int i = 0; i < HistoryRows.Count(); i++)
        //    //{
        //    //    var selected = HistoryRows[i].Equals(SelectedHistoryRow);
        //    //    if (HistoryRows[i].StringDate == r.StringDate)
        //    //    {
        //    //        DistanceAll -= HistoryRows[i].Mileage;
        //    //        //HistoryRows.RemoveAt(i);
        //    //        //HistoryRows.Insert(i, r);
        //    //        HistoryRows[i] = r;
        //    //        if (selected) SelectedHistoryRow = r;
        //    //        replaced = true;
        //    //        DistanceAll += r.Mileage;
        //    //        break;
        //    //    }
        //    //}
        //    //if (replaced == false)
        //    //{
        //    //    bool inserted = false;
        //    //    for (int i = 0; i < HistoryRows.Count(); i++)
        //    //    {
        //    //        if (HistoryRows[i].Date < r.Date)
        //    //        {
        //    //            HistoryRows.Insert(i, r);
        //    //            inserted = true;
        //    //            break;
        //    //        }
        //    //    }
        //    //    if (inserted == false)
        //    //        HistoryRows.Add(r);
        //    //    DistanceAll += r.Mileage;
        //    //}
        //}

        private RouteLine SortLocation(Location prev, Location loc, double spd, DateTime dt, DateTime sdt)
        {
            //_routesModel.TimeRoute.Add(new RoutePoint { Point = prev, Date = dt });
            var curroute = GetRouteClass(spd);
            var line = new RouteLine(_dispatcher) {StartDate = dt, TypeRoute = curroute, EndDate = sdt};
            line.Points.Add(prev);line.Points.Add(loc);
            return line;
        }            

            
            //if (prevroute != curroute)
            //{
            //    loc.FirstPoint = false;
            //    AddToRoute(loc, prevroute);
            //    //if(spd > 0)
            //    AddToRoute(new Location(prev, true), curroute);
            //}
            ////if(spd > 0)
            //AddToRoute(loc, curroute);


        /// <summary>
        /// filter data to different route tracks
        /// </summary>
        /// <param name="loc"></param>
        /// <param name="curroute"></param>
        void AddToRoute(Location loc, RouteSelect curroute)
        {
            switch (curroute)
            {
                case RouteSelect.Normal:
                    var added = _routesModel.Route.Count < 1 ? new Location(loc, true) : loc;
                    if (_routesModel.Route.IndexOf(added) < 0)
                        _routesModel.Route.Add(added);
                    break;
                case RouteSelect.Warning:
                    _routesModel.WarningRoute.Add(_routesModel.WarningRoute.Count < 1 ? new Location(loc, true) : loc);
                    break;
                case RouteSelect.Error:
                    _routesModel.ErrorRoute.Add(_routesModel.ErrorRoute.Count < 1 ? new Location(loc, true) : loc);
                    break;
            }
        }

        /// <summary>
        /// Get route type by speed parameter
        /// </summary>
        /// <param name="spd"></param>
        /// <returns></returns>
        private RouteSelect GetRouteClass(double spd)
        {
            if (spd >= SpdWarning) return RouteSelect.Error;
            return spd <= SpdNormal ? RouteSelect.Normal : RouteSelect.Warning;
        }

        private void UpdateCenter()
        {
            try
            {
                Iswaiting = false;
                //if (_routesModel.Route.Count > 0)
                //    Route = _routesModel.Route;
                //if (_routesModel.WarningRoute.Count > 0)
                //    WarningRoute = _routesModel.WarningRoute;
                //if (_routesModel.ErrorRoute.Count > 0)
                //    ErrorRoute = _routesModel.ErrorRoute;
                //if (_routesModel.TimeRoute.Count > 0)
                //{
                //    PlayerEndTime = _routesModel.TimeRoute.Max(o => o.Date);
                //    PlayerStartTime = _routesModel.TimeRoute.Min(o => o.Date);
                //    //UpdateCurentPosition(PlayerStartTime, true);
                //}
                if (_routesModel.Parkings.Count > 0)
                    _routesModel.Parkings.ForEach(o => Parkings.Add(o));
                if (_tempRoutes.Any())
                {
                    PlayerEndTime = _tempRoutes.Max(o => o.StartDate);
                    PlayerStartTime = _tempRoutes.Min(o => o.StartDate);
                    _tempRoutes.ForEach(o => Routes.Add(o));
                    UpdateCurentPosition(PlayerStartTime, true);
                }
                //if (Route == null || Route.Count <= 0) return;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        private void UpdateCurentPosition(DateTime time, bool updateCenter = false)
        {
            lock (Routes)
            {
                var pos = Routes.OrderBy(o => o.StartDate).LastOrDefault(f => f.StartDate <= time);
                if (!EnableHistory || Position == null) return;
                if (Routes.Any() && updateCenter)//pos != null && 
                {
                    //MapCenter = MapCenterUser = pos.Point;
                    //var maxlong = _routesModel.TimeRoute.Max(o => o.Point.Longitude);
                    //var maxlat = _routesModel.TimeRoute.Max(o => o.Point.Latitude);
                    //var minlong = _routesModel.TimeRoute.Min(o => o.Point.Longitude);
                    //var minlat = _routesModel.TimeRoute.Min(o => o.Point.Latitude);
                    var maxlong = Routes.Max(o => o.Points.Max(p => p.Longitude));
                    var maxlat = Routes.Max(o => o.Points.Max(p => p.Latitude));
                    var minlong = Routes.Min(o => o.Points.Min(p => p.Longitude));
                    var minlat = Routes.Min(o => o.Points.Min(p => p.Latitude));
                    OnCenterUpdates(new Location(minlat, minlong), new Location(maxlat, maxlong));
                    return;
                }

                _dt = pos != null && _dt < pos.StartDate ? pos.StartDate : time;
                var detail = pos != null ? DayStates.FirstOrDefault(o => o.Date.Equals(pos.StartDate)) : new CarStateModel();
                if (detail != null)
                {
                    Position.Data = new SCarData
                    {
                        Navigation = new SNaviData
                        {
                            Sattelites = detail.St,
                            Speed = detail.Spd,
                            Latitude = detail.Lt,
                            Longitude = detail.Ln
                        },
                        DateUpdate = new DateTimeDataModel(_dt)
                    };
                }
            }
        }




        //private void SelectRoute(CarStateModel point)
        //{
        //    LocationCollection lc = new LocationCollection();
        //    int position = DayStates.IndexOf(point);
        //    int stop = DayStates.Count() - 1;
        //    if (stop - position > 10)
        //    {
        //        stop = position + 10;
        //    }
        //    for (int i = position; i < stop; i++)
        //    {
        //        lc.Add(new Location
        //        {
        //            Latitude = DayStates[i].Lt / 10000.0,
        //            Longitude = DayStates[i].Ln / 10000.0
        //        });
        //    }
        //    //AddRoute();
        //    //Route = lc;
        //    var tempCar = new DISP_Car
        //    {
        //        Car = new SCarModel
        //        {
        //            CarNumber = _selectedDevice
        //        }
        //    };
        //    tempCar.Navigation.LocationPoint = new Location(point.Ln / 10000.0, point.Lt / 10000.0);
        //    //tempCar. = DayStates[position];
        //    //Position = tempCar;
        //}






        //private void ShowMaxSpeed()
        //{
        //    var csm = DayStates.FirstOrDefault(p => p.Spd >= MaxSpeed * 10);
        //    if (csm != null)
        //    {
        //        SelectedState = csm;
        //    }
        //}

        //private void UpdatePosition()
        //{

        //    if (SelectedState == null)
        //        return;
        //    MapCenterUser =
        //    Position.Navigation.LocationPoint = new Location(SelectedState.Lt / 10000.0, SelectedState.Ln / 10000.0);
        //}

        #endregion






        #region DISTANCE_CHECKER

        //private bool _distanceCheckActive = false;

        //private RelayCommand _clickDistanceCommand;

        //public RelayCommand ClickDistanceCommand { get { return _clickDistanceCommand ?? (_clickDistanceCommand = new RelayCommand(a => ClickDistance())); } }

        ///// <summary>
        ///// пользователь выбрал инструмент подсчета пробега
        ///// </summary>
        //private void ClickDistance()
        //{
        //    _distanceCheckActive = !_distanceCheckActive;
        //    if (_distanceCheckActive)
        //    {
        //        CDStartDay = "Укажите начало";
        //        CDStopDay = "Укажите окончание";
        //        VisCDStart = Visibility.Visible;
        //        VisCDStop = Visibility.Collapsed;
        //        _distanceStart = -1;
        //        _distanceStop = -1;
        //    }
        //}

        //private string _cdStartDay = "Укажите начало";
        //private string _cdStopDay = "Укажите окончание";

        //public string CDStartDay
        //{
        //    get { return _cdStartDay; }
        //    set
        //    {
        //        _cdStartDay = value;
        //        this.OnPropertyChanged("CDStartDay");
        //    }
        //}

        //public string CDStopDay
        //{
        //    get { return _cdStopDay; }
        //    set
        //    {
        //        _cdStopDay = value;
        //        this.OnPropertyChanged("CDStopDay");
        //    }
        //}

        //private string _totalDistance = "";
        //public string TotalDistance
        //{
        //    get { return _totalDistance; }
        //    set
        //    {
        //        _totalDistance = value;
        //        this.OnPropertyChanged("TotalDistance");
        //    }
        //}

        //private Visibility _visCDStart = Visibility.Visible;
        //private Visibility _visCDStop = Visibility.Collapsed;

        //public Visibility VisCDStart
        //{
        //    get { return _visCDStart; }
        //    set
        //    {
        //        _visCDStart = value;
        //        this.OnPropertyChanged("VisCDStart");
        //    }
        //}

        //public Visibility VisCDStop
        //{
        //    get { return _visCDStop; }
        //    set
        //    {
        //        _visCDStop = value;
        //        this.OnPropertyChanged("VisCDStop");
        //    }
        //}

        

        //private void DistanceSelectedDayChanged()
        //{
        //    if (SelectedHistoryRow != null)
        //    {
        //        if (VisCDStart == Visibility.Visible)
        //        {
        //            _distanceStart = HistoryRows.IndexOf(SelectedHistoryRow);
        //            VisCDStart = Visibility.Collapsed;
        //            VisCDStop = Visibility.Visible;
        //            CDStartDay = SelectedHistoryRow.Date.ToString("dd.MM.yy");
        //        }
        //        else
        //        {
        //            _distanceStop = HistoryRows.IndexOf(SelectedHistoryRow);
        //            VisCDStart = Visibility.Visible;
        //            VisCDStop = Visibility.Collapsed;
        //            CDStopDay = SelectedHistoryRow.Date.ToString("dd.MM.yy");
        //        }
        //        if (_distanceStart > -1 && _distanceStop > -1)
        //        {
        //            if (_distanceStart < HistoryRows.Count() && _distanceStop < HistoryRows.Count())
        //            {
        //                double dist = 0;
        //                for (int i = _distanceStart; i <= _distanceStop; i++)
        //                {
        //                    dist += HistoryRows[i].Mileage;
        //                }
        //                TotalDistance = Math.Round(dist, 1).ToString();
        //            }
        //        }
        //    }
        //}

        //private int _distanceStart = -1;
        //private int _distanceStop = -1;

        #endregion DISTANCE_CHECKER

        //public enum RouteSelect
        //{
        //    Normal,
        //    Warning,
        //    Error, None
        //};

        public override void OnPropertyChanged(string propertyName)
        {
            if (_dispatcher != null)
                _dispatcher.BeginInvoke(new Action(() =>
                {
                    base.OnPropertyChanged(propertyName);
                }));
        }

        /// <summary>
        /// Выполнение метода action в потоке приложения
        /// </summary>
        /// <param name="action"></param>
        private void DispatherThreadRun(Action action)
        {
            if (_dispatcher != null)
                _dispatcher.BeginInvoke(action);

        }
    }
}
