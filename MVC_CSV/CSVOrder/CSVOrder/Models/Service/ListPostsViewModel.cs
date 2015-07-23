using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSVOrder.Models.Service
{
    public class ListPostsViewModel
    {
        private List<PostModel> _listPosts = new List<PostModel>();
        private List<string> _columns = new List<string>();
        private List<CarOrderPostModel> _listOrders = new List<CarOrderPostModel>();

        public ListPostsViewModel()
        {
            Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            DemoData();
            Update();
        }

        public List<string> Columns
        {
            get { return _columns; }
            set { _columns = value; }
        }

        public List<PostModel> ListPosts
        {
            get { return _listPosts; }
            set
            {
                _listPosts = value;
                Update();
            }
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
            MinTime = !ListPosts.Any() ? 8 : ListPosts.Min(o => o.TimeFrom).Hours;
            MaxTime = !ListPosts.Any() ? 18 : (int)Math.Ceiling(ListPosts.Max(o => o.TimeTo).TotalHours);
            Columns.Clear();
            Columns.Add("Название поста");
            for (var i = MinTime; i < MaxTime; i++)
            {
                Columns.Add(string.Format("{0}:00", i));
                Columns.Add(string.Format("{0}:30", i));
            }
            Columns.Add(string.Format("{0}:00", MaxTime));
        }

        public DateTime Date { get; set; }

        private void DemoData()
        {
            for (var i = 1; i <= 5; i++)
                ListPosts.Add(new PostModel
                {
                    Id = i,
                    Name = string.Format("Пост № {0:00}", i),
                    TimeFrom = new TimeSpan(i % 2 == 0 ? 8 : 9, 0, 0),
                    TimeTo = new TimeSpan(i % 2 == 0 ? 19 : 18, 0, 0)
                });
            ListOrders.Add(new CarOrderPostModel
            {
                OrderNumer = 0,
                CarNumber = "Demo 1",
                DateWork = Date.AddHours(10).AddMinutes(25),
                PostId = 4
            });
            ListOrders.Add(new CarOrderPostModel
            {
                OrderNumer = 1,
                CarNumber = "Demo 2",
                DateWork = Date.AddHours(15).AddMinutes(32),
                PostId = 1
            });
        }
    }
}