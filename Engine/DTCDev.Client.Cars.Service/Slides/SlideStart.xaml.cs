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
    /// Interaction logic for SlideStart.xaml
    /// </summary>
    public partial class SlideStart : UserControl
    {
        public SlideStart()
        {
            InitializeComponent();
            this.DataContext = new SlideStartViewModel();
        }

        public event EventHandler ClickAutoList;
        public event EventHandler ClickAddCar;
        public event EventHandler ClickSettings;
        public event EventHandler ClickCalendar;
        public event EventHandler ClickViewCarsInfo;
        
        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (ClickAutoList != null) ClickAutoList(this, new EventArgs());
        }

        private void Border_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            if (ClickCalendar != null)
                ClickCalendar(this, new EventArgs());
        }

        private void brdrAddCar_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (ClickAddCar != null) ClickAddCar(this, new EventArgs());
        }

        private void brdrSettings_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (ClickSettings != null)
                ClickSettings(this, new EventArgs());
        }






        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void brdrViewCars_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (ClickViewCarsInfo != null)
                ClickViewCarsInfo(this, new EventArgs());
        }
    }
}
