using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace DTCDev.Client.Sensors.OBD
{
    public class OBDSensorDetector
    {
        public FrameworkElement GetControl(string pid, string value)
        {
            switch (pid)
            {
                case "04":
                    {
                        EngineLoad el = new EngineLoad();
                        el.Init("04", value, 0, 100, 0, 70);
                        return el;
                    }
                case "0C":
                    {
                        EngineRPM el = new EngineRPM();
                        el.Init("0C", value, 0, 10000, 0, 3500);
                        return el;
                    }
                case "11":
                    {
                        AccelerationSensor el = new AccelerationSensor();
                        el.Init("11", value, 0, 100, 0, 60);
                        return el;
                    }
                case "05":
                    {
                        EngineTemperature el = new EngineTemperature();
                        el.Init("05", value, 0, 150, 0, 110);
                        return el;
                    }
                case "2F":
                    {
                        FuelLevel el = new FuelLevel();
                        el.Init("2F", value, 0, 100, 0, 100);
                        return el;
                    }
            }
            return null;
        }
    }
}
