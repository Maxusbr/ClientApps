using DTCDev.Client.Cars.Service.Engine.Network;
using DTCDev.Client.Cars.Service.Engine.Storage;
using DTCDev.Models.CarsSending.Order;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace DTCDev.Client.Cars.Service.Engine.Handlers
{
    public class OrdersHandler
    {
        private static OrdersHandler _instance;

        public static OrdersHandler Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new OrdersHandler();
                return _instance;
            }
        }

        public OrdersHandler()
        {
            _instance = this;
        }




        public void UpdateOrders()
        {
            try
            {
                TCPConnection.Instance.SendData("DA");
            }
            catch { }
        }

        public void UpdateOrderWork(WorkChangeModel model)
        {
            try
            {
                TCPConnection.Instance.SendData("DB" + JsonConvert.SerializeObject(model));
            }
            catch { }
        }

        public void CloseOrder(int orderID)
        {
            try
            {
                TCPConnection.Instance.SendData("DC" + orderID);
            }
            catch { }
        }






        public void Split(char fx, string row)
        {
            switch (fx)
            {
                case 'a':
                case 'A':
                    FillOrdersData(row);
                    break;
                case 'c':
                case 'C':
                    UpdateOrders();
                    break;
            }
        }

        private void FillOrdersData(string row)
        {
            try
            {
                List<CarOrderModel> temp = JsonConvert.DeserializeObject<List<CarOrderModel>>(row);
                if (Application.Current != null)
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            OrderStorage.Instance.SetOrders(temp);
                        }));
            }
            catch { }
        }

        internal void AddNewOrder(CarOrderModel model)
        {
            //TODO Add NewOrder
        }
    }
}
