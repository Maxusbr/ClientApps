using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.Device
{
    public class DeviceSimpleModel
    {
        public int id { get; set; }
        /// <summary>
        /// Ссылка на контроллер
        /// </summary>
        public int IDController { get; set; }
        /// <summary>
        /// Пользовательское название устройства
        /// </summary>
        public string DeviceName { get; set; }
        /// <summary>
        /// Уникальный идентификатор устройства
        /// </summary>
        public string IDDevice { get; set; }
    }
}
