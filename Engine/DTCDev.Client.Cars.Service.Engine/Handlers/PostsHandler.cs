using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using DTCDev.Client.Cars.Service.Engine.Controls.ViewModels;
using DTCDev.Client.Cars.Service.Engine.Controls.ViewModels.Settings;
using DTCDev.Client.Cars.Service.Engine.Storage;
using DTCDev.Models.CarBase.CarList;
using DTCDev.Models.Service;
using DTCDev.Models.User;

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
        void Orders_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (OrdersCollectionChanged != null) OrdersCollectionChanged(sender, e);
        }

        public event EventHandler ListPostCollectionChanged;
        void ListPost_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
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
                IsChanged = false
            });
            Orders.Add(new OrderViewModel
            {
                ID = Orders.Count,
                PostID = 3,
                User = Users[1],
                Car = new DISP_Car { CarModel = new CarListBaseDataModel { CarNumber = "Demo2", Mark = "Audio", Model = "A4" } },
                DateWork = Date + new TimeSpan(13, 0, 0),
                IsChanged = false
            });
            Orders.Add(new OrderViewModel
            {
                ID = Orders.Count,
                PostID = 1,
                User = Users[2],
                Car = new DISP_Car { CarModel = new CarListBaseDataModel { CarNumber = "Demo3", Mark = "Audio", Model = "A5" } },
                DateWork = Date + new TimeSpan(14, 30, 0),
                IsChanged = false
            });

        }

        internal void Save(OrderViewModel order)
        {
            if (Orders.FirstOrDefault(o => o.Equals(order)) == null)
            {
                order.CanDeleted = true;
                Orders.Add(order);
            }
            Orders_CollectionChanged(this, null);
        }

        internal void DeleteOrder(OrderViewModel order)
        {
            var ord = Orders.FirstOrDefault(o => o.Equals(order));
            if(ord == null) return;
            Orders.Remove(ord);
            Orders_CollectionChanged(this, null);
        }
    }
}
