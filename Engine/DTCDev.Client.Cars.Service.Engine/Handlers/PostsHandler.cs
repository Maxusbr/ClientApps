using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows;
using DTCDev.Client.Cars.Service.Engine.Controls.ViewModels;
using DTCDev.Client.Cars.Service.Engine.Controls.ViewModels.Settings;
using DTCDev.Client.Cars.Service.Engine.Network;
using DTCDev.Client.Cars.Service.Engine.Storage;
using DTCDev.Models.CarBase.CarList;
using DTCDev.Models.CarsSending.Order;
using DTCDev.Models.Date;
using DTCDev.Models.Service;
using DTCDev.Models.User;
using Newtonsoft.Json;

namespace DTCDev.Client.Cars.Service.Engine.Handlers
{
    public class PostsHandler
    {
        private static PostsHandler _instance;
        public static PostsHandler Instance
        {
            get { return _instance ?? (_instance = new PostsHandler()); }
        }

        public PostsHandler()
        {
            _instance = this;
            ListPost.CollectionChanged += ListPost_CollectionChanged;
            //Orders.CollectionChanged += Orders_CollectionChanged;
            DesigneAddPostOrders();
        }

        public event EventHandler OrdersCollectionChanged;
        void Orders_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (OrdersCollectionChanged != null) OrdersCollectionChanged(sender, e);
        }

        public event EventHandler ListPostCollectionChanged;
        void ListPost_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (ListPostCollectionChanged != null) ListPostCollectionChanged(sender, e);
        }



        private readonly ObservableCollection<PostViewModel> _listPost = new ObservableCollection<PostViewModel>();
        public ObservableCollection<PostViewModel> ListPost { get { return _listPost; } }

        private readonly ObservableCollection<OrderViewModel> _orders = new ObservableCollection<OrderViewModel>();
        public ObservableCollection<OrderViewModel> Orders { get { return _orders; } }

        private readonly List<string> _listTypePost = new List<string>();
        public List<string> ListTypePost { get { return _listTypePost; } }

        private readonly List<UserLightModel> _users = new List<UserLightModel>();
        public List<UserLightModel> Users { get { return _users; } }

        public DateTime Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        public bool WeekStyle = false;

        private void DesigneAddPostOrders()
        {
            foreach (var dep in PersonalHandler.Instance.Model.Departments)
                foreach (var post in dep.Posts)
                {
                    ListPost.Add(new PostViewModel
                    {
                        ID = post.ID,
                        Name = string.Format("{0} ({1})", post.Name, dep.Name),
                        StartWorkTime = post.TimeFrom,
                        EndWorkTime = post.TimeTo
                    });
                }

            Users.Add(new UserLightModel { Nm = "Иванов Петр Иванович" });
            // Записан через сайт
            Users.Add(new UserLightModel { Nm = "Петров Иван Сидорович", Tp = 1});
            Users.Add(new UserLightModel { Nm = "Сидоров Сидор Петрович" });
            Orders.Add(new OrderViewModel
            {
                ID = Orders.Count,
                PostID = 2,
                User = Users[0],
                Car = new DISP_Car { CarModel = new CarListBaseDataModel { CarNumber = "Demo1", Mark = "Audio", Model = "A3" } },
                DateWork = Date + new TimeSpan(12, 0, 0),
                IsChanged = false,
                IsCanMoveToUse = true
            });
            Orders.Add(new OrderViewModel
            {
                ID = Orders.Count,
                PostID = 3,
                User = Users[1],
                Car = new DISP_Car { CarModel = new CarListBaseDataModel { CarNumber = "Demo2", Mark = "Audio", Model = "A4" } },
                DateWork = Date + new TimeSpan(13, 0, 0),
                IsChanged = false,
                IsCanMoveToUse = true
            });
            Orders.Add(new OrderViewModel
            {
                ID = Orders.Count,
                PostID = 1,
                User = Users[2],
                Car = new DISP_Car { CarModel = new CarListBaseDataModel { CarNumber = "Demo3", Mark = "Audio", Model = "A5" } },
                DateWork = Date + new TimeSpan(14, 30, 0),
                IsChanged = false, InUse = true
            });

        }

        internal void Save(OrderViewModel order)
        {
            //TODO Send model to server
            UpdateOrderWork(order.GetModel());
            if (Orders.FirstOrDefault(o => o.Equals(order)) == null)
            {
                order.CanDeleted = true;
                Orders.Add(order);
            }
            Orders_CollectionChanged(this, null);
        }

        internal void DeleteOrder(OrderViewModel order)
        {
            //TODO Send delete comand to server
            var ord = Orders.FirstOrDefault(o => o.Equals(order));
            if(ord == null) return;
            Orders.Remove(ord);
            Orders_CollectionChanged(this, null);
        }

        /// <summary>
        /// Переместить заявку в заказ-наряды
        /// </summary>
        /// <param name="order"></param>
        internal void InUse(OrderViewModel order)
        {
            var model = order.GetModel();
            if(model == null) return;
            model.InUse = 1;
            model.DTCreate = new DateDataModel(DateTime.Now);
            UpdateOrderWork(model);
            OrdersHandler.Instance.AddNewOrder(model);
        }

        /// <summary>
        /// Получить список заявок
        /// </summary>
        public void UpdateOrders()
        {
            try
            {
                //TODO исправить префикс
                TCPConnection.Instance.SendData("DA");
            }
            catch { }
        }

        /// <summary>
        /// Обновить заявку
        /// </summary>
        /// <param name="model"></param>
        public void UpdateOrderWork(CarOrderPostModel model)
        {
            if(model == null) return;
            try
            {
                //TODO исправить префикс
                TCPConnection.Instance.SendData("DB" + JsonConvert.SerializeObject(model));
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
            var temp = JsonConvert.DeserializeObject<List<CarOrderPostModel>>(row);
            if (Application.Current != null)
                Application.Current.Dispatcher.BeginInvoke(new Action(() => SetOrders(temp)));
        }

        private void SetOrders(IEnumerable<CarOrderPostModel> listOrders)
        {
            Orders.Clear();
            foreach (var item in listOrders)
            {
                var order = new OrderViewModel
                {
                    Car = CarStorage.Instance.GetCarByCarNumber(item.CarNumber),
                    ID = item.OrderNumer,
                    PostID = item.PostId,
                    User = item.User,
                    DateWork = item.DateWork.ToDate, 
                    InUse = item.InUse != 0,
                    IsCanMoveToUse = item.InUse == 0
                };
                Orders.Add(order);
            }
        }
    }
}
