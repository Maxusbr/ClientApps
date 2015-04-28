using DTCDev.Client.Cars.Service.Engine.Controls.ViewModels.Slides;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DTCDev.Client.Cars.Service.Slides
{
    /// <summary>
    /// Interaction logic for SlideOrdersList.xaml
    /// </summary>
    public partial class SlideOrdersList : UserControl
    {
        SlideOrdersListViewModel vm = new SlideOrdersListViewModel();

        public SlideOrdersList()
        {
            InitializeComponent();
            this.DataContext = vm;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Controls.Order.OrderRecomendations control = new Controls.Order.OrderRecomendations();
            control.CloseClick += control_CloseClick;
            DisplayDetails(control);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Controls.Order.OrderWorkMapView control = new Controls.Order.OrderWorkMapView();
            control.CloseClick += control_CloseClick;
            control.DataContext = vm;
            vm.StartLoadInfo();
            DisplayDetails(control);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }

        private void DisplayDetails(UserControl control)
        {
            grdDetails.Visibility = Visibility.Visible;
            grdMain.Visibility = Visibility.Collapsed;
            control.DataContext = this.DataContext;
            grdDetails.Children.Add(control);
        }

        void control_CloseClick(object sender, EventArgs e)
        {
            grdDetails.Children.Clear();
            grdDetails.Visibility = Visibility.Collapsed;
            grdMain.Visibility = Visibility.Visible;
        }
        
    }
}
