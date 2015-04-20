using DTCDev.Client.Sensors.OBD;
using DTCDev.Models.CarsSending.Car;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for OBDHistoryRow.xaml
    /// </summary>
    public partial class OBDHistoryRow : UserControl
    {
        public OBDHistoryRow()
        {
            InitializeComponent();
        }

        class DataModel : OBDHistoryDataModel.OBDParam
        {
            public int Seconds { get; set; }

            public DataModel(OBDHistoryDataModel.OBDParam item)
            {
                Item = item;
                TimeSpan ts = new TimeSpan(item.Time.H, item.Time.M, item.Time.S);
                Seconds = (int)ts.TotalSeconds;
            }

            public OBDHistoryDataModel.OBDParam Item { get; set; }
        }

        string _obdCode;
        List<DataModel> _displayParams = new List<DataModel>();
        DTCDev.Models.DeviceSender.DISP.DevicePresenter.Sensor _sensor;
        private double _currentWidth = 300;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            trDetector = new Thread(DetectCallRefresh);
            trDetector.Start();
            RefreshView();
        }

        Thread trDetector;
        private double _lastWidth = 0;

        private void DetectCallRefresh()
        {
            bool updated = true;
            while (this != null)
            {
                if ((int)this.ActualWidth != (int)_lastWidth)
                {
                    _lastWidth = this.ActualWidth;
                    updated = false;
                }
                else if(updated==false)
                {
                    _currentWidth = _lastWidth;
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        RefreshView();
                    }));
                    updated = true;
                }
                Thread.Sleep(200);
            }
        }

        public OBDHistoryRow(string obdCode, List<OBDHistoryDataModel.OBDParam> displayParams)
        {
            InitializeComponent();
            _obdCode = obdCode;
            for (int i = 0; i < displayParams.Count(); i++)
            {
                _displayParams.Add(new DataModel(displayParams[i]));
            }
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            RefreshView();
        }

        private void RefreshView()
        {
            if (_obdCode.Length < 1 || _displayParams.Count() < 1)
                return;
            Sensors.OBD.OBDSensorDetector detector = new Sensors.OBD.OBDSensorDetector();
            UIElement elm = detector.GetControl(_obdCode, _displayParams[0].Vol.ToString());
            if (elm == null)
                return;
            grdControl.Children.Add(elm);
            CreateGraph();
        }

        private void CreateGraph()
        {
            if (_currentWidth < 1)
                return;
            OBDSensorStatData sst = new OBDSensorStatData();
            _sensor = sst.GetOBDSensorModel(_obdCode);
            sst = null;
            PreCalc();
            cnvData.Children.Clear();
            cnvData.Width = _currentWidth;
            double step = _currentWidth / (24 * 60 * 60);

            int secondsInStep = 86400;
            double sStep = _currentWidth;
            while (sStep > 1)
            {
                secondsInStep = secondsInStep / 2;
                sStep = sStep / 2;
            }

            int currentSecond = 0;
            while (currentSecond < 86400)
            {
                List<DataModel> temp = _displayParams.Where(p => p.Seconds >= currentSecond && p.Seconds < currentSecond + secondsInStep).ToList();
                if (temp.Count() < 1)
                    AddBorder(step, currentSecond, secondsInStep, 0);
                else
                {
                    int vol = temp.Sum(p => p.Item.Vol) / temp.Count();
                    AddBorder(step, currentSecond, secondsInStep, vol);
                }
                currentSecond += secondsInStep;
            }
        }

        private void AddBorder(double step, int startSeconds, int widthSeconds, int vol)
        {
            decimal presVol = vol / scale;
            double height = (double)presVol *30;
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
            _min = _sensor.Model.NormalMin / 1000;
            if (_min > 0)
                _min = 0;
            _max = _sensor.Model.NormalMax / 1000;
            scale = _sensor.Model.Max / 1000.0m;
            startValue = _sensor.Model.Min / 1000.0m;
            txt0.Text = _min.ToString();
            txt1.Text = ((_max - _min) / 2).ToString();
            txt2.Text = _max.ToString();
        }
    }
}
