using DTCDev.Client.Sensors;
using DTCDev.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DTCDev.Client.Cars.Controls.Controls.History
{
    /// <summary>
    /// Interaction logic for ControllerLineHistoryRow.xaml
    /// </summary>
    public partial class ControllerLineHistoryRow : UserControl
    {
        public ControllerLineHistoryRow()
        {
            InitializeComponent();
        }

        DTCDev.Models.DeviceSender.DISP.DevicePresenter.Sensor _sensor;
        LinesDataModel _model;
        private double _currentWidth = 300;

        public ControllerLineHistoryRow(DTCDev.Models.DeviceSender.DISP.DevicePresenter.Sensor sensor, 
            LinesDataModel model)
        {
            InitializeComponent();
            _model = model;
            _sensor = sensor;
            if (_model != null)
            {
                if (_model.Rows.Count() > 0)
                    DisplayParameter(0);
                DisplayGraph();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            trDetector = new Thread(DetectCallRefresh);
            trDetector.Start();
        }

        Thread trDetector;
        private double _lastWidth = 0;

        private void DetectCallRefresh()
        {
            while (this != null)
            {
                if (this.ActualWidth != _lastWidth)
                {
                    _lastWidth = this.ActualWidth;
                }
                else
                {
                    _currentWidth = _lastWidth;
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        DisplayGraph();
                    }));
                }
                Thread.Sleep(200);
            }
        }

        private void DisplayParameter(int id)
        {
            grdControl.Children.Clear();
            SensorLocator locator = new SensorLocator();
            //_sensor.State.Vol = _model.Rows[id].Values[_sensor.Model.Port - 1];
            DTCDev.Models.DeviceSender.DISP.DevicePresenter.Sensor nSens = new DTCDev.Models.DeviceSender.DISP.DevicePresenter.Sensor
            {
                Model = new DTCDev.Models.DeviceSender.DeviceSensorsModel
                {
                    id = _sensor.Model.id,
                    IsAnalog = _sensor.Model.IsAnalog,
                    IsInput = _sensor.Model.IsInput,
                    Max = _sensor.Model.Max,
                    Min = _sensor.Model.Min,
                    Name = _sensor.Model.Name,
                    NormalMax = _sensor.Model.NormalMax,
                    NormalMin = _sensor.Model.NormalMin,
                    Port = _sensor.Model.Port,
                    PrType = _sensor.Model.PrType
                },
                State = new DTCDev.Models.DeviceSender.SensorState
                {
                    id = 0,
                    Vol = _model.Rows[id].Values[_sensor.Model.Port - 1]
                }
            };

            UserControl control = locator.GetSensor(SensorsTypeEnum.SensorsMode.HIST, nSens);
            if (control != null)
                grdControl.Children.Add(control);
        }

        private void DisplayGraph()
        {
            if (_currentWidth < 1)
                return;
            PreCalc();
            cnvData.Children.Clear();
            cnvData.Width = _currentWidth;
            double step = _currentWidth / (24 * 60 * 60);


            TimeSpan ts = new TimeSpan(0);
            if (_model == null)
            {
                LayoutRoot.Children.Add(new Border
                {
                    Background = new SolidColorBrush(Colors.White)
                });
                return;
            }
            if (_model.Rows.Count() < 1)
            {
                LayoutRoot.Children.Add(new Border
                {
                    Background = new SolidColorBrush(Colors.White)
                });
                return;
            }
            foreach (var item in _model.Rows)
            {
                TimeSpan tsCurrent = new TimeSpan(item.DT.hh, item.DT.mm, item.DT.ss);
                TimeSpan delta = tsCurrent - ts;
                if (delta.TotalSeconds > 300)
                {
                    AddBorder(step, (int)ts.TotalSeconds, (int)delta.TotalSeconds, 0);
                }
                else
                {
                    AddBorder(step, (int)ts.TotalSeconds, (int)delta.TotalSeconds, item.Values[_sensor.Model.Port-1]);
                }
                ts = tsCurrent;
            }
            AddBorder(step, (int)ts.TotalSeconds, (int)(new TimeSpan(1, 0, 0, 0).TotalSeconds), 0);
        }

        int _normalMin;
        int _normalMax;

        private void AddBorder(double step, int startSeconds, int widthSeconds, int vol)
        {
            decimal presVol = vol / scale;
            //if (vol > 0)
            //{
            //    int i = 0;
            //    i = vol;
            //    txt0.Text = i.ToString();
            //}
            double height = (double)presVol / (_max - _min);
            height = height * 30;
            height = Math.Round(height) + 1;
            Border b = new Border();
            b.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
            if (presVol < _min || presVol > _max)
                b.Background = new SolidColorBrush(Colors.Red);
            else
                b.Background = new SolidColorBrush(Colors.Blue);
            b.Height = (int)height;
            double width = widthSeconds * step;
            if (width < 1)
                width = 1;
            b.Width = width;
            cnvData.Children.Add(b);
        }

        int _min;
        int _max;
        decimal scale;
        decimal startValue;

        private void PreCalc()
        {
            _min = _sensor.Model.NormalMin/1000;
            if (_min > 0)
                _min = 0;
            _max = _sensor.Model.NormalMax/1000;
            scale = _sensor.Model.Max / 1000.0m;
            startValue = _sensor.Model.Min / 1000.0m;
            txt0.Text = _min.ToString();
            txt1.Text = ((_max - _min) / 2).ToString();
            txt2.Text = _max.ToString();
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _currentWidth = this.ActualWidth;
        }
    }
}
