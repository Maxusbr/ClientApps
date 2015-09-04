using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace DTCDev.Models.Device
{
    /// <summary>
    /// Класс описания одного сообщения, полученного от устройства
    /// </summary>
    public class DeviceMessage
    {
        /// <summary>
        /// идентификатор устройства
        /// </summary>
        public string DUID { get; set; }

        /// <summary>
        /// Строка сообщения
        /// </summary>
        public string MsgRow { get; set; }

        /// <summary>
        /// Дата получения
        /// </summary>
        public DateTime RecDate { get; set; }

        public string Prx { get; set; }

        /// <summary>
        /// OnLine
        /// </summary>
        public int OL { get; set; }
    }
}
