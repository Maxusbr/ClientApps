using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Threading;
using System.ComponentModel;
using System.Diagnostics;
using DTCDev.Models.DeviceSender.DISP;

namespace DTCDev.Client.Sensors.Analog.Max
{
    public partial class MxAnalogDefaultControl : UserControl, IAnalogSensor
    {
        public MxAnalogDefaultControl()
        {
            InitializeComponent();
        }

        private DevicePresenter.Sensor currentModel = new DevicePresenter.Sensor();

        private const int ADC_MAX = 4096;

        private decimal scale = 1;

        private decimal sValue = 0;

        private decimal max = 0;
        private decimal min = 0;

        private const double AnglePerTick = 0.06592;

        private double lastAngle = -45;






        public DevicePresenter.Sensor Sensor
        {
            get { return currentModel; }
            set
            {
                currentModel = value;
                FillSteps();
            }
        }

        public int DataPosition { get; set; }


        public decimal Scale { get; set; }

        public decimal Offset { get; set; }




        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            /// Проверить
            if (DesignerProperties.GetIsInDesignMode(this))
                return;
            tmrAlarmBlinker = new Timer(OnTimerUpdate);
            tmrAlarmBlinker.Change(500, 500);
            PreShowData();
            FillSteps();
            CalculateMinMax();
        }

        private void PreShowData()
        {
            if (currentModel != null)
            {
                //txtName.Text = currentModel.DisplayName;
                //txtMils.Text = currentModel.Measure;
            }
        }

        private void FillSteps()
        {
            //scale = decConvertor.DecimalConvertor(currentModel.SensorScale);
            //sValue = decConvertor.DecimalConvertor(currentModel.StartValue);
            //max = decConvertor.DecimalConvertor(currentModel.Max);
            //min = decConvertor.DecimalConvertor(currentModel.Min);
            //decimal allDiapasone = ADC_MAX / scale;
            //allDiapasone -= sValue;
            //decimal perStep = allDiapasone / 6;
            //txtVol1.Text = GetCurrentDisplayed(perStep, sValue, 0).ToString();
            //txtVol2.Text = GetCurrentDisplayed(perStep, sValue, 1).ToString();
            //txtVol3.Text = GetCurrentDisplayed(perStep, sValue, 2).ToString();
            //txtVol4.Text = GetCurrentDisplayed(perStep, sValue, 3).ToString();
            //txtVol5.Text = GetCurrentDisplayed(perStep, sValue, 4).ToString();
            //txtVol6.Text = GetCurrentDisplayed(perStep, sValue, 5).ToString();
            //txtVol7.Text = GetCurrentDisplayed(perStep, sValue, 6).ToString();
            
        }

        private int GetCurrentDisplayed(decimal perStep, decimal start, int position)
        {
            decimal now = start + (perStep * position);
            return (int)now;
        }

        private void CalculateMinMax()
        {
            
        }




        public void UpdateValue(int ADC_Value)
        {
            if (scale == 0)
            {
                Debug.Assert(scale != 0, "Fucking null");
                return;
            }
            decimal nowValue = ADC_Value / scale;
            nowValue = nowValue + sValue;
            CalculateAlarm(nowValue);
            txtValue.Text = Math.Round(nowValue, 2).ToString();

            //calculate angle
            double angle = AnglePerTick * ADC_Value;
            //set angle to zero value
            angle -= 45;

            //create animation
            grdSelectorPresenter.RenderTransform = new RotateTransform();
            DoubleAnimation da = new DoubleAnimation
            {
                From = lastAngle,
                To = angle, 
                Duration = new TimeSpan(0,0,1)
            };
            lastAngle = angle;
            Storyboard.SetTarget(da, grdSelectorPresenter.RenderTransform);
            Storyboard.SetTargetProperty(da, new PropertyPath(RotateTransform.AngleProperty));
            Storyboard sb = new Storyboard();
            sb.Children.Add(da);
            sb.Begin();
        }

        private void CalculateAlarm(decimal now)
        {
            if (now < min || now > max)
                alarmDetected = true;
            else
                alarmDetected = false;
        }







        private bool alarmDetected = false;

        private Timer tmrAlarmBlinker;

        private bool blinkerOn = false;

        private void OnTimerUpdate(object sender)
        {
            if (alarmDetected)
            {
                    /// проверить
                this.Dispatcher.BeginInvoke((Action)delegate()
                {
                    if (blinkerOn)
                        elpAlarm.Visibility = Visibility.Collapsed;
                    else
                        elpAlarm.Visibility = Visibility.Visible;
                    blinkerOn = !blinkerOn;
                });
            }
            else
            {
                if (blinkerOn)
                {
                    blinkerOn = false;
                    /// проверить
                    this.Dispatcher.BeginInvoke((Action)delegate()
                    {
                        elpAlarm.Visibility = Visibility.Collapsed;
                    });
                }
            }
        }
    }
}
