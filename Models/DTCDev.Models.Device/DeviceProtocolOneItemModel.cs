using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.Device
{
    /// <summary>
    /// класс описания одног экземпляра данных в протоколе. На текущий момент не используется
    /// </summary>
    public class DeviceProtocolOneItemModel

        {
        public int id { get; set; }
        public int id_Protocol { get; set; }
        public string DataName { get; set; }
        public int Position { get; set; }
        public int DataType { get; set; }
        //NULL possible       
        public bool isInvert { get; set; }
        public string StringNormalValue { get; set; }
        public bool BoolNormalValue { get; set; }
        public int deviceId { get; set; }
        public int devicePosition { get; set; }       
        public string MinValue { get; set; }
        public string MaxValue { get; set; }

    }
    
}
