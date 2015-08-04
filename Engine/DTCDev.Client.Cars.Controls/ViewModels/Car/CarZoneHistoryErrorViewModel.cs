using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using DTCDev.Client.Cars.Controls.Helpers;
using DTCDev.Client.Cars.Engine.AppLogic;
using DTCDev.Client.Cars.Engine.DisplayModels;
using DTCDev.Client.Cars.Engine.Handlers.Cars;
using DTCDev.Client.Controls.Map;
using DTCDev.Client.ViewModel;
using DTCDev.Models.Date;

namespace DTCDev.Client.Cars.Controls.ViewModels.Car
{
    public class CarZoneHistoryErrorViewModel: ViewModelBase
    {
        private DateTime _displayedHistoryDate;

        public CarZoneHistoryErrorViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                var el = new CarInOutViewModel()
                {
                    CarID = "1",
                    CarNumber = "AS12341",
                    ZoneId = 1,
                    ZoneName = "OPT",
                    InZoneDate = DateTime.Now,
                    OutZoneDate = DateTime.Now
                };
                el.PropertyChanged += CarInOut_PropertyChanged;
                CarsInOut.Add(el);
            }
            _zoneHandler = ZonesHandler.Instance;
            if (!_zoneHandler.Zones.Any())
                _zoneHandler.Update();
        }

        public CarZoneHistoryErrorViewModel(DISP_Car car, LocationCollection routeCollection)
        {
            SelectedCar = car;
            _routeCollection = routeCollection;
            _zoneHandler = ZonesHandler.Instance;
            _zoneHandler.LoadCompleted += zoneHandler_LoadCompleted;
            if (!_zoneHandler.Zones.Any())
                _zoneHandler.Update();
        }

        private void zoneHandler_LoadCompleted(string resultOperation)
        {
            CarZoneChanged();
        }

        private DISP_Car _selectedCar;
        public DISP_Car SelectedCar
        {
            get
            {
                return _selectedCar;
            }
            set
            {
                if (_selectedCar == value) return;
                _selectedCar = value;
                CarZoneChanged();
                OnPropertyChanged("SelectedCar");
            }
        }

        private readonly ZonesHandler _zoneHandler;

        private VmPolyline _zone;
        private LocationCollection _routeCollection;
        private readonly ObservableCollection<CarInOutViewModel> _carsInOut = new ObservableCollection<CarInOutViewModel>();


        void CarZoneChanged()
        {
            if (_selectedCar != null)
                _zone = _zoneHandler.Zones.FirstOrDefault(o => o.ID == _selectedCar.ZoneData.ZoneId);
        }
       

        public ObservableCollection<CarInOutViewModel> CarsInOut
        {
            get { return _carsInOut; }
        }

        private DateTime GetOutTime()
        {
            //List<CarStateModel> temp = HistoryHandler.Instance.HistoryMessages.Where(p =>
            //    p.yy == _displayedHistoryDate.Year &&
            //    p.Mnth == _displayedHistoryDate.Month &&
            //    p.dd == _displayedHistoryDate.Day).ToList();

            return new DateTime();
        }

        private DateTime GetInTime()
        {


            return new DateTime();
        }

        private static DateTime ToDate(DateTimeDataModel dm)
        {
            return new DateTime(dm.Y, dm.M, dm.D, dm.hh, dm.mm, dm.ss);
        }

        private bool _findError = true;
        internal void Update(DISP_Car car, VmPolyline zone, MovedLocationCollection routeOpacited)
        {
            if (!car.Equals(SelectedCar)) SelectedCar = car;
            Application.Current.Dispatcher.BeginInvoke(new Action(() => CarsInOut.Clear()));
            foreach (var location in routeOpacited)
            {
                var inzone = CalcLeavingZone.Instance.FillContains(location, zone.MovedLocations);
                if (_findError)
                {
                    if (inzone) continue;
                    AddCarToErrorLog(car, zone, location.Dates);
                    _findError = false;
                }
                else
                {
                    if (!inzone) continue;
                    AddBackCarToErrorLog(car, zone, location.Dates);
                    _findError = true;
                }
            }
        }

        private void AddBackCarToErrorLog(DISP_Car car, VmPolyline zone, DateTime dt)
        {
            var el = new CarInOutViewModel()
            {
                CarID = car.ID,
                CarNumber = car.Name,
                ZoneId = zone.ID,
                ZoneName = zone.Name,
                InZoneDate = dt
            };
            el.PropertyChanged += CarInOut_PropertyChanged;
            Application.Current.Dispatcher.BeginInvoke(new Action(() => CarsInOut.Add(el)));
        }

        private void CarInOut_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!e.PropertyName.Equals("RemoveElement")) return;
            var el = sender as CarInOutViewModel;
            if (el == null) return;
            el.PropertyChanged -= CarInOut_PropertyChanged;
            Application.Current.Dispatcher.BeginInvoke(new Action(() => CarsInOut.Remove(el)));
        }

        private void AddCarToErrorLog(DISP_Car car, VmPolyline zone, DateTime dt)
        {
            var el = new CarInOutViewModel()
            {
                CarID = car.ID,
                CarNumber = car.Name,
                ZoneId = zone.ID,
                ZoneName = zone.Name,
                OutZoneDate = dt
            };
            el.PropertyChanged += CarInOut_PropertyChanged;
            Application.Current.Dispatcher.BeginInvoke(new Action(() => CarsInOut.Add(el)));
        }
    }
}
