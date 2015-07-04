using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using DTCDev.Client.Cars.Service.Engine.Controls.ViewModels.Settings;
using DTCDev.Client.Cars.Service.Engine.Handlers;
using DTCDev.Client.ViewModel;

namespace DTCDev.Client.Cars.Service.Engine.Controls.ViewModels
{
    public class PostOrdersViewModel : ViewModelBase, IDateList
    {
        private readonly PostViewModel _post;
        private readonly ObservableCollection<OrderViewModel> _orders = new ObservableCollection<OrderViewModel>();
        private int _selectedOrder = -1;
        private readonly List<DateListModel> _datesList = new List<DateListModel>();
        public bool WeekStyle { get { return PostsHandler.Instance.WeekStyle; } }
        public DateTime Date { get { return PostsHandler.Instance.Date; } }

        public PostOrdersViewModel(PostViewModel post)
        {
            _post = post;
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {

            }
        }

        public PostViewModel Post { get { return _post; } }

        public ObservableCollection<OrderViewModel> Orders { get { return _orders; } }

        public int SelectedOrder
        {
            get { return _selectedOrder; }
            set { _selectedOrder = value; }
        }

        internal void Update(OrderViewModel ord)
        {
            var order = Orders.FirstOrDefault(o => o.ID == ord.ID);
            if (order == null)
            {
                //ord.IsCompleteSaved += Order_IsCompleteSaved;
                Orders.Add(ord);
            }
            else
            {
                order.UpdateOrder(ord);
            }
            UpdateDatesList();
        }

        private void UpdateDatesList()
        {
            _datesList.Clear();
            foreach (var el in Orders)
            {
                _datesList.Add(new DateListModel
                {
                    DateWork = el.DateWork,
                    ToolTip = string.Format("{0} ({1})",
                        el.User != null && el.User.Tp == 0 ? el.User.Nm : "Через сайт",
                        el.Car != null ? el.Car.MarkModelName : ""), 
                        Сondition = el.InUse
                });
            }
        }

        internal void Update(PostViewModel post)
        {
            if (post == null) return;
            Post.IsChange = post.IsChange;
            Post.Name = post.Name;
            Post.PostType = post.PostType;
            Post.StartWorkTime = post.StartWorkTime;
            Post.EndWorkTime = post.EndWorkTime;
        }

        public int StartWorkTime { get { return Post.StartWorkTime; } }

        public int EndWorkTime { get { return Post.EndWorkTime; } }

        public List<DateListModel> DatesList
        {
            get { return _datesList; }
        }
    }
}
