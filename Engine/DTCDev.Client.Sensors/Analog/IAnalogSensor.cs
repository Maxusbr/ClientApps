using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTCDev.Models.DeviceSender;
using DTCDev.Models.DeviceSender.DISP;

namespace DTCDev.Client.Sensors.Analog
{
    public interface IAnalogSensor
    {
        DevicePresenter.Sensor Sensor { get; set; }

        int DataPosition { get; set; }

        void UpdateValue(int ADC_Value);
    }
}
