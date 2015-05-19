using KOTServerTester.Network;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KOTServerTester.Handlers
{
    class CarsHandler
    {
        private static CarsHandler _instance;
        public static CarsHandler Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new CarsHandler();
                return _instance;
            }
        }

        public CarsHandler()
        {
            _instance = this;
        }

        List<CAR> _cars = new List<CAR>();

        public List<CAR> Cars { get { return _cars; } }

        #region OutCommands

        public void GetCars()
        {
            TCPConnection.Instance.SendData("BA");
        }

        public void GetCarState()
        {
            TCPConnection.Instance.SendData("BB");
        }

        public void GetCarWorks(string devID)
        {
            TCPConnection.Instance.SendData("BC"+devID);
        }

        public void GetCarWorksHistory(string devID)
        {
            TCPConnection.Instance.SendData("BD" + devID);
        }

        public void GetAlarmsState()
        {
            TCPConnection.Instance.SendData("BE");
        }

        #endregion


        public void Split(char fx, string row)
        {
            switch(fx)
            {
                case 'A':
                    SplitCars(row);
                    break;
            }
        }

        private void SplitCars(string row)
        {
            List<Models.CarListBaseDataModel> temp = JsonConvert.DeserializeObject<List<Models.CarListBaseDataModel>>(row);
            _cars.Clear();
            foreach (var item in temp)
            {
                _cars.Add(new CAR
                {
                    CarBase = item
                });
            }
        }
    }
}
