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
        CarDetailsViewModel _vm = new CarDetailsViewModel();
        public CarDetailsView()
        {
            InitializeComponent();
            this.DataContext = _vm;
        }

        DISP_Car _currentCar;

        public event EventHandler CloseMe;

        public void UpdateCarData(DISP_Car carData)
        {
            if (_currentCar != null)
                _currentCar.PropertyChanged -= _currentCar_PropertyChanged;
            _currentCar = carData;
            _currentCar.PropertyChanged += _currentCar_PropertyChanged;
            UpdateData();
            _vm.CAR = carData;
        }

        void _currentCar_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            UpdateData();
        }

        private void UpdateData()
        {
            speedPresenter.SetData((int)_currentCar.Current_Speed);
            sattelitePresenter.SetData(_currentCar.Data.Navigation.Sattelites);
            if(_currentCar.OBD.Where(p=>p.Key=="0C").FirstOrDefault()!=null)
            {
                int vol = 0;
                Int32.TryParse(_currentCar.OBD.Where(p => p.Key == "0C").First().Value, out vol);
                engineRPMPresenter.SetData(vol);
            }
            //txtDate.Text = "Последняя связь - " + _currentCar.DateNavigation;
            //txtSpeed.Text = _currentCar.strSpeed;
            //txtSat.Text = _currentCar.CountSatelite.ToString();
            //if (_currentCar.OBD.Where(p => p.Key == "2F").FirstOrDefault() != null)
            //    txtFuel.Text = _currentCar.OBD.Where(p => p.Key == "2F").First().Value + " %";
            //else
            //    txtFuel.Text = _currentCar.FuelLevel + " л.";

            PIDConverter converter = new PIDConverter();
            ////stkOBD.Children.Clear();
            //foreach (var item in _currentCar.OBD)
            //{
            //    StackPanel stk = new StackPanel();
            //    stk.Orientation = Orientation.Horizontal;
            //    TextBlock txtText = new TextBlock();
            //    txtText.TextWrapping = TextWrapping.Wrap;
            //    txtText.Text = converter.GetPidInfo(item.Key);
            //    stk.Children.Add(txtText);
            //    txtText.Margin = new Thickness(2,5,2,5);

            //    TextBlock txtVol = new TextBlock();
            //    txtVol.FontWeight = FontWeights.Bold;
            //    txtVol.Text = item.Value;
            //    stk.Children.Add(txtVol);
            //    stkOBD.Children.Add(stk);
            //    txtVol.Margin = new Thickness(2, 5, 2, 5);
            //}
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            if (CloseMe != null)
                CloseMe(this, new EventArgs());
        }
    }
}
