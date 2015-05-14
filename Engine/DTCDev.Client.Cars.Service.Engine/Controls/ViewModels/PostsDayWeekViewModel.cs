using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using DTCDev.Client.Cars.Service.Engine.Controls.ViewModels.Settings;
using DTCDev.Client.ViewModel;
using DTCDev.Models.CarBase.CarList;
using DTCDev.Models.User;

namespace DTCDev.Client.Cars.Service.Engine.Controls.ViewModels
{
    public class PostsDayWeekViewModel : ViewModelBase
    {
        public PostsDayWeekViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {

            }
            var el = new PostOrdersViewModel(new PostViewModel { Name = "Post #1", StartWorkTime = 8, EndWorkTime = 17 });
            el.Orders.Add(new OrderViewModel
            {
                ID = el.Orders.Count,
                User = new UserLightModel { Nm = "User 1" },
                Car = new CarListBaseDataModel { CarNumber = "Demo1", Mark = "Audio", Model = "A3" },
                DateWork = DateTime.Now
            });
            el.Orders.Add(new OrderViewModel
            {
                ID = el.Orders.Count,
                User = new UserLightModel { Nm = "User 2" },
                Car = new CarListBaseDataModel { CarNumber = "Demo1", Mark = "Audio", Model = "A5" },
                DateWork = DateTime.Now + new TimeSpan(1, 0, 0)
            });
            ListPostOrder.Add(el);
            el = new PostOrdersViewModel(new PostViewModel { Name = "Post #2", StartWorkTime = 9, EndWorkTime = 18 });
            el.Orders.Add(new OrderViewModel
            {
                ID = el.Orders.Count,
                User = new UserLightModel { Nm = "User 3" },
                Car = new CarListBaseDataModel { CarNumber = "Demo2", Mark = "Audio", Model = "A5" },
                DateWork = DateTime.Now - new TimeSpan(1, 0, 0)
            });
            ListPostOrder.Add(el);
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
                return ListPostOrder.Count == 0 ? 8 : ListPostOrder.Max(o => o.Post.EndWorkTime);
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
    }
}
