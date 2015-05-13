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
            bwPlayer.DoWork += bwPlayer_DoWork;
            bwPlayer.RunWorkerCompleted += bwPlayer_RunWorkerCompleted;
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
            dtm.Interval = new TimeSpan(0, 0, 0, 0, 100);
            dtm.Tick += dtm_Tick;
        }

        void Instance_AccLoaded(CarAccHistoryModel model)
        {
            AccHistory = model;
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

        private int _currentSecondSelected = 0;

        private DISP_Car _position = new DISP_Car();

        private bool _useAccelleration = true;

        private int _acceleration = 1;

        private Visibility _btnStopPlayVisibility = Visibility.Collapsed;

        private CarStateModel _selectedState;

        private int _roundedSpeed = 0;

        private int _maxSpeed = 0;

        private int _distance = 0;

        private CarZoneHistoryErrorViewModel _historyErrorVm = new CarZoneHistoryErrorViewModel();

        private readonly ObservableCollection<VmPolyline> _zoneSelects = new ObservableCollection<VmPolyline>();

        private readonly ZonesHandler _zoneHandler;

        private readonly CarsHandler _mapHandler;

        private Location _currentLocation;

        private int SpdNormal = 90;

        private int SpdWarning = 120;

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

        public int CurrentSecondSelected
        {
            get { return _currentSecondSelected; }
            set
            {
                _currentSecondSelected = value;
                this.OnPropertyChanged("CurrentSecondSelected");
                if (_isPlayerPlay == false)
                    CheckSelectedTime();
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
                if (_position != null) _position.IsSelected = false;
                _position = value;
                _position.IsSelected = true;
                this.OnPropertyChanged("Position");
                if (value != null && value.Location != null)
                    MapCenter = MapCenterUser = value.Location;
                else
                    MapCenter = new Location();
            }
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

        public Visibility BtnStopPlayVisibility
        {
            get { return _btnStopPlayVisibility; }
            set
            {
                _btnStopPlayVisibility = value;
                this.OnPropertyChanged("BtnStopPlayVisibility");
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

        public RelayCommand LoadNext10Command { get { return _loadNext10Command ?? (_loadNext10Command = new RelayCommand(a => LoadNext10Days())); }}

        private RelayCommand _moveTimeBackwardUpCommand;
        public RelayCommand MoveTimeBackwardUpCommand
        {
            get
            {
                return _moveTimeBackwardUpCommand ??
                       (_moveTimeBackwardUpCommand = new RelayCommand(a => MoveTimeBackUp()));
            }
        }

        private RelayCommand _moveTimeForwardUpCommand;
        public RelayCommand MoveTimeForwardUpCommand
        {
            get
            {
                return _moveTimeForwardUpCommand ??
                       (_moveTimeForwardUpCommand = new RelayCommand(a => MoveTimeForwardUp()));
            }
        }


        private RelayCommand _moveTimeBackwardCommand;
        public RelayCommand MoveTimeBackwardCommand
        {
            get
            {
                if (_moveTimeBackwardCommand == null)
                    _moveTimeBackwardCommand = new RelayCommand(a => MoveTimeBack());
                return _moveTimeBackwardCommand;
            }
        }

        private RelayCommand _moveTimeForwardCommand;
        public RelayCommand MoveTimeForwardCommand
        {
            get
            {
                if (_moveTimeForwardCommand == null)
                    _moveTimeForwardCommand = new RelayCommand(a => MoveTimeForward());
                return _moveTimeForwardCommand;
            }
        }


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


        #region PLAYER

        private RelayCommand _plr_RunCommand;
        private RelayCommand _plr_StopCommand;
        private RelayCommand _plr_PlusCommand;
        private RelayCommand _plr_MinezeCommand;
        private RelayCommand _plr_FastPlusCommand;
        private RelayCommand _plr_FastMinezeCommand;


        public RelayCommand Plr_RunCommand
        {
            get
            {
                if (_plr_RunCommand == null)
                    _plr_RunCommand = new RelayCommand(a => PLR_Start());
                return _plr_RunCommand;
            }
        }
        public RelayCommand Plr_StopCommand
        {
            get
            {
                if (_plr_StopCommand == null)
                    _plr_StopCommand = new RelayCommand(a => PLR_Stop());
                return _plr_StopCommand;
            }
        }
        public RelayCommand Plr_PlusCommand
        {
            get
            {
                if (_plr_PlusCommand == null)
                    _plr_PlusCommand = new RelayCommand(a => PLR_Plus());
                return _plr_PlusCommand;
            }
        }
        public RelayCommand Plr_MinezeCommand
        {
            get
            {
                if (_plr_MinezeCommand == null)
                    _plr_MinezeCommand = new RelayCommand(a => PLR_Mineze());
                return _plr_MinezeCommand;
            }
        }
        public RelayCommand Plr_FastPlusCommand
        {
            get
            {
                if (_plr_FastPlusCommand == null)
                    _plr_FastPlusCommand = new RelayCommand(a => PLR_FastPlus());
                return _plr_FastPlusCommand;
            }
        }
        public RelayCommand Plr_FastMinezeCommand
        {
            get
            {
                if (_plr_FastMinezeCommand == null)
                    _plr_FastMinezeCommand = new RelayCommand(a => PLR_FastMineze());
                return _plr_FastMinezeCommand;
            }
        }



        #endregion PLAYER

        #endregion




        #region Public Functions

        public void StopPlay()
        {
            _isPlayerPlay = false;
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
                    GetCache(_lastLoadedDate - TimeSpan.FromDays(i+1));
                }
                HistoryHandler.Instance.StartLoadHistory(CarSelector.SelectedCar.Car.Id, _lastLoadedDate - TimeSpan.FromDays(days+1), _lastLoadedDate - TimeSpan.FromDays(1), UseAccelleration);
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
                //Загружаем историю за последние 30 дней
                HistoryHandler.Instance.StartLoadHistory(CarSelector.SelectedCar.Car.Id, DateTime.Now-TimeSpan.FromDays(days), DateTime.Now, UseAccelleration);
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

            HistoryHandler.Instance.StartLoadDayLines(Position.Car.Id, _displayedHistoryDate);
            HistoryHandler.Instance.StartLoadOBD(Position.Car.Id, _displayedHistoryDate);
            HistoryHandler.Instance.StartLoadAcc(Position.Car.Id, _displayedHistoryDate);
            AccHistory = null;
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
                        var spantime = itemtime-StateDateTimeHelper.GetTime(first);
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
                            var spd =1000 * curdist/spantime.TotalSeconds;
                            if (item.Spd > 0 && Math.Abs(curdist) > .001 && !(foundParking && isParking))
                            {
                                if (spd > 35)
                                {
                                    var trrr = 0;
                                }
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
            if (System.IO.Directory.Exists(myDocs) == false)
                System.IO.Directory.CreateDirectory(myDocs);
            if (System.IO.File.Exists(myDocs + name))
                System.IO.File.Delete(myDocs + name);
            try
            {
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

            bool replaced = false;
            for (int i = 0; i < HistoryRows.Count(); i++)
            {
                if(HistoryRows[i].StringDate==r.StringDate)
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
            if (spd >= SpdWarning * 10) return RouteSelect.Error;
            return spd <= SpdNormal * 10 ? RouteSelect.Normal : RouteSelect.Warning;
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




        private void CheckSelectedTime()
        {
            if (DayStates == null || !DayStates.Any())
                return;
            TimeSpan ts = TimeSpan.FromSeconds(_currentSecondSelected);
            DateTime dt = _displayedHistoryDate + ts;
            if (DayStates.FirstOrDefault(p => p.hh == dt.Hour && p.mm == dt.Minute && p.ss == dt.Second) != null)
                return;
            else
            {
                FindModelByDate(dt);
                UpdatePosition();
            }
        }

        private void FindModelByDate(DateTime dt)
        {
            if (DayStates == null)
                return;
            if (DayStates.Count < 1)
                return;
            CarStateModel temp = DayStates.LastOrDefault(p => p.hh == dt.Hour && p.mm <= dt.Minute);
            if (temp == null)
            {
                temp = DayStates.LastOrDefault(p => p.hh < dt.Hour);
                if (temp == null)
                {
                    temp = DayStates.First();
                }
            }
            SelectedState = temp;
            id = DayStates.IndexOf(SelectedState);
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

        int id = 0;
        DispatcherTimer dtm = new DispatcherTimer();
        private bool forward = true;

        private void MoveBack()
        {
            //Debug.WriteLine(string.Format("Перемотка {0}: {1}раз", forward ? "вперед" : "назад", id));
            if (DayStates == null || !DayStates.Any())
                return;
            if (_selectedState == null)
            {
                SelectedState = DayStates.First();
                id = 0;
            }
            else
            {
                id--;
                if (id > 0)
                {
                    SelectedState = DayStates[id];
                }
                else
                    id = 0;
            }
            UpdatePosition();
        }

        private void MoveForward()
        {
            //Debug.WriteLine(string.Format("Перемотка {0}: {1}раз", forward ? "вперед" : "назад", id));
            if (DayStates == null || !DayStates.Any())
                return;
            if (SelectedState == null)
            {
                SelectedState = DayStates.First();
                id = 0;
            }
            else
            {
                id++;
                if (id <= DayStates.Count() - 1)
                {
                    SelectedState = DayStates[id];
                }
                else
                    id = DayStates.Count() - 1;
            }
            UpdatePosition();
        }
        /// <summary>
        /// Step to manualy move time back in view mode
        /// </summary>
        private void MoveTimeBack()
        {
            forward = false;
            dtm.Start();
        }

        /// <summary>
        /// Step to manualy move time forward in view mode
        /// </summary>
        private void MoveTimeForward()
        {
            forward = true;
            dtm.Start();
        }


        private void MoveTimeBackUp()
        {
            forward = false;
            dtm.Stop();
            MoveBack();
        }

        private void MoveTimeForwardUp()
        {
            forward = true;
            dtm.Stop();
            MoveForward();
        }


        void dtm_Tick(object sender, EventArgs e)
        {
            if (forward)
                MoveForward();
            else
                MoveBack();
        }

        #region PLAYER

        private bool _isPlayerPlay = false;

        private BackgroundWorker bwPlayer = new BackgroundWorker();

        private void PLR_Start()
        {
            _isPlayerPlay = true;
            _currentSecond = CurrentSecondSelected;
            bwPlayer.RunWorkerAsync();
            BtnStopPlayVisibility = Visibility.Visible;
        }

        private void PLR_Stop()
        {
            _isPlayerPlay = false;
            BtnStopPlayVisibility = Visibility.Collapsed;
        }

        private void PLR_Plus()
        {
            id++;
            if (id > DayStates.Count() - 1)
                id = DayStates.Count() - 1;
            _selectedState = DayStates[id];
            _currentSecond = CurrentSecondSelected;
        }

        private void PLR_Mineze()
        {
            id--;
            if (id < 0)
                id = 0;
            _selectedState = DayStates[id];
            _currentSecond = CurrentSecondSelected;
        }

        private void PLR_FastPlus()
        {
            id += 10;
            if (id > DayStates.Count() - 1)
                id = DayStates.Count() - 1;
            _selectedState = DayStates[id];
            _currentSecond = CurrentSecondSelected;
        }

        private void PLR_FastMineze()
        {
            id -= 10;
            if (id < 0)
                id = 0;
            _selectedState = DayStates[id];
            _currentSecond = CurrentSecondSelected;

        }

        private int _currentSecond = 0;



        void bwPlayer_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _currentSecond++;
            if (_currentSecond < 0)
                _currentSecond = 0;
            if (_currentSecond > 84600)
                _currentSecond = 84600;
            CurrentSecondSelected = _currentSecond;

            FindPoint();

            if (_isPlayerPlay)
                bwPlayer.RunWorkerAsync();
        }

        void bwPlayer_DoWork(object sender, DoWorkEventArgs e)
        {
            System.Threading.Thread.Sleep(1000 / _acceleration);
        }

        private void FindPoint()
        {
            TimeSpan ts = TimeSpan.FromSeconds(_currentSecond);
            DateTime dt = _displayedHistoryDate + ts;
            FindModelByDate(dt);
            SelectRoute(_selectedState);
        }

        #endregion PLAYER



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

        private enum RouteSelect
        {
            Normal,
            Warning,
            Error, None
        };


        public class LoadedHistoryRows
        {
            public DateTime Date{get;set;}

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

            public int MiddleSpeed{get;set;}

            public List<CarStateModel> Data { get; set; }


        }
    }
}
