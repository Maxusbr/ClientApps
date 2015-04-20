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

namespace DTCDev.Client.Sensors.Discret.Min
{
    public partial class DiscretMinLock : UserControl, ISensor
    {
        public DiscretMinLock()
        {
            InitializeComponent();
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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
        public void SetValue(bool v, int normal)
        {
            if (v == true)
            {
                imgLock.Visibility = Visibility.Visible;
                imgUnlock.Visibility = Visibility.Collapsed;
            }
            else
            {
                imgLock.Visibility = Visibility.Collapsed;
                imgUnlock.Visibility = Visibility.Visible;
            }
        }

        public void SetValue(decimal min, decimal v, decimal max)
        {

        }
    }
}
