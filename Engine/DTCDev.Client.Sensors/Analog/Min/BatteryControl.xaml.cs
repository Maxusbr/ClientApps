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
    public partial class BatteryControl : UserControl, ISensor
    {
        AnalogMinViewModel amvm = new AnalogMinViewModel();
        public BatteryControl()
        {
            InitializeComponent();
            this.DataContext = amvm;
        }

        public BatteryControl(BatType bType)
        {
            InitializeComponent();
            _bType = bType;
            CalcRanges();
        }

        private BatType _bType = BatType.V12;

        public enum BatType
        {
            V12,
            V24
        }

        DevicePresenter.Sensor _model;

        public DevicePresenter.Sensor Model
        {
            get { return _model; }

            set
            {
                _model = value;
                amvm.Model = value;
            }
        }



        public string SensorName { get; set; }

        public int DataPosition { get; set; }

        public decimal Scale { get; set; }

        public decimal Offset { get; set; }




        private decimal _alarm = 10.5m;
        private decimal _warning = 11.0m;
        private decimal _good = 12.0m;
        private decimal _max = 15m;

        private void CalcRanges()
        {
            if (_bType == BatType.V24)
            {
                _alarm = 18;
                _warning = 20;
                _good = 24;
                _max = 28;
            }
        }
    }
}
