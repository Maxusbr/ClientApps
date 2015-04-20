using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTCDev.Models.DeviceSender.DISP;

namespace DTCDev.Client.Sensors
{
    public interface ISensor
    {
        DevicePresenter.Sensor Model
        {
            get;
            set;
        }

        decimal Scale { get; set; }

        decimal Offset { get; set; }

        string SensorName { get; set; }

        int DataPosition { get; set; }
    }
}
