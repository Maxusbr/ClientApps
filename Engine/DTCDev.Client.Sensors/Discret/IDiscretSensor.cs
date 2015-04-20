using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using DTCDev.Models.DeviceSender.DISP;

namespace DTCDev.Client.Sensors.Discret
{
    public interface IDiscretSensor
    {
        DevicePresenter.Sensor Sensor { get; set; }

        int DataPosition { get; set; }

        void UpdateValue(string data);

        Discret.DiscretValuePresenter DisPressenter
        {
            get;
        }

        string SensorName { get; set; }
    }
}
