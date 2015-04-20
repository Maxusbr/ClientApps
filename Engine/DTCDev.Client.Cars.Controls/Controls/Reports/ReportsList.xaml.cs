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

namespace DTCDev.Client.Cars.Controls.Controls.Reports
{
    /// <summary>
    /// Interaction logic for ReportsList.xaml
    /// </summary>
    public partial class ReportsList : UserControl
    {
        public ReportsList()
        {
            InitializeComponent();
        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            grdContent.Children.Clear();
            grdContent.Visibility = Visibility.Collapsed;
            stkBack.Visibility = Visibility.Collapsed;
        }

        private void grdDistanceReport_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            grdContent.Children.Add(new DistanceReport());
            grdContent.Visibility = Visibility.Visible;
            stkBack.Visibility = Visibility.Visible;
        }

        private void grdSpeedReport_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            grdContent.Children.Add(new SpeedReport());
            grdContent.Visibility = Visibility.Visible;
            stkBack.Visibility = Visibility.Visible;
        }

        private void grdSpeedDropReport_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            grdContent.Children.Add(new SpeedOveralReport());
            grdContent.Visibility = Visibility.Visible;
            stkBack.Visibility = Visibility.Visible;
        }

        private void grdStartStopReport_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            grdContent.Children.Add(new WorkTimeReport());
            grdContent.Visibility = Visibility.Visible;
            stkBack.Visibility = Visibility.Visible;
        }

        private void grdParkingReport_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void grdFuelReport_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            grdContent.Children.Add(new FuelReport());
            grdContent.Visibility = Visibility.Visible;
            stkBack.Visibility = Visibility.Visible;
        }
    }
}
