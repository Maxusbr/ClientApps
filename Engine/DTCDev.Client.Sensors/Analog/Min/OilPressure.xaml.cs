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
using DTCDev.Models.DeviceSender.DISP;

namespace DTCDev.Client.Sensors.Analog.Min
{
    /// <summary>
    /// Interaction logic for OilPressure.xaml
    /// </summary>
    public partial class OilPressure : UserControl, ISensor
    {
        AnalogMinViewModel amvm = new AnalogMinViewModel();


        private DevicePresenter.Sensor _model = new DevicePresenter.Sensor();

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


        public OilPressure(SensorsTypeEnum.SensorsMode mode = SensorsTypeEnum.SensorsMode.MIN)
        {
            InitializeComponent();
            this.DataContext = amvm;
            amvm.DisplayMode = mode;
        }
    }
}
