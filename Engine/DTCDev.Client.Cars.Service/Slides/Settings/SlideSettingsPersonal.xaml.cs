using DTCDev.Client.Cars.Service.Engine.Controls.ViewModels.Settings;
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

namespace DTCDev.Client.Cars.Service.Slides.Settings
{
    /// <summary>
    /// Interaction logic for SlideSettingsPersonal.xaml
    /// </summary>
    public partial class SlideSettingsPersonal : UserControl
    {
        public SlideSettingsPersonal()
        {
            InitializeComponent();
            this.DataContext = new SettingsPersonalViewModel();
        }

        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            btnEditCompany.Visibility = Visibility.Visible;
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            btnEditCompany.Visibility = Visibility.Collapsed;
        }

        private void Border_MouseEnter_1(object sender, MouseEventArgs e)
        {
            btnEditPhones.Visibility = Visibility.Visible;
        }

        private void Border_MouseLeave_1(object sender, MouseEventArgs e)
        {
            btnEditPhones.Visibility = Visibility.Collapsed;
        }

        private void brdrAdrEditor_MouseEnter(object sender, MouseEventArgs e)
        {
            btnEditAdress.Visibility = Visibility.Visible;
        }

        private void brdrAdrEditor_MouseLeave(object sender, MouseEventArgs e)
        {
            btnEditAdress.Visibility = Visibility.Collapsed;
        }

        private void Border_MouseEnter_2(object sender, MouseEventArgs e)
        {
            btnEditCost.Visibility = Visibility.Visible;
        }

        private void Border_MouseLeave_2(object sender, MouseEventArgs e)
        {
            btnEditCost.Visibility = Visibility.Collapsed;
        }
    }
}
