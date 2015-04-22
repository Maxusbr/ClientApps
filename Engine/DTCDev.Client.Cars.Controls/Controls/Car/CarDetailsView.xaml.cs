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
            CarSelector.OnCarChanged += CarSelector_OnCarChanged;
            if(_viewModel == null)
                _viewModel = new CarDetailsViewModel();
            this.DataContext = _viewModel;
        }

        void CarSelector_OnCarChanged(DISP_Car car)
        {
            _currentCar.PropertyChanged -= car_PropertyChanged;
            _currentCar = car;
            _currentCar.PropertyChanged += car_PropertyChanged;
        }

        DISP_Car _currentCar;
        int _lastAngle = -30;
        private CarDetailsViewModel _viewModel;


        public CarDetailsView(DISP_Car car)
        {
            InitializeComponent();
            _currentCar = car;
            _currentCar.PropertyChanged += car_PropertyChanged;
            if (_viewModel == null)
                _viewModel = new CarDetailsViewModel();
            _viewModel.Car = car;
            this.DataContext = _viewModel;
            _viewModel.MapCenter = _viewModel.MapCenterUser = _currentCar.Location;
        }

        void car_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Data")
            {
                UpdateData();
            }

        }

        private void UpdateData()
        {
            int angle = _currentCar.Data.Navigation.Speed / 10;
            angle = angle - 30;
            angRotate.Angle = angle;
            _lastAngle = angle;
            _viewModel.MapCenter = _viewModel.MapCenterUser = _currentCar.Location;
            if (_currentCar.OBD != null)
            {
                DISP_Car.EOBDData _rpmOBD = _currentCar.OBD.Where(p => p.Key == "0C").FirstOrDefault();
                if (_rpmOBD != null)
                {
                    int i=0;
                    Int32.TryParse(_rpmOBD.Value, out i);
                    double rpmAngle = i * 0.03f;
                    angRPM.Angle = rpmAngle;
                }
            }
            brdrSatMax.Visibility = brdrSatMid.Visibility = brdrSatMin.Visibility = Visibility.Collapsed;
            if (_currentCar.Data.Navigation.Sattelites < 5)
            {
                brdrSatMin.Visibility = Visibility.Visible;
            }
            else if (_currentCar.Data.Navigation.Sattelites >= 5 && _currentCar.Data.Navigation.Sattelites < 10)
            {
                brdrSatMid.Visibility = brdrSatMin.Visibility = Visibility.Visible;
            }
            else
            {
                brdrSatMax.Visibility = brdrSatMid.Visibility = brdrSatMin.Visibility = Visibility.Visible;
            }
        }

        private void CreateAnimation(int angle)
        {
            DoubleAnimation dbAscending = new DoubleAnimation((double)_lastAngle, (double)angle, new Duration(TimeSpan.FromMilliseconds(300)));
            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(dbAscending);
            Storyboard.SetTarget(dbAscending, brdrSpeed);
            Storyboard.SetTargetProperty(dbAscending, new PropertyPath(RotateTransform.AngleProperty));
            storyboard.Begin();
        }
    }
}
