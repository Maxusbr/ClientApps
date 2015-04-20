using DTCDev.Client.Cars.Engine.DisplayModels;
using DTCDev.Client.Cars.Engine.Handlers.Cars;
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
    /// Interaction logic for SettingsControllers.xaml
    /// </summary>
    public partial class SettingsControllers : UserControl
    {
        public SettingsControllers()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            lstCars.ItemsSource = CarsHandler.Instance.Cars;
        }

        DISP_Car _selectedCar;

        private void lstCars_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstCars.SelectedItem != null)
            {
                if (_selectedCar != null)
                    _selectedCar.PropertyChanged -= _selectedCar_PropertyChanged;
                _selectedCar = (DISP_Car)lstCars.SelectedItem;
                _selectedCar.PropertyChanged += _selectedCar_PropertyChanged;
                DisplayLines();
                UpdateFuel();
            }
        }

        private void DisplayLines()
        {
            stkLines.Children.Clear();
            if (_selectedCar == null)
            {
                stkLines.Children.Add(new TextBlock { Text = "Линии не подключены" });
                return;
            }
            if (_selectedCar.Data == null)
            {
                stkLines.Children.Add(new TextBlock { Text = "Линии не подключены" });
                return;
            }
            if (_selectedCar.Data.Sensors == null)
            {
                stkLines.Children.Add(new TextBlock { Text = "Линии не подключены" });
                return;
            }
            int id=1;
            foreach (var item in _selectedCar.Data.Sensors)
            {
                StackPanel sp = new StackPanel();
                sp.Background = new SolidColorBrush(Colors.White);
                sp.MouseLeftButtonDown += sp_MouseLeftButtonDown;
                sp.Orientation = Orientation.Horizontal;
                sp.Tag = id;
                TextBlock txt = new TextBlock();
                txt.Text = "Линия " + id.ToString();
                txt.Margin = new Thickness(5, 5, 20, 5);
                sp.Children.Add(txt);
                TextBlock txtVol = new TextBlock();
                txtVol.Text = item.ToString();
                txtVol.Margin = new Thickness(5);
                sp.Children.Add(txtVol);
                stkLines.Children.Add(sp);
                id++;
            }
        }

        private void UpdateFuel()
        {
            if (_selectedCar == null)
                return;
            else
            {
                if (_selectedCar.FuelDataPosition == -1)
                {
                    txtLine.Text = "Не указано";
                    txtValue.Text = "0000";
                    txtValue.Text = "0 л.";
                    imgChange.Opacity = 0.3f;
                }
                else
                {
                    txtLine.Text = "Линия " + (_selectedCar.FuelDataPosition + 1).ToString();
                    int currValue = _selectedCar.Data.Sensors[_selectedCar.FuelDataPosition];
                    txtValue.Text = currValue.ToString();
                    imgChange.Opacity = 1;
                    txtCalculated.Text = ((int)((decimal)(currValue - _selectedCar.StartFuelValue) / (_selectedCar.StepPerLiter))).ToString() + " л.";
                }
            }
        }

        Border b = new Border();

        void sp_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            b = new Border();
            int id = (int)((StackPanel)sender).Tag;
            sensorID = id - 1;
            b.Tag = id;
            b.Width = 200;
            b.Height = 34;
            b.Background = new SolidColorBrush(Colors.LightBlue);
            StackPanel sp = new StackPanel();
            b.Child = sp;
            sp.Orientation = Orientation.Horizontal;
            TextBlock txt = new TextBlock();
            txt.Text = "Линия " + id.ToString();
            txt.Margin = new Thickness(5, 5, 20, 5);
            sp.Children.Add(txt);
            TextBlock txtVol = new TextBlock();
            txtVol.Text = _selectedCar.Data.Sensors[id - 1].ToString();
            txtVol.Margin = new Thickness(5);
            sp.Children.Add(txtVol);

            cnvMove.Visibility = Visibility.Visible;
            cnvMove.CaptureMouse();
            cnvMove.MouseMove += cnvMove_MouseMove;
            cnvMove.MouseLeftButtonUp += cnvMove_MouseLeftButtonUp;
            Point p = e.GetPosition(cnvMove);
            cnvMove.Children.Add(b);
            Canvas.SetLeft(b, p.X+1);
            Canvas.SetTop(b, p.Y+1);
            brdrPlaceThis.Visibility = Visibility.Visible;
        }

        int sensorID = -1;

        void cnvMove_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(cnvMove);
            if (p.X > 0 && p.X < 52 && p.Y > 0 && p.Y < 52)
            {
                grdFuelSettings.Visibility = Visibility.Visible;
                txtCurrSensor.Text = _selectedCar.Data.Sensors[sensorID].ToString();
            }
            cnvMove.ReleaseMouseCapture(); 
            cnvMove.MouseMove -= cnvMove_MouseMove;
            cnvMove.MouseRightButtonUp -= cnvMove_MouseLeftButtonUp;
            cnvMove.Visibility = Visibility.Collapsed;
            cnvMove.Children.Clear();
            brdrPlaceThis.Visibility = Visibility.Collapsed;
            elpTarget.Visibility = Visibility.Collapsed;
        }

        void cnvMove_MouseMove(object sender, MouseEventArgs e)
        {
            Point p = e.GetPosition(cnvMove);
            Canvas.SetLeft(b, p.X+1);
            Canvas.SetTop(b, p.Y+1);
            if (p.X > 0 && p.X < 52 && p.Y > 0 && p.Y < 52)
                elpTarget.Visibility = Visibility.Visible;
            else
                elpTarget.Visibility = Visibility.Collapsed;
        }

        void _selectedCar_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Data")
            {
                DisplayLines();
                UpdateFuel();
                if (sensorID > -1 && _selectedCar.Data != null && _selectedCar != null && _selectedCar.Data.Sensors.Count() > sensorID)
                    txtCurrSensor.Text = _selectedCar.Data.Sensors[sensorID].ToString();
            }
        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (imgChange.Opacity == 1)
            {
                grdFuelSettings.Visibility = Visibility.Visible;
                minValue = _selectedCar.StartFuelValue;
                int currValue = _selectedCar.Data.Sensors[_selectedCar.FuelDataPosition];
                currentFuel = (int)((decimal)(currValue - _selectedCar.StartFuelValue) / (_selectedCar.StepPerLiter));

                UpdateFuelSettings();
            }
        }

        int fuelTank = 0;
        int currentFuel = 0;
        int minValue = 0;
        int maxValue = 0;
        decimal step = 0;


        private void txtTank_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateFuelSettings();
        }

        private void UpdateFuelSettings()
        {
            Int32.TryParse(txtTank.Text, out fuelTank);
            Int32.TryParse(txtMin.Text, out minValue);
            Int32.TryParse(txtMax.Text, out maxValue);
            Int32.TryParse(txtCurr.Text, out currentFuel);
            brdrMaxError.Visibility =
                brdrTankError.Visibility = Visibility.Collapsed;
            txtDispFullLit.Text = fuelTank.ToString() + " л.";
            txtdispEmpPop.Text = minValue.ToString();
            txtDispFullPop.Text = maxValue.ToString();

            if (fuelTank == 0)
            {
                brdrTankError.Visibility = Visibility.Visible;
                return;
            }

            int currentPop = _selectedCar.Data.Sensors[sensorID];

            if (fuelTank > 0 && minValue > 0)
            {
                step = currentPop - minValue;
                if (currentFuel == 0)
                    step = step / 0.1m;
                else
                    step = step / currentFuel;
                maxValue = (int)(fuelTank * step) + minValue;
            }
            else if (fuelTank > 0 && maxValue > 0 && currentFuel > 0)
            {
                decimal toFull = fuelTank - currentFuel;
                decimal points = maxValue - currentPop;
                step = points / toFull;
                minValue = (int)(currentPop - currentFuel * step);
            }

            txtDispFullLit.Text = fuelTank.ToString() + " л.";
            txtdispEmpPop.Text = minValue.ToString();
            txtDispFullPop.Text = maxValue.ToString();

            if (fuelTank > 0 && currentFuel > 0)
            {
                double perLiter = 140.0f / fuelTank;
                double nowLevel = currentFuel * perLiter;
                int height = (int)nowLevel;
                if (height > 140)
                    height = 140;
                if (height < 0)
                    height = 0;
                brdrCurrFuel.Height = height;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CarsHandler.Instance.SetFuelInfo(_selectedCar.Data.ID, sensorID, minValue, step);
            grdFuelSettings.Visibility = Visibility.Collapsed;
            UpdateFuel();
        }
    }
}
