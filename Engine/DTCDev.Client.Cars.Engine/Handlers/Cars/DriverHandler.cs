using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using DTCDev.Models.CarsSending;
using Newtonsoft.Json;

namespace DTCDev.Client.Cars.Engine.Handlers.Cars
{
    public class DriverHandler
    {
        private static DriverHandler _instance;

        public static DriverHandler Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DriverHandler();
                return _instance;
            }
        }

        public DriverHandler()
        {
            _instance = this;
        }



        private List<DriverModel> _driver = new List<DriverModel>();


        public List<DriverModel> ListDriver
        {
            get { return _driver; }
        }





        public event EventHandler DriverRefreshed;

        public event EventHandler DriversChangesCompleted;



        public void Split(char fx, string row)
        {
            switch (fx)
            {
                case'a':
                case 'A':
                    FillDrivers(row);
                    break;
                case 'b':
                case 'B':
                    LoadDriver();
                    break;
                case 'c':
                case 'C':
                    LoadDriver();
                    break;
            }
        }

        private void FillDrivers(string row)
        {
            try
            {
                List<DriverModel> d = JsonConvert.DeserializeObject<List<DriverModel>>(row);
                if (Application.Current != null)
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            if (d != null)
                                _driver = d;
                            if (DriverRefreshed != null)
                                DriverRefreshed(this, new EventArgs());
                            CarsHandler.Instance.UpdateDrivers();
                        }));
            }
            catch { }
        }



        /// <summary>
        /// load driver from server
        /// </summary>
        public void LoadDriver()
        {
            try
            {
                TCPConnection.Instance.SendData("DA");
            }
            catch { }
        }


        public void EditDriver(DriverModel model)
        {
            try
            {
                string request = JsonConvert.SerializeObject(model);
                TCPConnection.Instance.SendData("DB" + request);
            }
            catch { }
        }

        public void DeleteDriver(DriverModel model)
        {
            try
            {
                string request = JsonConvert.SerializeObject(model);
                TCPConnection.Instance.SendData("DC" + request);
            }
            catch { }
        }
    }
}
