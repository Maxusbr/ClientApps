using DTCDev.Client.Cars.Service.Engine.Controls.ViewModels;
using DTCDev.Client.Cars.Service.Engine.Handlers;
using DTCDev.Models.CarBase.CarList;
using DTCDev.Models.CarBase.CarStatData;
using DTCDev.Models.CarsSending.Car;
using DTCDev.Models.CarsSending.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace DTCDev.Client.Cars.Service.Engine.Storage
{
    public class CarStorage
    {
        private static CarStorage _instance;

        public static CarStorage Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new CarStorage();
                return _instance;
            }
        }

        public event EventHandler OBDHistoryLoaded;
        public event EventHandler LastCarsUpdated;
        public event EventHandler CarsWorksUpdated; 
        public event EventHandler LoadComplete, LoadError;
        public event EventHandler ErrorsLoaded;
        public event EventHandler CurrentOBDLoaded;

        public CarStorage()
        {
            _instance = this;
        }

        private List<DISP_Car> _cars = new List<DISP_Car>();
        public List<DISP_Car> Cars
        {
            get { return _cars; }
            set { _cars = value; }
        }

        private DISP_Car _selectedCar;
        public DISP_Car SelectedCar
        {
            get { return _selectedCar; }
            set { _selectedCar = value; }
        }

        private DTCDev.Models.CarBase.CarList.CarListDetailsDataModel _selectedCarDetails;
        public DTCDev.Models.CarBase.CarList.CarListDetailsDataModel SelectedCarDetails
        {
            get { return _selectedCarDetails; }
            set { _selectedCarDetails = value; }
        }

        private List<WorksWithFlagDataModel> _carWorks = new List<WorksWithFlagDataModel>();
        public List<WorksWithFlagDataModel> CarWorks { get { return _carWorks; } set { _carWorks = value; } }



        public void Start()
        {
            Thread tr = new Thread(TrOnLineUpdater);
            tr.Start();
            CarsHandler.Instance.OnLineUpdated += Instance_OnLineUpdated;
            StartCurrentOBD();
        }

        void Instance_OnLineUpdated(List<Models.DicDataModel> data)
        {
            foreach (var item in data)
            {
                DISP_Car car = _cars.Where(p => p.CarModel.CarNumber == item.Data).FirstOrDefault();
                if (car != null)
                    car.UpdatedTime = item.ID;
            }
        }


        public DISP_Car GetCarByCarNumber(string number)
        {
            return _cars.Where(p => p.CarModel.CarNumber == number).FirstOrDefault();
        }

        /// <summary>
        /// Generate request for get car details
        /// </summary>
        /// <param name="carNumber"></param>
        public void GetCarDetails(string carNumber)
        {
            CarsHandler.Instance.GetCarDetails(carNumber);

            //update last viewed cars list
            if (_lastCarNumbers.IndexOf(carNumber) != -1)
                return;
            _lastCarNumbers.Add(carNumber);
            if (_lastCarNumbers.Count() > 10)
                _lastCarNumbers.RemoveAt(0);
            UserSettingsStorage.Instance.UserSettings.LastCarNumbers = _lastCarNumbers;
            if (LastCarsUpdated != null)
                LastCarsUpdated(this, new EventArgs());
        }

        private List<string> _lastCarNumbers = new List<string>();

        public List<string> LastCarNumbers
        {
            get { return _lastCarNumbers; }
            set
            {
                _lastCarNumbers = value;
            }
        }







        public void SetOrderData(CarOrderModel model)
        {
            DISP_Car car = _cars.Where(p => p.CarModel.CarNumber == model.CarNumber).FirstOrDefault();
            if (car != null)
                car.Order = model;
        }

        public void SetCarData(List<CarListBaseDataModel> data)
        {
            Cars.Clear();
            foreach (var item in data)
            {
                Cars.Add(new DISP_Car { CarModel = item });
            }
            if (LoadComplete != null)
                LoadComplete(this, new EventArgs());
        }

        public void SetLoadError()
        {
            if (LoadError != null)
                LoadError(this, new EventArgs());
        }

        public void SetErrorsLoaded()
        {
            if (ErrorsLoaded != null)
                ErrorsLoaded(this, new EventArgs());
        }

        public void SetOBDHistoryToCar(OBDHistoryDataModel model)
        {
            DISP_Car car = _cars.Where(p => p.CarModel.DID == model.DevID).FirstOrDefault();
            if (car != null)
            {
                car.OBDHistory = model;
                if (OBDHistoryLoaded != null)
                    OBDHistoryLoaded(this, new EventArgs());
            }
        }

        public void SetCurrentOBD(List<CarStateOBDModel> data)
        {
            string currDID = "-1";
            if (SelectedCar != null)
                currDID = SelectedCar.CarModel.DID;
            foreach (var item in data)
            {
                DISP_Car car = _cars.Where(p => p.CarModel.DID == item.DID).FirstOrDefault();
                if (car != null)
                {
                    car.CurrentOBD = item;
                    if (item.DID == currDID)
                        if (CurrentOBDLoaded != null)
                            CurrentOBDLoaded(this, new EventArgs());
                }
            }
        }

        public void SetCarTotalWorks(List<WorksWithFlagDataModel> data)
        {
            CarWorks = data;
            if (CarsWorksUpdated != null)
                CarsWorksUpdated(this, new EventArgs());
        }









        public void UpdateCarWorks()
        {
            if (SelectedCar == null)
                return;
            CarsHandler.Instance.UpdateCarWorks(SelectedCar.CarModel.CarNumber);
        }





        /// <summary>
        /// Метод для периодического поллинга параметров автомобиля
        /// </summary>
        private void TrOnLineUpdater()
        {
            while (true)
            {
                Thread.Sleep(10000);
                //запрашиваем последнее время обновления параметров
                CarsHandler.Instance.GetLastUpdatedTime();
                Thread.Sleep(10000);
                //запрашиваем список ошибок для всех автомобилей
                CarsHandler.Instance.GetCarsErrors();
            }
        }

        private void StartCurrentOBD()
        {
            new Thread(TrCurrentOBD).Start();
        }

        private void TrCurrentOBD()
        {
            try
            {
                Thread.Sleep(5000);
                while (true)
                {
                    CarsHandler.Instance.GetCurrentOBD();
                    Thread.Sleep(30000);
                }
            }
            catch
            {
                Thread.Sleep(1000);
                StartCurrentOBD();
            }
        }
    }
}
