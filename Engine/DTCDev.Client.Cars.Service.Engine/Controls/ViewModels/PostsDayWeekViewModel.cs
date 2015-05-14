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
            AddPostOrders();
        }

        private void AddPostOrders()
        {
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

        readonly ObservableCollection<PostOrdersViewModel> _listPostOrder = new ObservableCollection<PostOrdersViewModel>();
        private PostOrdersViewModel _selectedItem;
        public ObservableCollection<PostOrdersViewModel> ListPostOrder { get { return _listPostOrder; } }

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

        public string PostName
        {
            get { return SelectedItem != null && SelectedItem.Post != null ? SelectedItem.Post.Name : ""; }
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
    }
}
