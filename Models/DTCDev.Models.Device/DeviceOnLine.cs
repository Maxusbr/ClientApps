using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.Device
{
    /// <summary>
    /// Class for notify messages  online devices
    /// </summary>
    public class DeviceOnLine
    {
        public string IDDev { get; set; }

        public bool IsOnline { get; set; }
    }
}
