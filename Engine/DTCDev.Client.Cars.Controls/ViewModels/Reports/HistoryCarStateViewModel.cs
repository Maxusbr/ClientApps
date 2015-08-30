using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using DTCDev.Client.Cars.Controls.Helpers;
using DTCDev.Client.Cars.Controls.Models;
using DTCDev.Client.Cars.Controls.ViewModels.History;
using DTCDev.Client.Cars.Engine.AppLogic;
using DTCDev.Client.Cars.Engine.Handlers.Cars;
using DTCDev.Client.Controls.Map;
using DTCDev.Client.ViewModel;
using DTCDev.Models.CarsSending.Car;

namespace DTCDev.Client.Cars.Controls.ViewModels.Reports
{
    public class HistoryCarStateViewModel : ViewModelBase
    {
        private readonly Dispatcher _dispatcher;
        private RoutesModel _routesModel = new RoutesModel();
        private const double SpdNormal = 90;
        private const double SpdWarning = 120;

        private LocationCollection _errorRoute = new LocationCollection();
        private LocationCollection _warningRoute = new LocationCollection();
        private List<ParkingModel> _parkings = new List<ParkingModel>();
        private LocationCollection _route = new LocationCollection();
        private readonly List<MinMidMaxValues> _obdList = new List<MinMidMaxValues>();

        public HistoryCarStateViewModel() { }
        public HistoryCarStateViewModel(IList<CarStateModel> data, DateTime day, Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
            Iswaiting = true;
            Date = day;
            SortDataByDate(data, day);
            HistoryHandler.Instance.OBDLoaded += Instance_OBDLoaded;
            HistoryHandler.Instance.StartLoadOBD(CarSelector.SelectedCar.Car.Id, day);
        }

        private void Instance_OBDLoaded(OBDHistoryDataModel model)
        {
            if (model == null || !CarSelector.SelectedCar.Car.Id.Equals(model.DevID) || !model.Data.Any()) return;
            HistoryHandler.Instance.OBDLoaded -= Instance_OBDLoaded;
            var converter = new PIDConverter();
            DispatherThreadRun(delegate
            {
                OBDList.AddRange(model.Data.GroupBy(g => g.Code).Select(n => new MinMidMaxValues
                {
                    Name = converter.GetPidInfo(n.Key),
                    Min = n.Min(o => o.Vol),
                    Mid = n.Average(o => o.Vol),
                    Max = n.Max(o => o.Vol)
                }));
            });
            OnPropertyChanged("OBDList");
        }

        #region Events

        public delegate void UpdateCenterEvent(Location southWest, Location northEast);
        public event UpdateCenterEvent CenterUpdates;
        protected virtual void OnCenterUpdates(Location southwest, Location northeast)
        {
            if (CenterUpdates != null) CenterUpdates(southwest, northeast);
        }

        #endregion

        #region Properties

        public DateTime Date { get; set; }

        public string StringDate
        {
            get { return Date.ToString("dd.MM.yyyy"); }
        }

        private double _mileage;

        public double Mileage
        {
            get { return Math.Round(_mileage, 2); }
            set { _mileage = value; }
        }

        public int MaxSpeed { get; set; }
        public int MiddleSpeed { get; set; }
        public int CountErrors { get; set; }
        public double FuelRate { get; set; }
        public double FuelRemain { get; set; }
        public bool Iswaiting { get; set; }



        public LocationCollection Route
        {
            get { return _route; }
            set
            {
                _route = value;
                OnPropertyChanged("Route");
            }
        }

        public LocationCollection WarningRoute
        {
            get { return _warningRoute; }
            set
            {
                _warningRoute = value;
                OnPropertyChanged("WarningRoute");
            }
        }

        public LocationCollection ErrorRoute
        {
            get { return _errorRoute; }
            set
            {
                _errorRoute = value;
                OnPropertyChanged("ErrorRoute");
            }
        }

        public List<ParkingModel> Parkings
        {
            get { return _parkings; }
            set
            {
                _parkings = value;
                OnPropertyChanged("Parkings");
                OnPropertyChanged("ShowParking");
            }
        }

        public bool ShowParking
        {
            get { return Parkings.Any(); }
        }

        public List<MinMidMaxValues> OBDList
        {
            get { return _obdList; }
        }

        #endregion



        #region Metods

        public void SortDataByDate(IList<CarStateModel> data, DateTime day)
        {
            ClearRoutes();

            var first = data.OrderBy(o => o.Date).FirstOrDefault();
            if (first == null) return;
            var curroute = HistoryViewModel.RouteSelect.None;
            var prev = new Location
            {
                Latitude = first.Lt / 10000.0,
                Longitude = first.Ln / 10000.0,
                FirstPoint = true
            };

            double dist = 0;
            var dc = new DistanceCalculator();
            var slowTask = new Task(delegate
            {
                var foundParking = true;
                var nullDate = first.Spd < 6 || StateDateTimeHelper.EqualInterval(new DateTime(), first)
                    ? first
                    : null;
                foreach (var item in data.OrderBy(o => o.Date))
                {
                    var itemtime = StateDateTimeHelper.GetTime(item);
                    var loc = new Location
                    {
                        Latitude = item.Lt / 10000.0,
                        Longitude = item.Ln / 10000.0
                    };
                    if (data.IndexOf(item) <= 0) continue;
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
                    if (item.Spd > 0 && Math.Abs(curdist) > .001 && !(foundParking && isParking))
                    {
                        curroute = SortLocation(prev, loc, curroute, item.Spd / 10.0, item.Date);
                        dist += curdist;
                        prev = loc;
                    }
                    first = item;
                }
                var last = data.LastOrDefault();
                if (last == null) return;
                if (!StateDateTimeHelper.EqualInterval(DateTime.Now, last)) return;
                prev = new Location
                {
                    Latitude = last.Lt / 10000.0,
                    Longitude = last.Ln / 10000.0,
                };
                if (StateDateTimeHelper.EqualInterval(day.AddDays(1), last))
                    _routesModel.Parkings.Add(new ParkingModel(StateDateTimeHelper.GetTime(last), day.AddDays(1), prev));
            });
            slowTask.ContinueWith(o => DispatherThreadRun(delegate { ContinueSortData((int)dist, data.Max(m => m.Spd / 10)); }));
            slowTask.Start();
        }

        private void ContinueSortData(int dist, double maxspd)
        {
            Mileage = dist;
            MaxSpeed = (int)maxspd;
            UpdateCenter();
        }

        private void UpdateCenter()
        {
            try
            {
                Iswaiting = false;
                if (_routesModel.Route.Count > 0)
                    Route = _routesModel.Route;
                if (_routesModel.WarningRoute.Count > 0)
                    WarningRoute = _routesModel.WarningRoute;
                if (_routesModel.ErrorRoute.Count > 0)
                    ErrorRoute = _routesModel.ErrorRoute;
                if (_routesModel.Parkings.Count > 0)
                    Parkings = _routesModel.Parkings;
                if (_routesModel.TimeRoute.Count > 0)
                    UpdateCurentPosition();
            }
            catch (Exception e)
            {
            }
        }

        private void UpdateCurentPosition()
        {
            var maxlong = _routesModel.TimeRoute.Max(o => o.Point.Longitude);
            var maxlat = _routesModel.TimeRoute.Max(o => o.Point.Latitude);
            var minlong = _routesModel.TimeRoute.Min(o => o.Point.Longitude);
            var minlat = _routesModel.TimeRoute.Min(o => o.Point.Latitude);
            OnCenterUpdates(new Location(minlat, minlong), new Location(maxlat, maxlong));
        }

        private void ClearRoutes(bool clearParcing = true)
        {
            _routesModel = new RoutesModel();
            DispatherThreadRun(delegate
            {
                Route.Clear();
                WarningRoute.Clear();
                ErrorRoute.Clear();
                if (clearParcing)
                    Parkings.Clear();
            });

        }

        private HistoryViewModel.RouteSelect SortLocation(Location prev, Location loc,
            HistoryViewModel.RouteSelect prevroute, double spd, DateTime dt)
        {
            _routesModel.TimeRoute.Add(new RoutePoint { Point = prev, Date = dt });
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

        /// <summary>
        /// filter data to different route tracks
        /// </summary>
        /// <param name="loc"></param>
        /// <param name="curroute"></param>
        private void AddToRoute(Location loc, HistoryViewModel.RouteSelect curroute)
        {
            switch (curroute)
            {
                case HistoryViewModel.RouteSelect.Normal:
                    var added = _routesModel.Route.Count < 1 ? new Location(loc, true) : loc;
                    if (_routesModel.Route.IndexOf(added) < 0)
                        _routesModel.Route.Add(added);
                    break;
                case HistoryViewModel.RouteSelect.Warning:
                    _routesModel.WarningRoute.Add(_routesModel.WarningRoute.Count < 1 ? new Location(loc, true) : loc);
                    break;
                case HistoryViewModel.RouteSelect.Error:
                    _routesModel.ErrorRoute.Add(_routesModel.ErrorRoute.Count < 1 ? new Location(loc, true) : loc);
                    break;
            }
        }

        /// <summary>
        /// Get route type by speed parameter
        /// </summary>
        /// <param name="spd"></param>
        /// <returns></returns>
        private HistoryViewModel.RouteSelect GetRouteClass(double spd)
        {
            if (spd >= SpdWarning) return HistoryViewModel.RouteSelect.Error;
            return spd <= SpdNormal ? HistoryViewModel.RouteSelect.Normal : HistoryViewModel.RouteSelect.Warning;
        }
        public override void OnPropertyChanged(string propertyName)
        {
            DispatherThreadRun(() => base.OnPropertyChanged(propertyName));
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
        #endregion


    }

    public class MinMidMaxValues
    {
        private double _min;
        private double _mid;
        private double _max;
        public string Name { get; set; }

        public double Min
        {
            get { return Math.Round(_min, 2); }
            set { _min = value; }
        }

        public double Mid
        {
            get { return Math.Round(_mid, 2); }
            set { _mid = value; }
        }

        public double Max
        {
            get { return Math.Round(_max, 2); }
            set { _max = value; }
        }
    }
}
