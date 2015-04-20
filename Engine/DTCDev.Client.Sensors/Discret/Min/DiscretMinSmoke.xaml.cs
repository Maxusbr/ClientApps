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

namespace DTCDev.Client.Sensors.Discret.Min
{
    /// <summary>
    /// Interaction logic for DiscretMinSmoke.xaml
    /// </summary>
    public partial class DiscretMinSmoke : UserControl, ISensor
    {
        public DiscretMinSmoke()
        {
            InitializeComponent();
        }




        private DiscretValuePresenter disPressenter = new DiscretValuePresenter();
        public DiscretValuePresenter DisPresenter
        {
            get { return disPressenter; }
            set { disPressenter = value; }
        }


        public DevicePresenter.Sensor Model
        {
            get;
            set;
        }


        public int DataPosition { get; set; }

        public string SensorName { get; set; }


        public decimal Scale { get; set; }

        public decimal Offset { get; set; }

        private void control_Loaded(object sender, RoutedEventArgs e)
        {
            disPressenter.SensorModel = Model;
        }
    }
}
