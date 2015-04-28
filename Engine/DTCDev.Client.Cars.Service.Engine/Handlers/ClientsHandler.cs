using DTCDev.Client.Cars.Service.Engine.Network;
using DTCDev.Models.CarsSending.Sending;
using DTCDev.Models.User;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace DTCDev.Client.Cars.Service.Engine.Handlers
{
    public class ClientsHandler
    {
        private static ClientsHandler _instance;
        public static ClientsHandler Instance
        {
            get { return _instance ?? (_instance = new ClientsHandler()); }
        }

        public ClientsHandler()
        {
            _instance = this;
        }


        public event EventHandler UsersUpdated;
        public event EventHandler SendingUserCountUpdated;


        public List<UserLightModel> Users { get; set; }

        public int UserCount { get; set; }





        public void GetClients()
        {
            try
            {
                TCPConnection.Instance.SendData("CA");
            }
            catch { }
        }

        public void GetClientsCount(int param, SendingClientsEnum.SendModel model)
        {
            SendingCalcRequestModel req = new SendingCalcRequestModel
            {
                Mode = (int)model,
                Param = param
            };
            try
            {
                TCPConnection.Instance.SendData("CB" + JsonConvert.SerializeObject(req));
            }
            catch { }
        }





        public void Split(char fx, string row)
        {
            switch (fx)
            {
                case 'a':
                case 'A':
                    FillClientslist(row);
                    break;
                case 'b':
                case 'B':
                    FillClientsCount(row);
                    break;
            }

        }

        private void FillClientslist(string row)
        {
            try
            {
                List<UserLightModel> temp = JsonConvert.DeserializeObject<List<UserLightModel>>(row);
                if (temp != null)
                {
                    if (Application.Current != null)
                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                Users = temp;
                                if (UsersUpdated != null)
                                    UsersUpdated(this, new EventArgs());
                            }));
                }
            }
            catch { }
        }

        private void FillClientsCount(string row)
        {
            int col = 0;
            Int32.TryParse(row, out col);
            if (Application.Current != null)
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        UserCount = col;
                        if (SendingUserCountUpdated != null)
                            SendingUserCountUpdated(this, new EventArgs());
                    }));
        }
    }
}
