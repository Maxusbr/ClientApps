using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using DTCDev.Client.Cars.Manager.Data;
using DTCDev.Models.CarBase.CarStatData;
using Newtonsoft.Json;

namespace DTCDev.Client.Cars.Engine.Handlers.Cars
{
    public class CarBaseHandler
    {
         private static CarBaseHandler _instance;

        public static CarBaseHandler Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new CarBaseHandler();
                return _instance;
            }
        }

        public CarBaseHandler()
        {
            _instance = this;
        }

        public void Init()
        {
            SpecificationDataStorage.Instance.GetMarks += Instance_GetMarks;
            SpecificationDataStorage.Instance.GetModels += Instance_GetModels;
            SpecificationDataStorage.Instance.GetBodies += Instance_GetBodies;
            SpecificationDataStorage.Instance.GetEngineTypes += Instance_GetEngineTypes;
            SpecificationDataStorage.Instance.GetEngines += Instance_GetEngines;
            SpecificationDataStorage.Instance.GetTransTypes += Instance_GetTransTypes;

            CarDataStorage.Instance.AddCar += Instance_AddCar;
            CarDataStorage.Instance.AddTracker += Instance_AddTracker;
            CarDataStorage.Instance.GetTrackersList += Instance_GetTrackersList;
            CarDataStorage.Instance.GetCarsEvent += Instance_GetCarsEvent;
        }

        void Instance_GetCarsEvent(object sender, EventArgs e)
        {
            try
            {
                TCPConnection.Instance.SendData("TH");
                TCPConnection.Instance.SendData("TG");
            }
            catch { }
        }



        #region Request handlers

        void Instance_GetMarks(object sender, EventArgs e)
        {
            try
            {
                TCPConnection.Instance.SendData("TA");
            }
            catch { }
        }

        void Instance_GetModels(int idPearent)
        {
            try
            {
                TCPConnection.Instance.SendData("TB"+idPearent.ToString());
            }
            catch { }
        }

        void Instance_GetBodies(int idPearent)
        {
            try
            {
                TCPConnection.Instance.SendData("TC" + idPearent.ToString());
            }
            catch { }
        }

        void Instance_GetEngineTypes(int idModel, int idPearent)
        {
            try
            {
                TCPConnection.Instance.SendData("TD" + idModel.ToString()+";"+ idPearent.ToString());
            }
            catch { }
        }

        void Instance_GetEngines(int idModel, int idPearent)
        {
            try
            {
                TCPConnection.Instance.SendData("TE" + idModel.ToString() + ";" + idPearent.ToString());
            }
            catch { }
        }

        void Instance_GetTransTypes(int idModel, int idPearent)
        {
            try
            {
                TCPConnection.Instance.SendData("TF" + idModel.ToString() + ";" + idPearent.ToString());
            }
            catch { }
        }





        void Instance_GetTrackersList(object sender, EventArgs e)
        {
            try
            {
                TCPConnection.Instance.SendData("TG");
            }
            catch { }
        }

        void Instance_AddTracker(TrackerTypes.TTypes ttype, string deviceID, string phoneNumber)
        {
            throw new NotImplementedException();
        }

        void Instance_AddCar(string carNumber, string deviceID, DateTime dateBuy, DateTime dateProduce, int startDistance, int currentDistance)
        {
            throw new NotImplementedException();
        }

        #endregion Request handlers



        #region TCP

        public void Split(char fx, string row)
        {
            switch (fx)
            {
                case 'a':
                case'A':
                    FillMarks(row);
                    break;
                case 'b':
                case'B':
                    FillModels(row);
                    break;
                case 'c':
                case 'C':
                    FillBodyTypes(row);
                    break;
                case'd':
                case'D':
                    FillEngineTypes(row);
                    break;
                case 'e':
                case 'E':
                    FillEngineVolume(row);
                    break;
                case 'f':
                case 'F':
                    FillTransType(row);
                    break;
                case 'g':
                case 'G':
                    FillDevices(row);
                    break;
                case 'h':
                case'H':
                    FillCarsList(row);
                    break;
            }
        }

        private void FillMarks(string row)
        {
            try
            {
                var temp = JsonConvert.DeserializeObject<List<KVPBase>>(row);
                if (temp == null) return;
                //if (Application.Current != null)
                //    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                //    {
                        SpecificationDataStorage.Instance.SetMarks(temp);
                    //}));
            }
            catch { }
        }

        private void FillModels(string row)
        {
            try
            {
                var temp = JsonConvert.DeserializeObject<List<KVPBase>>(row);
                if (temp == null) return;
                //if (Application.Current != null)
                //    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                //    {
                        SpecificationDataStorage.Instance.SetModels(temp);
                    //}));
            }
            catch { }
        }

        private void FillBodyTypes(string row)
        {
            try
            {
                var temp = JsonConvert.DeserializeObject<List<KVPBase>>(row);
                if (temp == null) return;
                //if (Application.Current != null)
                //    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                //    {
                        SpecificationDataStorage.Instance.SetBodyTypes(temp);
                    //}));
            }
            catch { }
        }

        private void FillEngineTypes(string row)
        {
            try
            {
                var temp = JsonConvert.DeserializeObject<List<KVPBase>>(row);
                if (temp == null) return;
                //if (Application.Current != null)
                //    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                //    {
                        SpecificationDataStorage.Instance.SetEngineTypes(temp);
                    //}));
            }
            catch { }
        }

        private void FillEngineVolume(string row)
        {
            try
            {
                var temp = JsonConvert.DeserializeObject<List<KVPBase>>(row);
                if (temp == null) return;
                //if (Application.Current != null)
                //    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                //    {
                        SpecificationDataStorage.Instance.SetEngineVolumes(temp);
                    //}));
            }
            catch { }
        }

        private void FillTransType(string row)
        {
            try
            {
                var temp = JsonConvert.DeserializeObject<List<KVPBase>>(row);
                if (temp == null) return;
                //if (Application.Current != null)
                //    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                //    {
                        SpecificationDataStorage.Instance.SetTransTypes(temp);
                    //}));
            }
            catch { }
        }

        private void FillDevices(string row)
        {
            try
            {
                var devices = JsonConvert.DeserializeObject<List<CurrentDevicesDataModel>>(row);
                if (devices == null) return;
                //if (Application.Current != null)
                //    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                //    {
                        CarDataStorage.Instance.SetDevices(devices);
                    //}));
            }
            catch { }
        }

        private void FillCarsList(string row)
        {
            try
            {
                var cars = JsonConvert.DeserializeObject<List<CarPresenterDataModel>>(row);
                if (cars == null) return;
                //if (Application.Current != null)
                //    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                //    {
                        CarDataStorage.Instance.SetCars(cars);
                        foreach (var item in cars)
                        {
                            CarsHandler.Instance.SetCarMarkModel(item.CarNumber, item.Mark, item.Model);
                        }
                    //}));
            }
            catch { }
        }

        #endregion TCP
    }
}
