using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using DTCDev.Client.Cars.Service.Engine.Controls.ViewModels.Settings;
using DTCDev.Client.ViewModel;

namespace DTCDev.Client.Cars.Service.Engine.Controls.ViewModels
{
    public class PostOrdersViewModel : ViewModelBase
    {
        private readonly PostViewModel _post;
        private readonly ObservableCollection<OrderViewModel> _orders = new ObservableCollection<OrderViewModel>();
        private int _selectedOrder = -1;

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
    }
}
