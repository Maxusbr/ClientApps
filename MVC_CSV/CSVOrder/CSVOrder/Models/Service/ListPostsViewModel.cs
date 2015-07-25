using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CSVOrder.DAL.Abstract;
using CSVOrder.Models.User;

namespace CSVOrder.Models.Service
{
    public class ListPostsViewModel
    {
        private readonly List<PostModel> _listPosts = new List<PostModel>();
        private List<DateTime> _columns = new List<DateTime>();
        private List<CarOrderPostModel> _listOrders = new List<CarOrderPostModel>();
        private DateTime _date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        private readonly IServiseRepository _storage;

        public ListPostsViewModel(IServiseRepository storage)
        {
            _storage = storage;
            Update();
        }

        public ListPostsViewModel(IServiseRepository storage, DateTime dt)
        {
            _storage = storage;
            Date = dt;
            Update();
        }

        public List<DateTime> Columns
        {
            get { return _columns; }
            set { _columns = value; }
        }

        public List<PostModel> ListPosts
        {
            get { return _storage.Posts.ToList(); }
        }

        public List<CarOrderPostModel> ListOrders
        {
            get { return _listOrders; }
            set { _listOrders = value; }
        }

        public int MinTime { get; set; }

        public int MaxTime { get; set; }

        public void Update()
        {
            ListOrders.Clear();
            foreach (var item in _storage.Orders)
            {
                ListOrders.Add(_storage.GetOrder(item.OrderNumer));
            }

            MinTime = !ListPosts.Any() ? 8 : ListPosts.Min(o => o.TimeFrom).Hours;
            MaxTime = !ListPosts.Any() ? 18 : (int)Math.Ceiling(ListPosts.Max(o => o.TimeTo).TotalHours);
            Columns.Clear();
            for (var i = MinTime; i < MaxTime; i++)
            {
                Columns.Add(Date + new TimeSpan(i, 0, 0));
                Columns.Add(Date + new TimeSpan(i, 30, 0));
            }
            Columns.Add(Date + new TimeSpan(MaxTime, 0, 0));
        }

        public DateTime Date
        {
            get { return _date; }
            set
            {
                _date = value;
                Update();
            }
        }
    }
}