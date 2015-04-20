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

namespace DTCDev.Client.Cars.Controls.Controls.Settings
{
    /// <summary>
    /// Interaction logic for SettingsBase.xaml
    /// </summary>
    public partial class SettingsBase : UserControl
    {
        public SettingsBase()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            grdContent.Children.Clear();
            grdContent.Visibility = Visibility.Collapsed;
            stkBack.Visibility = Visibility.Collapsed;
        }

        private void grdZonesToCar_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SettingsCarsToZone control = new SettingsCarsToZone();
            grdContent.Children.Add(control);
            grdContent.Visibility = Visibility.Visible;
            stkBack.Visibility = Visibility.Visible;
        }

        private void grdCars_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SettingsCars control = new SettingsCars();
            grdContent.Children.Add(control);
            grdContent.Visibility = Visibility.Visible;
            stkBack.Visibility = Visibility.Visible;
        }

        private void grdZones_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SettingsZones control = new SettingsZones();
            grdContent.Children.Add(control);
            grdContent.Visibility = Visibility.Visible;
            stkBack.Visibility = Visibility.Visible;
        }

        private void grdControllers_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SettingsControllers control = new SettingsControllers();
            grdContent.Children.Add(control);
            grdContent.Visibility = Visibility.Visible;
            stkBack.Visibility = Visibility.Visible;
        }

        private void grdControllerLines_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SettingsControllerLines control = new SettingsControllerLines();
            grdContent.Children.Add(control);
            grdContent.Visibility = Visibility.Visible;
            stkBack.Visibility = Visibility.Visible;
        }
    }
}
