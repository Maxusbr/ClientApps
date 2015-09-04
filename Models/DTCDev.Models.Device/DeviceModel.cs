using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.Device
{
    /// <summary>
    /// модель одного устройства
    /// </summary>
    public class DeviceModel
    {
        public int id {get; set;}
        /// <summary>
        /// Ссылка на контроллер
        /// </summary>
        public int IDController {get; set;}
        /// <summary>
        /// Пользовательское название устройства
        /// </summary>
        public string DeviceName {get; set;}
        /// <summary>
        /// Уникальный идентификатор устройства
        /// </summary>
        public string IDDevice {get; set;}
        /// <summary>
        /// Пароль устройства
        /// </summary>
        public string DevicePassword {get; set;}//
        /// <summary>
        /// Урл картинки для устройства, если оно поддерживает подключение веб-камеры
        /// </summary>
        public string ImageWebCamUrl {get; set;}//
        /// <summary>
        /// Является ли составным (виртуальным) устройством
        /// </summary>
        public bool isComposite {get; set;}
        /// <summary>
        /// Требуется ли запись строк в БД (история)
        /// </summary>
        public bool WriteReceivedRowsToDB {get; set;}//
        /// <summary>
        /// Требуется ли пароль для устройства
        /// </summary>
        public bool DeviceNeedPassword {get; set;}
        /// <summary>
        /// Идентификатор икс координаты устройства в протоколе
        /// </summary>
        public float PositionX {get; set;}//
        /// <summary>
        /// Идентификатор игрек координаты устройства в протоколе
        /// </summary>
        public float PositionY { get; set; }//
        /// <summary>
        /// Ссылка на наличие специальных интерфейсов (типа RS485/CAN)
        /// </summary>
        public int id_SpecificInterfaces { get; set; }
        /// <summary>
        /// Флаг таймаута опроса устройства, сигнализирует о временных перерывах со связью, когда оператор не рвет соединение
        /// </summary>
        public bool Timeout { get; set; }

        public string DriverRow { get; set; }

        public string SMSNombers { get; set; }

        public List<int> cid { get; set; }

        public string CUID { get; set; }
    }
}
