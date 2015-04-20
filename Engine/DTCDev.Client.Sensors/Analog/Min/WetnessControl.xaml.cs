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
using DTCDev.Models.DeviceSender;
using DTCDev.Models.DeviceSender.DISP;

namespace DTCDev.Client.Sensors.Analog.Min
{
    public partial class WetnessControl : UserControl,ISensor
    {
        public WetnessControl()
        {
            InitializeComponent();
            anPresenter.DrivedElement = elpRed;
        }


        private Analog.AnalogValuePresenter anPresenter = new Analog.AnalogValuePresenter();

        private DevicePresenter.Sensor _model = new DevicePresenter.Sensor();

        public DevicePresenter.Sensor Model
        {
            get { return _model; }
            set
            {
                _model = value;
                //Telemetry.Convertors.StringToDecimal std = new Convertors.StringToDecimal();
                //Scale = std.DecimalConvertor(_model.SensorScale);
                //Offset = std.DecimalConvertor(_model.StartValue);
            }
        }

        public int DataPosition { get; set; }

        public string SensorName { get; set; }


        public decimal Scale { get; set; }

        public decimal Offset { get; set; }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            anPresenter.DrivedElement = elpRed;
        }


        
        public void SetValue(bool v, int normal)
        {
        }

        public void SetValue(decimal min, decimal v, decimal max)
        {
            anPresenter.SetValue(min, v, max);
        }
        
    }
}
