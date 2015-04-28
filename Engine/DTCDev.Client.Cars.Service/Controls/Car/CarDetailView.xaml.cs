using DTCDev.Client.Cars.Service.Engine.Controls.ViewModels;
using DTCDev.Client.Cars.Service.Engine.Controls.ViewModels.Cars;
using DTCDev.Client.Cars.Service.Engine.Storage;
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

namespace DTCDev.Client.Cars.Service.Controls
{
    /// <summary>
    /// Interaction logic for CarDetailView.xaml
    /// </summary>
    public partial class CarDetailView : UserControl
    {
        public CarDetailView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            grdDetails.Children.Clear();
            grdContentPlacer.Visibility = Visibility.Visible;
            txtContentHeader.Text = "Список ошибок ЭБУ";
            grdDetails.Children.Add(new Car.CarDTCView());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            grdDetails.Children.Clear();
            grdContentPlacer.Visibility = Visibility.Visible;
            txtContentHeader.Text = "История работ";
            grdDetails.Children.Add(new Car.CarHistoryView());
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            grdDetails.Children.Clear();
            grdContentPlacer.Visibility = Visibility.Visible;
            txtContentHeader.Text = "Диагностика автомобиля";
            Car.CarParamsView view = new Car.CarParamsView();
            grdDetails.Children.Add(view);
        }

        private void btnDetailsClose_Click(object sender, RoutedEventArgs e)
        {
            grdDetails.Children.Clear();
            grdContentPlacer.Visibility = Visibility.Collapsed;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            grdDetails.Children.Clear();
            grdContentPlacer.Visibility = Visibility.Visible;
            txtContentHeader.Text = "Добавить работу";
            Car.CarAddWorkView view = new Car.CarAddWorkView();
            CarAddWorkViewModel vm = new CarAddWorkViewModel();
            view.DataContext = vm;
            vm.CreateOrderComplete += vm_CreateOrderComplete;
            grdDetails.Children.Add(view);
        }

        void vm_CreateOrderComplete(object sender, EventArgs e)
        {
            grdDetails.Children.Clear();
            grdContentPlacer.Visibility = Visibility.Collapsed;
        }
    }
}
