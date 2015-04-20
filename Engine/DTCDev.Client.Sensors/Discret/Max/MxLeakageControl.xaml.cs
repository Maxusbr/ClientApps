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

namespace DTCDev.Client.Sensors.Discret.Max
{
    public partial class MxLeakageControl : UserControl
    {
        public MxLeakageControl()
        {
            InitializeComponent();
        }



        private DiscretValuePresenter disPressenter = new DiscretValuePresenter();


        public DiscretValuePresenter DisPresenter
        {
            get { return disPressenter; }
            set { disPressenter = value; }
        }

        public DevicePresenter.Sensor Sensor
        {
            get;
            set;
        }

        public string SensorName { get; set; }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            disPressenter.SensorModel = Sensor;
            txtName.Text = SensorName;
            txtName1.Text = SensorName;
        }
    }
}
