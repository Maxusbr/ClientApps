using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.Device.SpecData
{
    public class DeviceConnectionResultData
    {
        public string IDDevice { get; set; }
        public string StringConnection { get; set; }
        public int id { get; set; }
        public Nullable<bool> IsCounter { get; set; }
        public bool IsInput { get; set; }
        public bool IsNumber { get; set; }
        public int PortNomber { get; set; }
        public int PositionInProtocol { get; set; }
        public Nullable<bool> BoolNormalValue { get; set; }
        public string DisplayName { get; set; }
        public string InageURL { get; set; }
        public Nullable<bool> IsCounter1 { get; set; }
        public bool IsNumeric { get; set; }
        public Nullable<bool> IsVirtual { get; set; }
        public Nullable<decimal> Max { get; set; }
        public string Measure { get; set; }
        public Nullable<decimal> Min { get; set; }
        public string Name { get; set; }
        public int PresentModel { get; set; }
        public decimal SensorScale { get; set; }
        public Nullable<decimal> StartValue { get; set; }
    }
}
