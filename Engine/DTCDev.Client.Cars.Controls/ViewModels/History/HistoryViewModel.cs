using System;
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
        public HistoryViewModel()
        {
            MapCenter = MapCenterUser = new Location(55.75, 37.62);
            CarSelector.OnCarChanged += CarSelector_OnCarChanged;
            HistoryHandler.Instance.LoadCompleted += Instance_LoadCompleted;
            HistoryHandler.Instance.DayRefreshed += Instance_DayRefreshed;
            HistoryHandler.Instance.LinesLoaded += Instance_LinesLoaded;
            HistoryHandler.Instance.OBDLoaded += Instance_OBDLoaded;
            HistoryHandler.Instance.AccLoaded += Instance_AccLoaded;
            _zoneHandler = ZonesHandler.Instance;
            _mapHandler = CarsHandler.Instance;
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {

                Position.Location =
                MapCenter = MapCenterUser = new Location(55.75, 37.62);
                Points.Add(
                new DISP_Car
                {
                    Name = "Obj-1",
                    Location = new Location(55.758, 37.76)
                });
                Points.Add(
                    new DISP_Car
                    {
                        Name = "Obj-2",
                        Location = new Location(55.75, 37.5)
                    });
                Points.Add(
                    new DISP_Car
                    {
                        Name = "Obj-3",
                        Location = new Location(55.8, 37.625)
                    });
                Position = Points[0];
                Route.Add(new Location(55.75, 37.62, true));
                Route.Add(new Location(55.73, 37.63));
                Route.Add(new Location(55.73, 37.60));
                WarningRoute.Add(new Location(55.76, 37.63, true));
                WarningRoute.Add(new Location(55.75, 37.63));
                ErrorRoute.Add(new Location(55.75, 37.62, true));
                ErrorRoute.Add(new Location(55.76, 37.63));
                Parkings.Add(new ParkingModel(DateTime.Today, new Location(55.76, 37.63)));
            }

            if (!_zoneHandler.Zones.Any())
                _zoneHandler.Update();
        }

        void Instance_AccLoaded(CarAccHistoryModel model)
        {
            AccHistory = model;
            IsEnabledRadio = true;
            if (!IsCheckedSpeed) UpdateRoutes(true);
        }

        void Instance_OBDLoaded(OBDHistoryDataModel model)
        {
            OBDHistory = model;
        }

        void Instance_LinesLoaded(DTCDev.Models.LinesDataModel model)
        {
            Lines = model;
        }

        /// <summary>
        /// Обновление трека
        /// </summary>
        internal void UpdateRoutes(bool recalcValues)
        {
            _routesModel = new RoutesModel();
            if (IsCheckedWay)
            {
                if (recalcValues)
                {
                    _maxValue = AccHistory.Data.Max(o => o.X);
                    _minValue = AccHistory.Data.Min(o => o.X);
                    _leftValue = MinValue + 0.3m * (MaxValue - MinValue);
                    _rightValue = MinValue + 0.6m * (MaxValue - MinValue);
                    OnPropertyChanged("IsCheckedWay");
                }
                UpdateRouteWay();
            }
            if (IsCheckedAccelerate)
            {
                if (recalcValues)
                {
                    _maxValue = AccHistory.Data.Max(o => Math.Max(o.Y, o.Z));
                    _minValue = AccHistory.Data.Min(o => Math.Min(o.Y, o.Z));
                    _leftValue = _minValue + 0.3m*(_maxValue - _minValue);
                    _rightValue = _minValue + 0.6m*(_maxValue - _minValue);
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
            _routesModel = new RoutesModel();
            if (AccHistory == null) return;
            SpdNormal = (int)LeftValue;
            SpdWarning = (int)RightValue;
            var slowTask = new Task(delegate
            {
                if (AccHistory == null) return;
                var firstLoc = DayStates.FirstOrDefault();
                if (firstLoc == null) return;
                var first = AccHistory.Data.FirstOrDefault();
                if (first == null) return;
                var curroute = RouteSelect.None;
                var prev = new Location
                {
                    Latitude = firstLoc.Lt / 10000.0,
                    Longitude = firstLoc.Ln / 10000.0,
                    FirstPoint = true
                };

                foreach (var itemLoc in DayStates)
                {
                    var item = AccHistory.Data.LastOrDefault(o => o.Date.ToDateTime() > firstLoc.Date && o.Date.ToDateTime() <= itemLoc.Date) ?? first;
                    var loc = new Location
                    {
                        Latitude = itemLoc.Lt / 10000.0,
                        Longitude = itemLoc.Ln / 10000.0
                    }; 
                    if (itemLoc.Spd > 0)
                        curroute = SortLocation(prev, loc, curroute, Math.Max(item.Y, item.Z));
                    prev = loc;
                    first = item;
                    firstLoc = itemLoc;
                }
            });
            slowTask.ContinueWith(o =>
            {
                if (Application.Current != null)
                    Application.Current.Dispatcher.BeginInvoke(new Action(UpdateCenter));
            });
            slowTask.Start();
        }

        /// <summary>
        /// вместо скорости для цветовой градации используются показания акселерометра
        /// </summary>
        private void UpdateRouteWay()
        {
            _routesModel = new RoutesModel();
            SpdNormal = (int) LeftValue;
            SpdWarning = (int) RightValue;
            var slowTask = new Task(delegate
            {
                if (AccHistory == null) return;
                var firstLoc = DayStates.FirstOrDefault();
                if (firstLoc == null) return;
                var first = AccHistory.Data.FirstOrDefault();
                if (first == null) return;
                var curroute = RouteSelect.None;
                var prev = new Location
                {
                    Latitude = firstLoc.Lt/10000.0,
                    Longitude = firstLoc.Ln/10000.0,
                    FirstPoint = true
                };

                foreach (var itemLoc in DayStates)
                {
                    var item = AccHistory.Data.LastOrDefault(o => o.Date.ToDateTime() > firstLoc.Date && o.Date.ToDateTime() <= itemLoc.Date) ?? first;
                    var loc = new Location
                    {
                        Latitude = itemLoc.Lt/10000.0,
                        Longitude = itemLoc.Ln/10000.0
                    };
                    if (itemLoc.Spd > 0)
                        curroute = SortLocation(prev, loc, curroute, item.X);
                    prev = loc;
                    first = item;
                    firstLoc = itemLoc;
                }
            });
            slowTask.ContinueWith(o =>
            {
                if (Application.Current != null)
                    Application.Current.Dispatcher.BeginInvoke(new Action(UpdateCenter));
            });
            slowTask.Start();
        }

        /// <summary>
        /// текущая модель отображения трека
        /// </summary>
        private void UpdateRouteSpeed()
        {
            SpdNormal = 900;
            SpdWarning = 1200;
            SortDataByDate(false);
        }

        /// <summary>
        /// Clear all routes for map
        /// </summary>
        private void ClearRoutes()
        {
            _routesModel = new RoutesModel();
            Route.Clear();
            WarningRoute.Clear();
            ErrorRoute.Clear();
            RouteOpacited.Clear();
            Parkings.Clear();
        }


        #region Fields


        private DateTime _startDate;

        private DateTime _stopDate;

        private string _selectedDevice = "";

        private string _loadedText = "";

        private DateTime _displayedHistoryDate;

        private List<CarStateModel> _dayStates = new List<CarStateModel>();

        private DISP_Car _position = new DISP_Car();

        private bool _useAccelleration = true;

        private int _acceleration = 1;



        private CarStateModel _selectedState;

        private int _roundedSpeed = 0;

        private int _maxSpeed = 0;

        private int _distance = 0;

        private CarZoneHistoryErrorViewModel _historyErrorVm = new CarZoneHistoryErrorViewModel();

        private readonly ObservableCollection<VmPolyline> _zoneSelects = new ObservableCollection<VmPolyline>();

        private readonly ZonesHandler _zoneHandler;

        private readonly CarsHandler _mapHandler;

        private Location _currentLocation;

        private int SpdNormal = 900;

        private int SpdWarning = 1200;

        private bool _periodSet = false;

        public Dispatcher Dispather;

        private bool _tablePackageView = false;

        private DISP_Car _selectedMapObject;

        private int _spanMap = 3;

        private bool _enableHistory = false;

        private ObservableCollection<LoadedHistoryRows> _historyRows = new ObservableCollection<LoadedHistoryRows>();

        private LoadedHistoryRows _selectedRow;
        #endregion




        #region Properties

        public string LoadedText
        {
            get { return _loadedText; }
            set
            {
                _loadedText = value;
                this.OnPropertyChanged("LoadedText");
            }
        }

        public bool EnableHistory
        {
            get
            {
                return _enableHistory;
            }
            set
            {
                _enableHistory = value;
                this.OnPropertyChanged("EnableHistory");
            }
        }

        public ObservableCollection<DISP_Car> Points
        {
            get { return _mapHandler.Cars; }
        }

        /// <summary>
        /// Список загруженных дат
        /// </summary>
        public ObservableCollection<LoadedHistoryRows> HistoryRows
        {
            get { return _historyRows; }
        }

        public LoadedHistoryRows SelectedHistoryRow
        {
            get { return _selectedRow; }
            set
            {
                _selectedRow = value;
                this.OnPropertyChanged("SelectedHistoryRow");
                SortData();
                if (_distanceCheckActive)
                    DistanceSelectedDayChanged();
            }
        }


        public DISP_Car SelectedMapObject
        {
            get
            {
                return _selectedMapObject;
            }
            set
            {
                if (_selectedMapObject != value)
                {
                    if (_selectedMapObject != null)
                    {
                        _selectedMapObject.PropertyChanged -= selectedMapObject_PropertyChanged;
                        _selectedMapObject.InZone = true;
                    }

                    _selectedMapObject = value;
                    if (value != null)
                    {
                        MapCenter = MapCenterUser = this._selectedMapObject.Location;
                        _selectedMapObject.PropertyChanged += selectedMapObject_PropertyChanged;
                        //if (SelectedZone != null)
                        //    GetMoreInfo(this._selectedMapObject);
                    }
                    OnPropertyChanged("SelectedMapObject");
                }
                CarSelector.SelectedCar = value;
            }
        }

        void selectedMapObject_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!e.PropertyName.Equals("Location")) return;
            var obj = sender as DISP_Car;
            if (obj != null)
            {
                MapCenter = MapCenterUser = obj.Location;
                //if (SelectedZone != null)
                //    GetMoreInfo(this.selectedMapObject);
            }
        }

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

        private LocationCollection _route = new LocationCollection();
        public LocationCollection Route
        {
            get { return _route; }
            set
            {
                _route = value;
                this.OnPropertyChanged("Route");
            }
        }

        private LinesDataModel _lines = new LinesDataModel();

        public LinesDataModel Lines
        {
            get { return _lines; }
            set
            {
                _lines = value;
                this.OnPropertyChanged("Lines");
            }
        }

        private OBDHistoryDataModel _obdHistory = new OBDHistoryDataModel();

        public OBDHistoryDataModel OBDHistory
        {
            get { return _obdHistory; }
            set
            {
                _obdHistory = value;
                this.OnPropertyChanged("OBDHistory");
            }
        }

        private CarAccHistoryModel _accHistory = new CarAccHistoryModel();
        public CarAccHistoryModel AccHistory
        {
            get { return _accHistory; }
            set
            {
                _accHistory = value;
                this.OnPropertyChanged("AccHistory");
            }
        }

        private LocationCollection _warningRoute = new LocationCollection();
        public LocationCollection WarningRoute
        {
            get { return _warningRoute; }
            set
            {
                _warningRoute = value;
                this.OnPropertyChanged("WarningRoute");
            }
        }

        private LocationCollection _errorRoute = new LocationCollection();
        public LocationCollection ErrorRoute
        {
            get { return _errorRoute; }
            set
            {
                _errorRoute = value;
                this.OnPropertyChanged("ErrorRoute");
            }
        }

        public LocationCollection _offlineRoute = new LocationCollection();
        public LocationCollection OfflineRoute
        {
            get { return _offlineRoute; }
            set
            {
                _offlineRoute = value;
                this.OnPropertyChanged("OfflineRoute");
            }
        }

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


        public void SetDates(DateTime first, DateTime last)
        {
            if (first <= last)
            {
                StartDate = first;
                StopDate = last;
            }
            else
            {
                StartDate = last;
                StopDate = first;
            }
            SelectedDatesRangeChanged();
            LoadData();
        }

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
                if (TablePackageView)
                    OnPropertyChanged("PackagesList");
            }
        }

        public List<PackageViewModel> PackagesList
        {
            get
            {
                List<PackageViewModel> answer = new List<PackageViewModel>();

                return DayStates.Select(o => new PackageViewModel(o)).ToList();
            }
        }

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

            get { return _position; }
            set
            {
                if (_position != null)
                {
                    _position.IsSelected = false;
                    _position.PropertyChanged -= PositionOnPropertyChanged;
                }
                _position = value;
                _position.IsSelected = true;
                this.OnPropertyChanged("Position");
                if (value == null) return;
                if (value.Location != null)
                    MapCenter = MapCenterUser = value.Location;
                _position.PropertyChanged += PositionOnPropertyChanged;
            }
        }

        private void PositionOnPropertyChanged(object sender, PropertyChangedEventArgs arg)
        {
            if (arg.PropertyName.Equals("Location") && !EnableHistory)
                MapCenter = MapCenterUser = Position.Location;
        }

        public bool UseAccelleration
        {
            get { return _useAccelleration; }
            set
            {
                _useAccelleration = value;
                this.OnPropertyChanged("UseAccelleration");
            }
        }

        public bool TablePackageView
        {
            get { return _tablePackageView; }
            set
            {
                _tablePackageView = value;
                OnPropertyChanged("TablePackageView");
                OnPropertyChanged("PackagesList");
            }
        }
        public int Accelleration
        {
            get { return _acceleration; }
            set
            {
                _acceleration = value;
                this.OnPropertyChanged("Accelleration");
            }
        }

        public CarStateModel SelectedState
        {
            get { return _selectedState; }
            set
            {
                _selectedState = value;
                this.OnPropertyChanged("SelectedState");
                Update();
            }
        }

        void Update()
        {
            if (SelectedState != null)
            {
                Position.Data = new SCarData() { Navigation = new SNaviData() { Speed = _selectedState.Spd } };
                Position.Update();
            }
        }

        public int RoundedSpeed
        {
            get { return _roundedSpeed; }
            set
            {
                _roundedSpeed = value;
                this.OnPropertyChanged("RoundedSpeed");
            }
        }

        public int MaxSpeed
        {
            get { return _maxSpeed; }
            set
            {
                _maxSpeed = value;
                this.OnPropertyChanged("MaxSpeed");
            }
        }

        public int Distance
        {
            get { return _distance; }
            set
            {
                _distance = value;
                this.OnPropertyChanged("Distance");
            }
        }

        private double _distanceAll = 0;
        public double DistanceAll
        {
            get { return Math.Round(_distanceAll, 2); }
            set
            {
                _distanceAll = value;
                this.OnPropertyChanged("DistanceAll");
            }
        }

        public Location CurrentLocation
        {
            get { return _currentLocation; }
            set
            {
                _currentLocation = value;
                this.OnPropertyChanged("CurrentLocation");
            }
        }

        public bool PeriodSet
        {
            get { return _periodSet; }
            set
            {
                _periodSet = value;
                OnPropertyChanged("PeriodSet");
                SortData();
            }
        }

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

        public int SpanMap
        {
            get { return _spanMap; }
            set
            {
                _spanMap = value;
                this.OnPropertyChanged("SpanMap");
            }
        }

        #endregion




        #region Events


        #endregion




        #region Commands

        private RelayCommand _loadNext10Command;

        public RelayCommand LoadNext10Command { get { return _loadNext10Command ?? (_loadNext10Command = new RelayCommand(a => LoadNext10Days())); } }




        private RelayCommand _showMaxSpeedCommand;

        public RelayCommand ShowMaxSpeedCommad
        {
            get
            {
                if (_showMaxSpeedCommand == null)
                    _showMaxSpeedCommand = new RelayCommand(a => this.ShowMaxSpeed());
                return _showMaxSpeedCommand;
            }
        }



        #endregion








        #region Private Functions

        /// <summary>
        /// Метод запроса истории еще за 10 дней
        /// </summary>
        private void LoadNext10Days()
        {
            if (!EnableHistory) return;

            if (CarSelector.SelectedCar != null && CarSelector.SelectedCar.Car.Id != String.Empty)
            {
                int days = 10;
                for (int i = 0; i < days; i++)
                {
                    GetCache(_lastLoadedDate - TimeSpan.FromDays(i + 1));
                }
                HistoryHandler.Instance.StartLoadHistory(CarSelector.SelectedCar.Car.Id, _lastLoadedDate - TimeSpan.FromDays(days + 1), _lastLoadedDate - TimeSpan.FromDays(1), UseAccelleration);
            }
        }

        /// <summary>
        /// Update visual states when selected date is changed
        /// </summary>
        private void SelectedDatesRangeChanged()
        {
            string displayedDate = "";
            if (_startDate != null)
                displayedDate = _startDate.ToString("dd.MM.yyyy");
            displayedDate += " - ";
            if (_stopDate != null)
                displayedDate += _stopDate.ToString("dd.MM.yyyy");
        }





        void CarSelector_OnCarChanged(Engine.DisplayModels.DISP_Car car)
        {
            ZoneSelect.Clear();
            if (car != null)
            {
                Position = EnableHistory ? new DISP_Car() : CarSelector.SelectedCar;
                Position.Car = new SCarModel()
                {
                    CarNumber = car.Car.CarNumber,
                    Id = car.Car.Id
                };
                if (car.Car != null)
                    SelectedDevice = car.Car.CarNumber;
                //TODO Раскоментировать и заменить после привязки машин к зонам 
                //var zone = _zoneHandler.Zones.FirstOrDefault(o => o.ID == car.ZoneId);
                var zone = _zoneHandler.Zones.FirstOrDefault(o => o.ID == 1);//car.ZoneId);
                if (zone == null) return;
                ZoneSelect.Add(zone);
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
                SelectedHistoryRow = null;
                HistoryRows.Clear();
                DistanceAll = 0;
                DayStates.Clear();
                Route.Clear();
                WarningRoute.Clear();
                OfflineRoute.Clear();
                ErrorRoute.Clear();
                Parkings.Clear();
                int days = 30;
                for (int i = 0; i < 30; i++)
                {
                    GetCache(DateTime.Now - TimeSpan.FromDays(i));
                }
                IsEnabledRadio = false;
                //Загружаем историю за последние 30 дней
                HistoryHandler.Instance.StartLoadHistory(CarSelector.SelectedCar.Car.Id, DateTime.Now - TimeSpan.FromDays(days), DateTime.Now, UseAccelleration);
            }
        }



        //loading data of history completed
        void Instance_LoadCompleted(object sender, EventArgs e)
        {
            LoadedText = "Обновление завершено";
        }

        /// <summary>
        /// Дата, на каоторой остановилась загрузка
        /// </summary>
        private DateTime _lastLoadedDate = DateTime.Now;

        //refreshed one day history
        void Instance_DayRefreshed(DateTime day, List<CarStateModel> data)
        {
            BuildHistoryRow(data, day);

            CacheRoute(string.Format("[{0}]-{1}-{2}-{3}", CarSelector.SelectedCar.ID, day.Day, day.Month, day.Year), data);
            _lastLoadedDate = day;
            LoadedText = "Обновляю " + day.ToString("dd.MM.yy");
        }


        private void SortData()
        {
            if (SelectedHistoryRow == null)
                return;
            else
            {
                DayStates = SelectedHistoryRow.Data;
                SortDataByDate(true);
            }
            AccHistory = null;
            HistoryHandler.Instance.StartLoadDayLines(Position.Car.Id, SelectedHistoryRow.Date);
            HistoryHandler.Instance.StartLoadOBD(Position.Car.Id, SelectedHistoryRow.Date);
            HistoryHandler.Instance.StartLoadAcc(Position.Car.Id, SelectedHistoryRow.Date);
            
        }

        private void SortDataByDate(bool savecache)
        {
            ClearRoutes();

            var first = DayStates.FirstOrDefault();
            if (first != null)
            {
                var curroute = RouteSelect.None;
                var prev = new Location
                {
                    Latitude = first.Lt / 10000.0,
                    Longitude = first.Ln / 10000.0,
                    FirstPoint = true
                };
                MapCenter = MapCenterUser = prev;
                if (Position != null)
                    Position.Location = prev;

                double dist = 0;
                var dc = new DistanceCalculator();
                var slowTask = new Task(delegate
                {
                    var foundParking = true;
                    var nullDate = first.Spd < 6 || StateDateTimeHelper.EqualInterval(_displayedHistoryDate, first)
                        ? first : null;
                    int indx;
                    foreach (var item in DayStates)
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
                                if (_routesModel.Parkings.Count > 0)
                                    _routesModel.Parkings[_routesModel.Parkings.Count - 1].SetEndDates(itemtime);
                            }
                            var curdist = dc.Calculate(first, item);
                            var spd = 1000 * curdist / spantime.TotalSeconds;
                            if (item.Spd > 0 && Math.Abs(curdist) > .001 && !(foundParking && isParking))
                            {
                                if (spd > 35)
                                {
                                    var trrr = 0;
                                }
                                if(IsCheckedSpeed)
                                    curroute = SortLocation(prev, loc, curroute, item.Spd);
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
                });
                slowTask.ContinueWith(o =>
                {
                    ContinueSortData((int)dist);
                    if (!savecache) return;
                    var strt = DayStates.First();
                });
                slowTask.Start();
            }
            else
            {
                MaxSpeed = 0;
                RoundedSpeed = 0;
                Distance = 0;
            }
        }

        private void ContinueSortData(int dist)
        {
            UpdateCenter();
            UpdateErrorControl();
            MaxSpeed = DayStates.Max(p => p.Spd) / 10;
            RoundedSpeed = DayStates.Sum(p => p.Spd) / (10 * DayStates.Count());

            Distance = dist;

            var strt = DayStates.First();
            var stp = DayStates.Last();
            var dtStrt = new DateTime(1, 1, 1, strt.hh, strt.mm, 0);
            var stStp = new DateTime(1, 1, 1, stp.hh, stp.mm, 0);
        }

        private void CacheRoute(string name, List<CarStateModel> data)
        {
            var myDocs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\M2B\\Cache\\";
            try
            {
                if (System.IO.Directory.Exists(myDocs) == false)
                    System.IO.Directory.CreateDirectory(myDocs);
                if (System.IO.File.Exists(myDocs + name))
                    System.IO.File.Delete(myDocs + name);

                using (var writer = new StreamWriter(myDocs + name))
                {
                    string row = JsonConvert.SerializeObject(data);
                    writer.WriteLine(row);
                }
            }
            catch (Exception)
            {

            }
        }

        private void GetCache(DateTime date)
        {
            try
            {
                var myDocs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\M2B\\Cache\\";
                var name = string.Format("[{0}]-{1}-{2}-{3}", CarSelector.SelectedCar.ID, date.Day, date.Month, date.Year);
                if (!System.IO.File.Exists(myDocs + name)) return;
                using (var reader = new System.IO.StreamReader(myDocs + name))
                {
                    var xs = new System.Xml.Serialization.XmlSerializer(typeof(List<CarStateModel>));
                    var cr = JsonConvert.DeserializeObject<List<CarStateModel>>(reader.ReadToEnd());
                    if (cr == null) return;
                    BuildHistoryRow(cr, date);
                    SortDataByDate(false);

                }
            }
            catch { }
        }

        private void BuildHistoryRow(List<CarStateModel> data, DateTime date)
        {
            LoadedHistoryRows r = new LoadedHistoryRows();
            r.Date = date;
            r.Data = data;
            try
            {
                DistanceCalculator dc = new DistanceCalculator();
                for (int i = 0; i < data.Count() - 1; i++)
                {
                    r.Mileage += dc.Calculate(data[i], data[i + 1]);
                }
            }
            catch { }
            if (data.Count() > 0)
            {
                try
                {
                    r.MiddleSpeed = data.Sum(p => p.Spd) / 10 / data.Count();
                    List<CarStateModel> moving = data.Where(p => p.Spd > 10).ToList();
                    int hStart = moving.Min(p => p.hh);
                    int minStart = moving.Where(p => p.hh == hStart).Min(p => p.mm);
                    int hStop = moving.Max(p => p.hh);
                    int mStop = moving.Where(p => p.hh == hStop).Max(p => p.mm);

                    r.Start = hStart.ToString() + ":" + minStart.ToString();
                    r.Stop = hStop.ToString() + ":" + mStop.ToString();


                }
                catch { }
            }

            bool replaced = false;
            for (int i = 0; i < HistoryRows.Count(); i++)
            {
                if (HistoryRows[i].StringDate == r.StringDate)
                {
                    DistanceAll -= HistoryRows[i].Mileage;
                    HistoryRows.RemoveAt(i);
                    HistoryRows.Insert(i, r);
                    replaced = true;
                    DistanceAll += r.Mileage;
                }
            }
            if (replaced == false)
            {
                bool inserted = false;
                for (int i = 0; i < HistoryRows.Count(); i++)
                {
                    if (HistoryRows[i].Date < r.Date)
                    {
                        HistoryRows.Insert(i, r);
                        inserted = true;
                        break;
                    }
                }
                if (inserted == false)
                    HistoryRows.Add(r);
                DistanceAll += r.Mileage;
            }
        }

        private RouteSelect SortLocation(Location prev, Location loc, RouteSelect prevroute, int spd)
        {
            var curroute = GetRouteClass(spd);
            if (prevroute != curroute)
            {
                loc.FirstPoint = false;
                AddToRoute(loc, prevroute);
                //if(spd > 0)
                AddToRoute(new Location(prev, true), curroute);
            }
            //if(spd > 0)
            AddToRoute(loc, curroute);
            return curroute;
        }

        private RoutesModel _routesModel = new RoutesModel();

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
        private RouteSelect GetRouteClass(int spd)
        {
            if (spd >= SpdWarning) return RouteSelect.Error;
            return spd <= SpdNormal ? RouteSelect.Normal : RouteSelect.Warning;
        }

        private void UpdateCenter()
        {
            try
            {
                if (_routesModel.Route.Count > 0)
                    Route = _routesModel.Route;
                if (_routesModel.WarningRoute.Count > 0)
                    WarningRoute = _routesModel.WarningRoute;
                if (_routesModel.ErrorRoute.Count > 0)
                    ErrorRoute = _routesModel.ErrorRoute;
                if (_routesModel.Parkings.Count > 0)
                    Parkings = _routesModel.Parkings;

                if (Route == null || Route.Count <= 0) return;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }






        private void SelectRoute(CarStateModel point)
        {
            LocationCollection lc = new LocationCollection();
            int position = DayStates.IndexOf(point);
            int stop = DayStates.Count() - 1;
            if (stop - position > 10)
            {
                stop = position + 10;
            }
            for (int i = position; i < stop; i++)
            {
                lc.Add(new Location
                {
                    Latitude = DayStates[i].Lt / 10000.0,
                    Longitude = DayStates[i].Ln / 10000.0
                });
            }
            //AddRoute();
            //Route = lc;
            DISP_Car tempCar = new DISP_Car();
            tempCar.Car = new SCarModel
            {
                CarNumber = _selectedDevice
            };
            tempCar.Location = new Location(point.Ln / 10000.0, point.Lt / 10000.0);
            //tempCar. = DayStates[position];
            //Position = tempCar;
        }






        private void ShowMaxSpeed()
        {
            var csm = DayStates.FirstOrDefault(p => p.Spd >= MaxSpeed * 10);
            if (csm != null)
            {
                SelectedState = csm;
            }
        }

        private void UpdatePosition()
        {

            if (SelectedState == null)
                return;
            MapCenterUser =
            Position.Location = new Location(SelectedState.Lt / 10000.0, SelectedState.Ln / 10000.0);
        }

        #endregion






        #region DISTANCE_CHECKER

        private bool _distanceCheckActive = false;

        private RelayCommand _clickDistanceCommand;

        public RelayCommand ClickDistanceCommand { get { return _clickDistanceCommand ?? (_clickDistanceCommand = new RelayCommand(a => ClickDistance())); } }

        /// <summary>
        /// пользователь выбрал инструмент подсчета пробега
        /// </summary>
        private void ClickDistance()
        {
            _distanceCheckActive = !_distanceCheckActive;
            if (_distanceCheckActive)
            {
                CDStartDay = "Укажите начало";
                CDStopDay = "Укажите окончание";
                VisCDStart = Visibility.Visible;
                VisCDStop = Visibility.Collapsed;
                _distanceStart = -1;
                _distanceStop = -1;
            }
        }

        private string _cdStartDay = "Укажите начало";
        private string _cdStopDay = "Укажите окончание";

        public string CDStartDay
        {
            get { return _cdStartDay; }
            set
            {
                _cdStartDay = value;
                this.OnPropertyChanged("CDStartDay");
            }
        }

        public string CDStopDay
        {
            get { return _cdStopDay; }
            set
            {
                _cdStopDay = value;
                this.OnPropertyChanged("CDStopDay");
            }
        }

        private string _totalDistance = "";
        public string TotalDistance
        {
            get { return _totalDistance; }
            set
            {
                _totalDistance = value;
                this.OnPropertyChanged("TotalDistance");
            }
        }

        private Visibility _visCDStart = Visibility.Visible;
        private Visibility _visCDStop = Visibility.Collapsed;

        public Visibility VisCDStart
        {
            get { return _visCDStart; }
            set
            {
                _visCDStart = value;
                this.OnPropertyChanged("VisCDStart");
            }
        }

        public Visibility VisCDStop
        {
            get { return _visCDStop; }
            set
            {
                _visCDStop = value;
                this.OnPropertyChanged("VisCDStop");
            }
        }

        private decimal _leftValue = 10;
        public decimal LeftValue
        {
            get { return _leftValue; }
            set
            {
                _leftValue = value;
                OnPropertyChanged("LeftValue");
            }
        }

        private decimal _rightValue = 80;
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

        private decimal _maxValue = 100;
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
                if (_isCheckedSpeed == value) return;
                _isCheckedSpeed = value;
                OnPropertyChanged("IsCheckedSpeed");
                if (!value) return;
                UpdateRouteSpeed();
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

                if (!value) return;
                _maxValue = AccHistory.Data.Max(o => o.X);
                _minValue = AccHistory.Data.Min(o => o.X);
                _leftValue = MinValue + 0.3m * (MaxValue - MinValue);
                _rightValue = MinValue + 0.6m * (MaxValue - MinValue);
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

                if (!value) return;
                _maxValue = AccHistory.Data.Max(o => Math.Max(o.Y, o.Z));
                _minValue = AccHistory.Data.Min(o => Math.Min(o.Y, o.Z));
                _leftValue = _minValue + 0.3m * (_maxValue - _minValue);
                _rightValue = _minValue + 0.6m * (_maxValue - _minValue);
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
                _isEnabledRadio = value;
                OnPropertyChanged("IsEnabledRadio");
            }
        }

        private void DistanceSelectedDayChanged()
        {
            if (SelectedHistoryRow != null)
            {
                if (VisCDStart == Visibility.Visible)
                {
                    _distanceStart = HistoryRows.IndexOf(SelectedHistoryRow);
                    VisCDStart = Visibility.Collapsed;
                    VisCDStop = Visibility.Visible;
                    CDStartDay = SelectedHistoryRow.Date.ToString("dd.MM.yy");
                }
                else
                {
                    _distanceStop = HistoryRows.IndexOf(SelectedHistoryRow);
                    VisCDStart = Visibility.Visible;
                    VisCDStop = Visibility.Collapsed;
                    CDStopDay = SelectedHistoryRow.Date.ToString("dd.MM.yy");
                }
                if (_distanceStart > -1 && _distanceStop > -1)
                {
                    if (_distanceStart < HistoryRows.Count() && _distanceStop < HistoryRows.Count())
                    {
                        double dist = 0;
                        for (int i = _distanceStart; i <= _distanceStop; i++)
                        {
                            dist += HistoryRows[i].Mileage;
                        }
                        TotalDistance = Math.Round(dist, 1).ToString();
                    }
                }
            }
        }

        private int _distanceStart = -1;
        private int _distanceStop = -1;
        #endregion DISTANCE_CHECKER

        private enum RouteSelect
        {
            Normal,
            Warning,
            Error, None
        };


        public class LoadedHistoryRows
        {
            public DateTime Date { get; set; }

            public string StringDate { get { return Date.ToString("dd.MM.yyyy"); } }

            private double _mileage;

            public double Mileage
            {
                get { return Math.Round(_mileage, 2); }
                set
                {
                    _mileage = value;
                }
            }

            public int MiddleSpeed { get; set; }

            public List<CarStateModel> Data { get; set; }

            public string Start { get; set; }

            public string Stop { get; set; }
        }


    }
}
