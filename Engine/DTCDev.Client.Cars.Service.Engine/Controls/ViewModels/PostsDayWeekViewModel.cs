using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using DTCDev.Client.Cars.Service.Engine.Controls.ViewModels.Settings;
using DTCDev.Client.Cars.Service.Engine.Handlers;
using DTCDev.Client.ViewModel;
using DTCDev.Models.CarBase.CarList;
using DTCDev.Models.User;

namespace DTCDev.Client.Cars.Service.Engine.Controls.ViewModels
{
    public class PostsDayWeekViewModel : ViewModelBase
    {
        private readonly PostsHandler _handler = PostsHandler.Instance;
        public PostsDayWeekViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {

            } 
            _handler.OrdersCollectionChanged += _handler_OrdersCollectionChanged;
            AddPostOrders();
        }

        void _handler_OrdersCollectionChanged(object sender, EventArgs e)
        {
            AddPostOrders();
        }

        private void AddPostOrders()
        {
            ListPostOrder.Clear();
            foreach (var post in _handler.ListPost)
            {
                var el = ListPostOrder.FirstOrDefault(o => o.Post != null && o.Post.ID == post.ID);
                var added = el == null;
                if (added) el = new PostOrdersViewModel(post);
                foreach (var order in _handler.Orders.Where(o => o.PostID == el.Post.ID))
                {
                    el.Update(order);
                }
                if (added)
                    ListPostOrder.Add(el);
                else 
                    el.Update(post);
            }
        }

        public DateTime Date
        {
            get { return _handler.Date; }
            set
            {
                if(_handler.Date == value) return;
                _handler.Date = value;
                OnPropertyChanged("Date");
                OnPropertyChanged("Period");
            }
        }

        public bool WeekStyle
        {
            get { return _handler.WeekStyle; }
            set
            {
                if (_handler.WeekStyle == value) return;
                _handler.WeekStyle = value;
                if(value)
                    UpdateDate();
                OnPropertyChanged("WeekStyle");
                OnPropertyChanged("Period");
                OnPropertyChanged("ToggleButtonName");
            }
        }

        private void UpdateDate()
        {
            _handler.Date = _handler.Date.AddDays(DayOfWeek.Monday - _handler.Date.DayOfWeek);
        }

        public string Period
        {
            get { return WeekStyle ? string.Format("{0} - {1}", Date.ToShortDateString(), 
                (Date + new TimeSpan(7,0,0,0)).ToShortDateString()): Date.ToShortDateString(); }
        }

        readonly ObservableCollection<PostOrdersViewModel> _listPostOrder = new ObservableCollection<PostOrdersViewModel>();
        
        public ObservableCollection<PostOrdersViewModel> ListPostOrder { get { return _listPostOrder; } }

        private PostOrdersViewModel _selectedItem;
        public PostOrdersViewModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                OnPropertyChanged("SelectedItem");
                OnPropertyChanged("PostName");
            }
        }

        public int StartTime
        {
            get
            {
                return ListPostOrder.Count == 0 ? 8 : ListPostOrder.Min(o => o.Post.StartWorkTime);
            }
        }

        public int EndTime
        {
            get
            {
                return ListPostOrder.Count == 0 ? 17 : ListPostOrder.Max(o => o.Post.EndWorkTime);
            }
        }

        public string PostName
        {
            get { return SelectedItem != null && SelectedItem.Post != null ? SelectedItem.Post.Name : ""; }
        }

        public string ToggleButtonName
        {
            get { return WeekStyle ? "Неделя" : "День"; }
        }


        public void UpdateOrders(OrderViewModel model)
        {
            var pos = ListPostOrder.FirstOrDefault(o => o.Post.ID == model.PostID);
            if(pos == null) return;
            var ord = pos.Orders.FirstOrDefault(o => o.Equals(model));
            model.IsChanged = false;
            if (ord == null)
                pos.Orders.Add(model);
            else
                ord.UpdateOrder(model);
            _handler.Save(model);
        }

        public void DeleteOrder(OrderViewModel order)
        {
            var pos = ListPostOrder.FirstOrDefault(o => o.Post.ID == order.PostID);
            if(pos == null) return;
            var ord = pos.Orders.FirstOrDefault(o => o.Equals(order));
            if (ord == null) return;
            pos.Orders.Remove(ord);
            _handler.DeleteOrder(order);
        }
    }
}
