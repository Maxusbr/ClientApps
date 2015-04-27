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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DTCDev.Client.Cars.Controls.ViewModels.Car;
using DTCDev.Client.Cars.Engine.AppLogic;
using DTCDev.Client.Cars.Engine.DisplayModels;

namespace DTCDev.Client.Cars.Controls.Controls.Car
{
    /// <summary>
    /// Interaction logic for CarDetailsView.xaml
    /// </summary>
    public partial class CarDetailsView : UserControl
    {
        public CarDetailsView()
        {
            InitializeComponent();
        }

        DISP_Car _currentCar;

        public void UpdateCarData(DISP_Car carData)
        {
            if (_currentCar != null)
                _currentCar.PropertyChanged -= _currentCar_PropertyChanged;
            _currentCar = carData;
            _currentCar.PropertyChanged += _currentCar_PropertyChanged;
            UpdateData();
        }

        void _currentCar_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            UpdateData();
        }

        private void UpdateData()
        {
            txtDate.Text = "Последняя связь - " + _currentCar.DateNavigation;
            txtSpeed.Text = _currentCar.strSpeed;
            txtSat.Text = _currentCar.CountSatelite.ToString();
            if (_currentCar.OBD.Where(p => p.Key == "2F").FirstOrDefault() != null)
                txtFuel.Text = _currentCar.OBD.Where(p => p.Key == "2F").First().Value + " %";
            else
                txtFuel.Text = _currentCar.FuelLevel + " л.";
        }
    }
}
