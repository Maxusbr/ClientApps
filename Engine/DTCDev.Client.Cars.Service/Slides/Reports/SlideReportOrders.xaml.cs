using DTCDev.Client.Cars.Service.Controls;
using DTCDev.Client.Cars.Service.Controls.PrintControls;
using DTCDev.Client.Cars.Service.Engine.Controls.ViewModels.Reports;
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

namespace DTCDev.Client.Cars.Service.Slides.Reports
{
    /// <summary>
    /// Interaction logic for SlideReportOrders.xaml
    /// </summary>
    public partial class SlideReportOrders : UserControl
    {
        OrderReportViewModel vm = new OrderReportViewModel();

        public SlideReportOrders()
        {
            InitializeComponent();
            this.DataContext = vm;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OrderReportPrintControl control = new OrderReportPrintControl(vm.Orders.ToList(), vm.DTStart, vm.DTStop);
            PrintControl print = new PrintControl();
            print.CloseClick += print_CloseClick;
            grdPrint.Children.Add(print);
            grdPrint.Visibility = Visibility.Visible;
            grdRoot.Visibility = Visibility.Collapsed;
            print.AddPrinted(control);
        }

        void print_CloseClick(object sender, EventArgs e)
        {
            grdPrint.Children.Clear();
            grdPrint.Visibility = Visibility.Collapsed;
            grdRoot.Visibility = Visibility.Visible;
        }
    }
}
