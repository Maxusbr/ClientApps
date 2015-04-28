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

namespace DTCDev.Client.Cars.Service.SideMenu
{
    /// <summary>
    /// Interaction logic for SM_LastCars.xaml
    /// </summary>
    public partial class SM_LastCars : UserControl
    {
        public SM_LastCars()
        {
            InitializeComponent();
            CarStorage.Instance.LastCarsUpdated += Instance_LastCarsUpdated;
        }

        public event EventHandler ClickClose;

        void Instance_LastCarsUpdated(object sender, EventArgs e)
        {
            DisplayLastCars();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DisplayLastCars();
            headerLine.HeaderText = "Последние авто";
        }

        private void DisplayLastCars()
        {
            wrp.Children.Clear();
            foreach (var item in CarStorage.Instance.LastCarNumbers)
            {
                DISP_Car car = CarStorage.Instance.GetCarByCarNumber(item);
                SideMenuHelp.LastCarItem view = new SideMenuHelp.LastCarItem(car);
                wrp.Children.Add(view);
            }
        }

        private void headerLine_CloseClick(object sender, EventArgs e)
        {
            if (ClickClose != null)
                ClickClose(this, new EventArgs());
        }
    }
}
