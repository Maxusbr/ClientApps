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
using DTCDev.Client.Cars.Engine.DisplayModels;
using DTCDev.Client.Cars.Engine.Handlers;
using DTCDev.Client.Sensors;
using DTCDev.Models.CarsSending.Car;
using DTCDev.Client.Sensors.OBD;
using DTCDev.Client.Cars.Engine.AppLogic;

namespace DTCDev.Client.Cars.Controls.Controls.Car
{
    /// <summary>
    /// Interaction logic for CarViewElement.xaml
    /// </summary>
    public partial class CarViewElement : UserControl
    {
        public CarViewElement()
        {
            InitializeComponent();
        }

        public DISP_Car CarExemplar
        {
            get { return (DISP_Car)GetValue(CarProperty); }
            set { SetValue(CarProperty, value); }
        }

        private static DependencyProperty CarProperty = DependencyProperty.Register("CarExemplar",
            typeof(DISP_Car),
            typeof(CarViewElement), 
            new PropertyMetadata(new DISP_Car(), OnCarChanged));

        private static void OnCarChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            CarViewElement cve = (CarViewElement)sender;
            DISP_Car nValue = (DISP_Car)e.NewValue;
            DISP_Car oValue = (DISP_Car)e.OldValue;
            nValue.PropertyChanged += cve.nValue_PropertyChanged;
            oValue.PropertyChanged -= cve.nValue_PropertyChanged;
            if (nValue.Device != null)
                nValue.Device.PropertyChanged += cve.Device_PropertyChanged;
            if (oValue.Device != null)
                oValue.Device.PropertyChanged -= cve.Device_PropertyChanged;
            cve.DisplaySensors(nValue);
            cve.DisplayState(nValue);
        }

        void Device_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Sensors")
            {
                DisplayState(CarExemplar);
            }
        }

        void nValue_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Data")
            {
                DisplayState((DISP_Car)sender);
                DisplayOBD((DISP_Car)sender);
                DisplayAccelerometer((DISP_Car)sender);
                DisplaySensors((DISP_Car)sender);
            }
        }

        private void DisplaySensors(DISP_Car car)
        {
            stkSensors.Children.Clear();
            SensorLocator locator = new SensorLocator();
            foreach (var item in car.Device.Sensors)
            {
                UserControl control = locator.GetSensor(SensorsTypeEnum.SensorsMode.MIN, item);
                if (control != null)
                    stkSensors.Children.Add(control);
            }
        }

        private void DisplayState(DISP_Car car)
        {
            if (CarExemplar.Data == null)
                return;
            try
            {
                DateTime dt = new DateTime(CarExemplar.Data.DateUpdate.Y, CarExemplar.Data.DateUpdate.M, CarExemplar.Data.DateUpdate.D, CarExemplar.Data.DateUpdate.hh, CarExemplar.Data.DateUpdate.mm, CarExemplar.Data.DateUpdate.ss);
                // TODO исправить серверное время
                TimeSpan ts = DateTime.UtcNow - dt;
                TimeSpan tsUtc = DateTime.Now - DateTime.UtcNow;
                imgConnect.Visibility = Visibility.Collapsed;
                imgOldConnect.Visibility = Visibility.Collapsed;
                imgNoConnect.Visibility = Visibility.Collapsed;

                grdTime.ToolTip = (dt + tsUtc).ToString("dd.MM.yy HH.mm.ss");

                if ((ts.TotalMinutes > 10 && ts.TotalDays<60) || TCPConnection.Instance.IsConnected == false)
                {
                    imgOldConnect.Visibility = Visibility.Visible;
                }
                else if (ts.TotalDays >= 60)
                {
                    imgNoConnect.Visibility = Visibility.Visible;
                }
                else
                {
                    imgConnect.Visibility = Visibility.Visible;

                }
                txtSpeed.Text = (CarExemplar.Data.Navigation.Speed/10).ToString();
                if (CarExemplar.Data.Navigation.Speed < 1000)
                {
                    imgSpeedErr.Visibility = Visibility.Collapsed;
                    imgSpeedOk.Visibility = Visibility.Visible;
                }
                else
                {
                    imgSpeedOk.Visibility = Visibility.Collapsed;
                    imgSpeedErr.Visibility = Visibility.Visible;
                }
                if (CarExemplar.Data.Navigation.Sattelites < 5 || ts.TotalMinutes>10)
                {
                    imgSatOk.Visibility = Visibility.Collapsed;
                    imgSatErr.Visibility = Visibility.Visible;
                }
                else
                {
                    imgSatOk.Visibility = Visibility.Visible;
                    imgSatErr.Visibility = Visibility.Collapsed;
                }
                DetectConnection(ts, car);

                DisplayFuel(car);
            }
            catch { }
        }

        private void DisplayFuel(DISP_Car car)
        {
            if (car.FuelData.FuelDataPosition < 0)
            {
                grdFuel.Visibility = Visibility.Collapsed;
            }
            else
            {
                grdFuel.Visibility = Visibility.Visible;
                if (car.FuelData.FuelLevelValue < 1000)
                    txtFuel.Text = car.FuelData.FuelLevelValue.ToString() + " л";
                else
                    txtFuel.Text = car.FuelData.FuelLevelValue.ToString();
            }
        }

        private void DetectConnection(TimeSpan timeout, DISP_Car car)
        {
            if (timeout.TotalMinutes > 10)
            {
                imgConnection1.Visibility = Visibility.Collapsed;
                imgConnection2.Visibility = Visibility.Collapsed;
                imgConnection3.Visibility = Visibility.Collapsed;
                imgConnectionOff.Visibility = Visibility.Visible;
            }
            else
            {
                imgConnection1.Visibility = Visibility.Collapsed;
                imgConnection2.Visibility = Visibility.Collapsed;
                imgConnection3.Visibility = Visibility.Collapsed;
                imgConnectionOff.Visibility = Visibility.Collapsed;
                if (car.Data.GSM_Level < 30)
                    imgConnection1.Visibility = Visibility.Visible;
                else if (car.Data.GSM_Level >= 30 && car.Data.GSM_Level < 60)
                    imgConnection2.Visibility = Visibility.Visible;
                else
                    imgConnection3.Visibility = Visibility.Visible;
            }
        }

        private void DisplayOBD(DISP_Car car)
        {
            stkOBDSensors.Children.Clear();
            stkOBDSensors.Children.Add(new TextBlock { Text = "OBD", Margin = new Thickness(0, 5, 6, 5) });
            OBDSensorDetector detector = new OBDSensorDetector();
            int count = 0;
            foreach (var item in car.OBD)
            {
                UIElement elm = detector.GetControl(item.Key, item.Value);
                if (elm != null)
                {
                    stkOBDSensors.Children.Add(elm);
                    count++;
                }
            }
            stkOBDSensors.Width = count * 32;
        }

        private void DisplayAccelerometer(DISP_Car car)
        {
            if (car.Data.AcsX == 0 && car.Data.AcsXMax == 0 && car.Data.AcsY == 0
                && car.Data.AcsYMax == 0 && car.Data.AcsZ == 0
                && car.Data.AcsZMax == 0)
            {
                grdAccelerometer.Width = 0;
            }
            else
            {
                grdAccelerometer.Width = 32;
                brdrCentNotConnect.Visibility = Visibility.Collapsed;
                brdrMaxNotConnect.Visibility = Visibility.Collapsed;

                if (car.Data.AcsX > 50 && car.Data.AcsY > 50 && car.Data.AcsZ > 50)
                    brdrCentBad.Visibility = Visibility.Visible;
                else
                    brdrCentBad.Visibility = Visibility.Collapsed;

                if (car.Data.AcsXMax > 50 && car.Data.AcsYMax > 50 && car.Data.AcsZMax > 50)
                    brdrMaxBad.Visibility = Visibility.Visible;
                else
                    brdrMaxBad.Visibility = Visibility.Collapsed;

                int maxCent = 0;
                int maxMax = 0;

                if (maxCent < car.Data.AcsX)
                    maxCent = car.Data.AcsX;
                if (maxCent < car.Data.AcsY)
                    maxCent = car.Data.AcsY;
                if (maxCent < car.Data.AcsZ)
                    maxCent = car.Data.AcsZ;

                if (maxMax < car.Data.AcsXMax)
                    maxMax = car.Data.AcsXMax;
                if (maxMax < car.Data.AcsYMax)
                    maxMax = car.Data.AcsYMax;
                if (maxMax < car.Data.AcsZMax)
                    maxMax = car.Data.AcsZMax;

                maxCent = maxCent / 2;
                maxMax = maxMax / 2;
                if (maxCent < 3)
                    maxCent = 3;

                if (maxMax < 3)
                    maxMax = 3;

                if (maxCent > 32)
                    maxCent = 32;
                if (maxMax > 32)
                    maxMax = 32;
                grdGoodCent.Height = maxCent;
                grdGood.Height = maxMax;

                StackPanel sp = new StackPanel();
                sp.Background = new SolidColorBrush(Colors.Gray);
                grdAccelerometer.ToolTip = sp;
                TextBlock txt = new TextBlock { Text = "Ускорение Х = " + car.Data.AcsX.ToString(), Margin = new Thickness(5) };
                sp.Children.Add(txt);
                TextBlock txt1 = new TextBlock { Text = "Ускорение Y = " + car.Data.AcsY.ToString(), Margin = new Thickness(5) };
                sp.Children.Add(txt1);
                TextBlock txt2 = new TextBlock { Text = "Ускорение Z = " + car.Data.AcsZ.ToString(), Margin = new Thickness(5) };
                sp.Children.Add(txt2);
                TextBlock txt3 = new TextBlock { Text = "Максимальное ускорение Х = " + car.Data.AcsXMax.ToString(), Margin = new Thickness(5) };
                sp.Children.Add(txt3);
                TextBlock txt4 = new TextBlock { Text = "Максимальное ускорение Y = " + car.Data.AcsXMax.ToString(), Margin = new Thickness(5) };
                sp.Children.Add(txt4);
                TextBlock txt5 = new TextBlock { Text = "Максимальное ускорение Z = " + car.Data.AcsXMax.ToString(), Margin = new Thickness(5) };
                sp.Children.Add(txt5);
            }
        }


        public event EventHandler DisplayDetails;

        private void image1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            CarSelector.ViewCarDetailsCar = this.CarExemplar;
        }



    }
}
