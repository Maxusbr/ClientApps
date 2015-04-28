using DTCDev.Client.Cars.Service.Engine.Handlers;
using DTCDev.Models.CarBase.CarStatData;
using DTCDev.Models.CarsSending.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Client.Cars.Service.Engine.Storage
{
    public class OrderStorage
    {
        private static OrderStorage _instance;

        public static OrderStorage Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new OrderStorage();
                return _instance;
            }
        }

        public OrderStorage()
        {
            _instance = this;
        }

        public event EventHandler OrdersRefreshed;

        List<OrderStorageModel> _currentOrders = new List<OrderStorageModel>();
        public List<CarOrderModel> CurrnetOrders
        {
            get { return _currentOrders.Select(p=>p.Model).ToList(); }
        }





        public void UpdateOrders()
        {
            OrdersHandler.Instance.UpdateOrders();
        }

        public void UpdateWork(WorkChangeModel model, CarOrderModel edited)
        {
            OrderStorageModel order = _currentOrders.Where(p => p.Model.OrderNumer == edited.OrderNumer).FirstOrDefault();
            if (order == null)
                return;
            WorksInfoDataModel work = order.Model.Works.Where(p => p.id == model.ID).FirstOrDefault();
            if (work == null)
                return;
            work.Cost = model.NewCost;
            order.Comment = model.Coment;
            OrdersHandler.Instance.UpdateOrderWork(model);
        }

        public void CloseOrder(int orderID)
        {
            OrdersHandler.Instance.CloseOrder(orderID);
        }







        public void SetOrders(List<CarOrderModel> data)
        {
            _currentOrders.Clear();
            data.ForEach(x => _currentOrders.Add(new OrderStorageModel { Model = x }));
            foreach (var item in _currentOrders)
            {
                foreach (var work in item.Model.Works)
                {
                    work.NHD = work.NH / 10.0m;
                }
            }
            if (OrdersRefreshed != null)
                OrdersRefreshed(this, new EventArgs());
        }

        public class OrderStorageModel
        {
            public CarOrderModel Model { get; set; }

            public string Comment { get; set; }
        }
    }
}
