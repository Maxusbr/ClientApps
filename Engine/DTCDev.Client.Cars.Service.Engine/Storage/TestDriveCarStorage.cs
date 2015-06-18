using DTCDev.Client.Cars.Service.Engine.Controls.ViewModels;
using DTCDev.Client.Cars.Service.Engine.Handlers;
using DTCDev.Client.Cars.Service.Engine.Network;
using DTCDev.Models.CarBase.CarList;
using DTCDev.Models.CarBase.CarStatData;
using DTCDev.Models.CarsSending.Car;
using DTCDev.Models.CarsSending.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using DTCDev.Models.User;

namespace DTCDev.Client.Cars.Service.Engine.Storage
{
    public class TestDriveCarStorage
    {
        private static TestDriveCarStorage _instance;

        public static TestDriveCarStorage Instance
        {
            get { return _instance ?? (_instance = new TestDriveCarStorage()); }
        }

        public event EventHandler LoadComplete, LoadError;
        public event EventHandler ErrorsLoaded;
        public event CarsHandler.GetCarDetailHandler OnGetCarDetailCompleteOnlyFill;
        protected virtual void OnOnGetCarDetailCompleteOnlyFill(CarListDetailsDataModel cardetail)
        {
            var handler = OnGetCarDetailCompleteOnlyFill;
            if (handler != null) handler(cardetail);
        }
        protected virtual void OnLoadComplete()
        {
            var handler = LoadComplete;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        public TestDriveCarStorage()
        {
            _instance = this;
            CarStorage.Instance.LoadComplete += Instance_LoadComplete;
            CarsHandler.Instance.OnGetCarDetailCompleteOnlyFill += Instance_OnGetCarDetailCompleteOnlyFill;
            Instance_LoadComplete(this, null);
            DesigneAddData();
        }

        void Instance_OnGetCarDetailCompleteOnlyFill(CarListDetailsDataModel carDetail)
        {
            //TODO добавить заполнение модели
            OnOnGetCarDetailCompleteOnlyFill(carDetail);
        }

        void Instance_LoadComplete(object sender, EventArgs e)
        {
            Cars.Clear();
            foreach (var item in CarStorage.Instance.Cars)
            {
                Cars.Add(item);
            }
            OnLoadComplete();
        }


        private readonly List<TestDriveModel> _testdrives = new List<TestDriveModel>();
        /// <summary>
        /// список всех автомобилей в системе
        /// </summary>
        public List<TestDriveModel> TestDrives
        {
            get { return _testdrives; }
        }


        private List<DISP_Car> _cars = new List<DISP_Car>();
        /// <summary>
        /// список всех автомобилей в системе
        /// </summary>
        public List<DISP_Car> Cars
        {
            get { return _cars; }
            set { _cars = value; }
        }
        private DISP_Car _selectedCar;
        /// <summary>
        /// Выбранный пользователем автомобиль из списка
        /// </summary>
        public DISP_Car SelectedCar
        {
            get { return _selectedCar; }
            set { _selectedCar = value; }
        }

        private CarListDetailsDataModel _selectedCarDetails;
        public CarListDetailsDataModel SelectedCarDetails
        {
            get { return _selectedCarDetails; }
            set { _selectedCarDetails = value; }
        }

        public DISP_Car GetCarByCarNumber(string number)
        {
            return _cars.FirstOrDefault(p => p.CarModel.CarNumber == number);
        }


        /// <summary>
        /// Запрос списка автомобилей
        /// </summary>
        public void GetCars()
        {
            //TODO добавить собственный запрос
            CarsHandler.Instance.GetCars();
        }
        /// <summary>
        /// Generate request for get car details
        /// </summary>
        /// <param name="carNumber"></param>
        public void GetCarDetails(string carNumber)
        {
            //TODO добавить собственный запрос
            CarsHandler.Instance.GetCarDetails(carNumber, true);
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


        private int _timeTestDrive = 30;
        public int TimeTestDrive
        {
            get { return _timeTestDrive; }
            set { _timeTestDrive = value; }
        }

        public DateTime Date { get; set; }

        public bool WeekStyle { get; set; }


        private readonly List<TestDriveUserModel> _users = new List<TestDriveUserModel>();

        public List<TestDriveUserModel> Users { get { return _users; } }

        private void DesigneAddData()
        {
            Users.Add(new TestDriveUserModel { Name = "Иванов Петр Иванович" });
            Users.Add(new TestDriveUserModel { Name = "Петров Иван Сидорович" });
            Users.Add(new TestDriveUserModel { Name = "Сидоров Сидор Петрович" });
        }


        internal void Save(TestDriveModel testdrive)
        {
            //TODO Save TestDrive
        }
    }
}
