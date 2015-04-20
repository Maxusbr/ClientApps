using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using DTCDev.Models.CarBase.CarStatData;

namespace DTCDev.Client.Cars.Manager.Data
{
    public class CarDataStorage
    {
        private static CarDataStorage _instance;
        public static CarDataStorage Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new CarDataStorage();
                return _instance;
            }
        }

        public CarDataStorage()
        {
            _instance = this;
        }

        private ObservableCollection<CarPresenterDataModel> _carsList = new ObservableCollection<CarPresenterDataModel>();

        public ObservableCollection<CarPresenterDataModel> CarsList
        {
            get { return _carsList; }
        }

        private ObservableCollection<CurrentDevicesDataModel> _devices = new ObservableCollection<CurrentDevicesDataModel>();

        public ObservableCollection<CurrentDevicesDataModel> Devices
        {
            get { return _devices; }
        }

        public event EventHandler GetTrackersList;

        public delegate void AddTrackerHandler(TrackerTypes.TTypes ttype, string deviceID, string phoneNumber);
        public event AddTrackerHandler AddTracker;

        public delegate void AddCarHandler(string carNumber, string deviceID, DateTime dateBuy, DateTime dateProduce, int startDistance, int currentDistance);
        public event AddCarHandler AddCar;

        public event EventHandler GetCarsEvent;


        public void Update()
        {
            if (GetTrackersList != null)
                GetTrackersList(this, new EventArgs());

            if (GetCarsEvent != null)
                GetCarsEvent(this, new EventArgs());
        }

        public void CallAddTracker(TrackerTypes.TTypes ttype, string deviceID, string phoneNumber)
        {
            if (AddTracker != null)
                AddTracker(ttype, deviceID, phoneNumber);
        }

        public void CallAddCarNumber(string carNumber, string deviceID, DateTime dateBuy, DateTime dateProduce, int startDistance, int currentDistance)
        {
            if (AddCar != null)
                AddCar(carNumber, deviceID, dateBuy, dateProduce, startDistance, currentDistance);
        }

        public void SetCars(List<CarPresenterDataModel> data)
        {
            data = data.OrderBy(p => p.CarNumber).ToList();
            CarsList.Clear();
            foreach (var item in data)
            {
                CarsList.Add(item);
            }
        }

        public void SetDevices(List<CurrentDevicesDataModel> devices)
        {
            Devices.Clear();
            devices.ForEach(o => Devices.Add(o));
        }
    }
}
