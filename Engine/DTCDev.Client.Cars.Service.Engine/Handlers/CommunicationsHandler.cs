using DTCDev.Client.Cars.Service.Engine.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCDev.Models;
using Newtonsoft.Json;

namespace DTCDev.Client.Cars.Service.Engine.Handlers
{
    public class CommunicationsHandler
    {
        private static CommunicationsHandler _instance;
        public static CommunicationsHandler Instance
        {
            get { return _instance ?? (_instance = new CommunicationsHandler()); }
        }

        public CommunicationsHandler()
        {
            _instance = this;
        }

        public void SendMessage(int id, string message)
        {
            try
            {
                var model = new DicDataModel { ID = id, Data = message };
                TCPConnection.Instance.SendData("FA" + JsonConvert.SerializeObject(model));
            }
            catch { }
        }

        public void AddOrder(int count, string msg)
        {
            try
            {
                var model = new DicDataModel { ID = count, Data = msg };
                TCPConnection.Instance.SendData("FB" + JsonConvert.SerializeObject(model));
            }
            catch { }
        }







        #region TCP

        public void Split(char fx, string row)
        {
            switch (fx)
            {

            }
        }

        #endregion TCP
    }
}
