using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using DTCDev.Client.Cars.Service.Engine.Controls.ViewModels;
using DTCDev.Client.Cars.Service.Engine.Controls.ViewModels.Settings;
using DTCDev.Client.Cars.Service.Engine.Storage;
using DTCDev.Models.CarBase.CarList;
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
            Orders.CollectionChanged += Orders_CollectionChanged;
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

        private void DesigneAddPostOrders()
        {
            ListPost.Add(new PostViewModel { ID = 0, Name = "Post #1", StartWorkTime = 8, EndWorkTime = 17 });
            ListPost.Add(new PostViewModel { ID = 1, Name = "Post #2", StartWorkTime = 9, EndWorkTime = 18 });
            ListPost.Add(new PostViewModel { ID = 2, Name = "Post #3", StartWorkTime = 10, EndWorkTime = 19 });
            Orders.Add(new OrderViewModel
            {
                ID = Orders.Count,
                PostID = 0,
                User = new UserLightModel { Nm = "User 1" },
                Car =new DISP_Car{CarModel = new CarListBaseDataModel { CarNumber = "Demo1", Mark = "Audio", Model = "A3" }},
                DateWork = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)  + new TimeSpan(12, 0, 0), 
                IsChanged = false
            });
            Orders.Add(new OrderViewModel
            {
                ID = Orders.Count,
                PostID = 0,
                User = new UserLightModel { Nm = "User 2" },
                Car = new DISP_Car{CarModel = new CarListBaseDataModel { CarNumber = "Demo2", Mark = "Audio", Model = "A4" }},
                DateWork = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day) + new TimeSpan(13, 0, 0),
                IsChanged = false
            });
            Orders.Add(new OrderViewModel
            {
                ID = Orders.Count,
                PostID = 1,
                User = new UserLightModel { Nm = "User 3" },
                Car = new DISP_Car{CarModel = new CarListBaseDataModel { CarNumber = "Demo3", Mark = "Audio", Model = "A5" }},
                DateWork = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day) + new TimeSpan(14, 30, 0),
                IsChanged = false
            });
        }

        internal void Save(OrderViewModel model)
        {

        }
    }
}
