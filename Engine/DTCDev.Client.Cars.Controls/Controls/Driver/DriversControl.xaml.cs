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
using DTCDev.Client.Cars.Controls.ViewModels.Driver;
using DTCDev.Client.Cars.Engine.Handlers.Cars;

namespace DTCDev.Client.Cars.Controls.Controls.Driver
{
    /// <summary>
    /// Interaction logic for DriversControl.xaml
    /// </summary>
    public partial class DriversControl : UserControl
    {
        public DriversControl()
        {
            InitializeComponent();
            this.DataContext = new DriversWinViewModel();
        }

        private void pnlAdd_AddClick(object sender, EventArgs e)
        {
            AddNewDriver control = new AddNewDriver();
            control.ShowDialog();
            if (control.DialogResult == true)
                DriverHandler.Instance.EditDriver(control.Driver);
        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            grdContent.Children.Clear();
            grdContent.Visibility = Visibility.Collapsed;
            stkBack.Visibility = Visibility.Collapsed;
        }

        private void grdDistanceReport_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DriversList control = new DriversList();
            grdContent.Children.Add(control);
            grdContent.Visibility = Visibility.Visible;
            stkBack.Visibility = Visibility.Visible;
        }

        private void grdDriversToCar_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DriverToCar control = new DriverToCar();
            grdContent.Children.Add(control);
            grdContent.Visibility = Visibility.Visible;
            stkBack.Visibility = Visibility.Visible;
        }
    }
}
