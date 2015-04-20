using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Client.Sensors.OBD
{
    public class OBDSensorStatData
    {
        public DTCDev.Models.DeviceSender.DISP.DevicePresenter.Sensor GetOBDSensorModel(string pid)
        {
            DTCDev.Models.DeviceSender.DISP.DevicePresenter.Sensor answer = new Models.DeviceSender.DISP.DevicePresenter.Sensor();

            switch (pid)
            {
                case "0C": //RPM
                    answer.Model = new Models.DeviceSender.DeviceSensorsModel
                    {
                        IsAnalog = 1,
                        IsInput = 1,
                        Max = 9000000,
                        Min = 0,
                        NormalMax = 5000000,
                        NormalMin = 0
                    };                                   
                    break;
                case "2F": //FUEL
                    answer.Model = new Models.DeviceSender.DeviceSensorsModel
                    {
                        IsAnalog = 1,
                        IsInput = 1,
                        Max = 100000,
                        Min = 0,
                        NormalMax = 100000,
                        NormalMin = 0
                    };    
                    break;
                case "04": //Engine load
                    answer.Model = new Models.DeviceSender.DeviceSensorsModel
                    {
                        IsAnalog = 1,
                        IsInput = 1,
                        Max = 100000,
                        Min = 0,
                        NormalMax = 70000,
                        NormalMin = 0
                    };    
                    break;
                case "05": //Temperature
                    answer.Model = new Models.DeviceSender.DeviceSensorsModel
                    {
                        IsAnalog = 1,
                        IsInput = 1,
                        Max = 130000,
                        Min = 0,
                        NormalMax = 110000,
                        NormalMin = 50000
                    };    
                    break;
                case "11": //Position
                    answer.Model = new Models.DeviceSender.DeviceSensorsModel
                    {
                        IsAnalog = 1,
                        IsInput = 1,
                        Max = 100000,
                        Min = 0,
                        NormalMax = 60000,
                        NormalMin = 0
                    };    
                    break;
                case "0D": //Speed
                    answer.Model = new Models.DeviceSender.DeviceSensorsModel
                    {
                        IsAnalog = 1,
                        IsInput = 1,
                        Max = 150000,
                        Min = 0,
                        NormalMax = 90000,
                        NormalMin = 0
                    };    
                    break;
            }

            return answer;
        }
    }
}
