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
using DTCDev.Models.DeviceSender.DISP;

namespace DTCDev.Client.Sensors.Analog.Min
{
    public partial class EngineOnControl : UserControl, ISensor
    {
        public EngineOnControl()
        {
            InitializeComponent();
        }

        DevicePresenter.Sensor _model;

        public DevicePresenter.Sensor Model
        {
            get { return _model; }

            set
            {
                _model = value;
            }
        }



        public string SensorName { get; set; }

        public int DataPosition { get; set; }

        public decimal Scale { get; set; }

        public decimal Offset { get; set; }

        private decimal _scale = 1;


        public void SetValue(bool v, int normal)
        {
        }

        public void SetValue(decimal min, decimal v, decimal max)
        {
            if (v > 0)
                elpOk.Visibility = Visibility.Visible;
            else
                elpOk.Visibility = Visibility.Collapsed;

        }
    }
}
