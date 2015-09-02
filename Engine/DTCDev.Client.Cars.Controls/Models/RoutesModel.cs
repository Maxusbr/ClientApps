using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using DTCDev.Client.Controls.Map;
using DTCDev.Client.ViewModel;

namespace DTCDev.Client.Cars.Controls.Models
{
    public class RoutesModel
    {
        private LocationCollection _route = new LocationCollection();
        public LocationCollection Route
        {
            get { return _route; }
            set
            {
                _route = value;
            }
        }

        private LocationCollection _warningRoute = new LocationCollection();
        public LocationCollection WarningRoute
        {
            get { return _warningRoute; }
            set
            {
                _warningRoute = value;
            }
        }

        private LocationCollection _errorRoute = new LocationCollection();
        public LocationCollection ErrorRoute
        {
            get { return _errorRoute; }
            set
            {
                _errorRoute = value;
            }
        }
        private List<ParkingModel> _parkings = new List<ParkingModel>();
        public List<ParkingModel> Parkings
        {
            get { return _parkings; }
            set
            {
                _parkings = value;
            }
        }

        public void ClearRoutes()
        {
            Route.Clear(); WarningRoute.Clear(); ErrorRoute.Clear(); //TimeRoute.Clear();
        }

        public void ClearAll()
        {
            Route.Clear(); WarningRoute.Clear(); ErrorRoute.Clear(); Parkings.Clear(); //TimeRoute.Clear();
        }

        private readonly ObservableCollection<RoutePoint> _timeRoute = new ObservableCollection<RoutePoint>();
        public ObservableCollection<RoutePoint> TimeRoute
        {
            get { return _timeRoute; }
        }
    }

    public class RoutePoint
    {
        public Location Point { get; set; }
        public DateTime Date { get; set; }
    }

    public class RouteLine: ViewModelBase
    {
        private ObservableCollection<ToolTipValue> _toolTips = new ObservableCollection<ToolTipValue>();
        private readonly Dispatcher _dispatcher;

        public RouteLine(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
            Points = new LocationCollection();
        }

        public LocationCollection Points { get; } 

        public RouteSelect TypeRoute { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ObservableCollection<ToolTipValue> ToolTips
        {
            get { return _toolTips; }
        }

        public void AddTipValue(string name, double value)
        {
            DispatherThreadRun(delegate {
                ToolTips.Add(new ToolTipValue { Name = name, Value = value });
            });
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

    public class ToolTipValue
    {
        public string Name { get; set; }
        public double Value { get; set; }
    }

    public enum RouteSelect
    {
        Normal,
        Warning,
        Error,
        None
    };
}
