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
using System.Windows.Threading;
using DTCDev.Client.Cars.Engine.DisplayModels;
using DTCDev.Client.Cars.Engine.Handlers;
using DTCDev.Client.Sensors;
using DTCDev.Models.CarsSending.Car;
using DTCDev.Client.Sensors.OBD;
using DTCDev.Client.Cars.Engine.AppLogic;
using DTCDev.Models.DeviceSender.DISP;

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
                //DisplayAccelerometer((DISP_Car)sender);
                DisplaySensors((DISP_Car)sender);
            }
        }

        List<DevicePresenter.Sensor> _sensorsData = new List<DevicePresenter.Sensor>();

        private void DisplaySensors(DISP_Car car)
        {
            if(car!=null)
                if(car.ConnectedSensors!=null)
                    if(car.ConnectedSensors.Count()>0)
                    {
                        stkSensors.Children.Clear();
                        _sensorsData.Clear();
                        foreach (var item in car.ConnectedSensors)
                        {
                            SensorLocator sl = new SensorLocator();
                            DevicePresenter.Sensor sensor = new DevicePresenter.Sensor();
                            sensor.Model = new DTCDev.Models.DeviceSender.DeviceSensorsModel
                            {
                                id = item.Value.id,
                                IsAnalog = item.Value.isNumeric,
                                IsInput = 1,
                                NormalMax = Convert.ToInt32(item.Value.Max.Split('.')[0]),
                                NormalMin = Convert.ToInt32(item.Value.Min.Split('.')[0]),
                                Name = item.Value.Name,
                                Max = Convert.ToInt32(item.Value.Max.Split('.')[0]) * 5,
                                Min = Convert.ToInt32(item.Value.Min.Split('.')[0]) * 5,
                                Port = item.Key,
                                PrType = item.Value.PresentModel
                            };
                            sensor.State = new DTCDev.Models.DeviceSender.SensorState();
                            if (car.Data.Sensors != null)
                                if (car.Data.Sensors.Count() > item.Key)
                                    sensor.State.Vol = car.Data.Sensors[item.Key] + Convert.ToInt32(item.Value.StartValue.Split('.')[0]);

                            UserControl control = sl.GetSensor(SensorsTypeEnum.SensorsMode.MIN, sensor);
                            stkSensors.Children.Add(control);
                        }
                    }
        }

        private void DisplayState(DISP_Car car)
        {
            if (CarExemplar.Data == null)
                return;
            try
            {
                DateTime dt =CarExemplar.Data.DateUpdate.ToDateTime();//new DateTime(CarExemplar.Data.DateUpdate.Y, CarExemplar.Data.DateUpdate.M, CarExemplar.Data.DateUpdate.D, CarExemplar.Data.DateUpdate.hh, CarExemplar.Data.DateUpdate.mm, CarExemplar.Data.DateUpdate.ss);
                // TODO исправить серверное время
                TimeSpan ts = DateTime.UtcNow - dt;
                TimeSpan tsUtc = DateTime.Now - DateTime.UtcNow;
                imgConnect.Visibility = Visibility.Collapsed;
                imgOldConnect.Visibility = Visibility.Collapsed;
                imgNoConnect.Visibility = Visibility.Collapsed;


                txtLastEntered.Text = "Последний выход на связь "+(dt + tsUtc).ToString("dd.MM.yy HH.mm.ss");

                if ((ts.TotalMinutes > 10 && ts.TotalDays<60) || TCPConnection.Instance.IsConnected == false)
                {
                    imgOldConnect.Visibility = Visibility.Visible;
                    brdrConnection.Background = new SolidColorBrush(Colors.Orange);
                }
                else if (ts.TotalDays >= 60)
                {
                    imgNoConnect.Visibility = Visibility.Visible;
                    brdrConnection.Background = new SolidColorBrush(Colors.DarkGray);
                }
                else
                {
                    imgConnect.Visibility = Visibility.Visible;
                    brdrConnection.Background = new SolidColorBrush(Colors.Blue);

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
                if (CarExemplar.Data.Navigation.Sattelites < 5)
                {
                    imgSatOk.Visibility = Visibility.Collapsed;
                    imgSatErr.Visibility = Visibility.Visible;
                    brdrSat.Background = new SolidColorBrush(Colors.Orange);
                }
                else
                {
                    imgSatOk.Visibility = Visibility.Visible;
                    imgSatErr.Visibility = Visibility.Collapsed;
                    brdrSat.Background = new SolidColorBrush(Colors.Blue);
                }
                txtSatCount.Text = "Спутников - " + CarExemplar.Data.Navigation.Sattelites.ToString();

                DisplayFuel(car);
            }
            catch { }
        }

        int _lastFuel = 0;

        private void DisplayFuel(DISP_Car car)
        {
            if (car.FuelData.FuelDataPosition < 0)
            {
                if (car.OBD != null)
                {
                    if (car.OBD.Count() > 0)
                    {
                        DISP_Car.EOBDData obd = car.OBD.Where(p => p.Key == "2F").FirstOrDefault();
                        if (obd == null)
                            grdFuel.Visibility = Visibility.Collapsed;
                        else
                        {
                            grdFuel.Visibility = Visibility.Visible;
                            txtFuel.Text = obd.Value + "%";
                        }
                    }
                    else
                        grdFuel.Visibility = Visibility.Collapsed;
                }
                else
                    grdFuel.Visibility = Visibility.Collapsed;
            }
            else
            {
                grdFuel.Visibility = Visibility.Visible;
                if (car.Data.Sensors != null)
                {
                    if (car.Data.Sensors.Count() > car.FuelData.FuelDataPosition)
                    {
                        int vol = car.Data.Sensors[car.FuelData.FuelDataPosition];
                        vol = vol - car.FuelData.StartFuelValue;
                        vol = (int)(vol / car.FuelData.StepPerLiter);
                        if (vol != _lastFuel)
                        {
                            txtFuel.Text = vol.ToString() + " л";
                            _lastFuel = vol;
                        }
                    }
                    else
                        grdFuel.Visibility = Visibility.Collapsed;
                }
                else
                    grdFuel.Visibility = Visibility.Collapsed;
                //if (car.FuelData.FuelLevelValue < 1000)
                //    txtFuel.Text = car.FuelData.FuelLevelValue.ToString() + " л";
                //else
                //    txtFuel.Text = car.FuelData.FuelLevelValue.ToString();
            }
        }

        private void DisplayOBD(DISP_Car car)
        {
            if (car.OBD == null)
                return;
            stkOBDParams.Children.Clear();
            PIDConverter converter = new PIDConverter();
            bool errorfinded=false;
            if (car.OBD.Count() > 0)
            {
                foreach (var item in car.OBD)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(item.Value);
                    sb.Append(" - ");
                    sb.Append(converter.GetPidInfo(item.Key));
                    int min = converter.GetMinVol(item.Key);
                    int max = converter.GetMaxVol(item.Key);
                    int vol = 0;
                    Int32.TryParse(item.Value, out vol);
                    TextBlock text = new TextBlock();
                    text.Text = sb.ToString();
                    if (vol < min || vol > max)
                    {
                        errorfinded = true;
                        text.Foreground = new SolidColorBrush(Colors.DarkRed);
                        text.FontWeight = FontWeights.Bold;
                    }
                    stkOBDParams.Children.Add(text);
                }
                if (errorfinded)
                    brdrOBDStatus.Background = new SolidColorBrush(Colors.Orange);
                else
                    brdrOBDStatus.Background = new SolidColorBrush(Colors.Blue);
            }
            //stkOBDSensors.Children.Clear();
            //stkOBDSensors.Children.Add(new TextBlock { Text = "OBD", Margin = new Thickness(0, 5, 6, 5) });
            //OBDSensorDetector detector = new OBDSensorDetector();
            //int count = 0;
            //foreach (var item in car.OBD)
            //{
            //    UIElement elm = detector.GetControl(item.Key, item.Value);
            //    if (elm != null)
            //    {
            //        stkOBDSensors.Children.Add(elm);
            //        count++;
            //    }
            //}
            //stkOBDSensors.Width = count * 32;
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

        bool _displayedOBD = false;

        private void grdOBDStatus_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_displayedOBD)
            {
                grdObdPresenter.Height = 0;
            }
            else
            {
                grdObdPresenter.Height = 150;
            }
            _displayedOBD = !_displayedOBD;
        }



    }
}
