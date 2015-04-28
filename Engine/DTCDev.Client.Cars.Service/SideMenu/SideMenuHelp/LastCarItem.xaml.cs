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
using DTCDev.Client.Cars.Service.Engine.Storage;

namespace DTCDev.Client.Cars.Service.SideMenu.SideMenuHelp
{
    /// <summary>
    /// Interaction logic for LastCarItem.xaml
    /// </summary>
    public partial class LastCarItem : UserControl
    {
        public LastCarItem()
        {
            InitializeComponent();
        }

        DISP_Car _car;

        public LastCarItem(DISP_Car car)
        {
            InitializeComponent();
            _car = car;
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateData();
        }

        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            brdrEnter.Visibility = Visibility.Visible;
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            brdrEnter.Visibility = Visibility.Collapsed;
        }

        private void UpdateData()
        {
            if (_car == null)
                return;
            txtNumber.Text = _car.CarModel.CarNumber;
            string distance = _car.CarModel.IncomeDistance.ToString();
            txtDist1.Text = txtDist2.Text = txtDist3.Text = txtDist4.Text = txtDist5.Text = "0";
            int count = distance.Length;
            string[] chars = new string[count];
            for (int i = 0; i < count; i++)
            {
                chars[i] = distance[count - 1 - i].ToString();
            }
            if (count > 0)
                txtDist1.Text = chars[0];
            if (count > 1)
                txtDist2.Text = chars[1];
            if (count > 2)
                txtDist3.Text = chars[2];
            if (count > 3)
                txtDist4.Text = chars[3];
            if (count > 4)
                txtDist5.Text = chars[4];

            if (_car.CarModel.IncomeDistance < 0)
            {
                brdrGood1.Visibility = brdrGood2.Visibility =
                    brdrGood3.Visibility = brdrGood4.Visibility =
                    brdrGood5.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void UserControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            CarStorage.Instance.GetCarDetails(_car.CarModel.CarNumber);
        }
    }
}
