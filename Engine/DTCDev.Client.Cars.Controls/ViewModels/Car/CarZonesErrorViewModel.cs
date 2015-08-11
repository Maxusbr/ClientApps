using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Emit;
using System.Windows;
using DTCDev.Client.Cars.Controls.Helpers;
using DTCDev.Client.Cars.Engine.DisplayModels;
using DTCDev.Client.Cars.Engine.Handlers.Cars;
using DTCDev.Client.ViewModel;
using DTCDev.Models.CarsSending.Car;
using DTCDev.Models.Date;

namespace DTCDev.Client.Cars.Controls.ViewModels.Car
{
    public class CarZonesErrorViewModel : ViewModelBase
    {
        private DateTime _displayedHistoryDate;
        private static CarZonesErrorViewModel _instance;
        public static CarZonesErrorViewModel Instance
        {
            get { return _instance ?? (_instance = new CarZonesErrorViewModel()); }
        }

        public CarZonesErrorViewModel()
        {
            _instance = this;
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

            _carHandler = CarsHandler.Instance;
            _carHandler.CarsRefreshed += Instance_CarsRefreshed;

            if (!_carHandler.Cars.Any())
                _carHandler.Update();
            _zoneHandler = ZonesHandler.Instance;
            if (!_zoneHandler.Zones.Any())
                _zoneHandler.Update();

            
        }

        private readonly ObservableCollection<DISP_Car> _cars = new ObservableCollection<DISP_Car>();
        public ObservableCollection<DISP_Car> Cars
        {
            get { return _cars; }
        }

        private readonly CarsHandler _carHandler;
        private readonly ZonesHandler _zoneHandler;

        private readonly ObservableCollection<CarInOutViewModel> _carsInOut = new ObservableCollection<CarInOutViewModel>();
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


        private void Instance_CarsRefreshed(IEnumerable<DISP_Car> data)
        {
            var dispCars = data as IList<DISP_Car> ?? data.ToList();
            foreach (var el in dispCars.Where(c => c.IsCreated))
            {
                Cars.Add(el);
                el.PropertyChanged += car_PropertyChanged;
            }
            dispCars.Where(c => c.IsCreated).ToList().ForEach(o => o.IsCreated = false);
        }

        private void car_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!e.PropertyName.Equals("Location")) return;
            var car = sender as DISP_Car;
            if (car == null) return;
            var zone = _zoneHandler.Zones.FirstOrDefault(o => o.ID == car.ZoneData.ZoneId);
            if (zone == null) return;
            MonitorCarInZone(car, zone);

        }

        private readonly List<DISP_Car> _inErrorCars = new List<DISP_Car>();
        private void MonitorCarInZone(DISP_Car car, VmPolyline zone)
        {
            var error = CalcLeavingZone.Instance.FillContains(car.Navigation.LocationPoint, zone.MovedLocations);
            var indx = _inErrorCars.IndexOf(car);
            if (indx < 0)
            {
                if (error)
                    AddCarToErrorLog(car, zone);
            }
            else
            {
                if (!error)
                    RemoveCarFromErrorLog(car, zone);
            }
        }

        private void RemoveCarFromErrorLog(DISP_Car car, VmPolyline zone)
        {
            var el = new CarInOutViewModel()
            {
                CarID = car.ID,
                CarNumber = car.Name,
                ZoneId = zone.ID,
                ZoneName = zone.Name,
                InZoneDate = ToDate(car.Data.DateUpdate)
            };
            el.PropertyChanged += CarInOut_PropertyChanged;
            CarsInOut.Add(el);
            _inErrorCars.Remove(car);
        }

        private void CarInOut_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(!e.PropertyName.Equals("RemoveElement")) return;
            var el = sender as CarInOutViewModel;
            if(el == null) return;
            el.PropertyChanged -= CarInOut_PropertyChanged;
            CarsInOut.Remove(el);
        }

        private void AddCarToErrorLog(DISP_Car car, VmPolyline zone)
        {
            var el = new CarInOutViewModel()
            {
                CarID = car.ID,
                CarNumber = car.Name,
                ZoneId = zone.ID,
                ZoneName = zone.Name,
                OutZoneDate = ToDate(car.Data.DateUpdate)
            };
            el.PropertyChanged += CarInOut_PropertyChanged;
            CarsInOut.Add(el);
            _inErrorCars.Add(car);
        }

        private static DateTime ToDate(DateTimeDataModel dm)
        {
            return new DateTime(dm.Y, dm.M, dm.D, dm.hh, dm.mm, dm.ss);
        }

    }
}
