using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.Device.SpecData
{
    public class DeviceParsingModel
    {
        public string DID { get; set; }

        public int ControllerID { get; set; }

        public List<DeviceConnectionResultData> Rules { get; set; }
    }
}
