using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.Device
{
    /// <summary>
    /// Класс для хранения информации о разборе правил для устройств для классов, выполняющих обработку входящих сообщений от железа через TCP стек
    /// </summary>
    public class HDDeviceRules
    {
        /// <summary>
        /// Айди устройства
        /// </summary>
        public string deviceID { get; set; }

        /// <summary>
        /// Пароль к устройству
        /// </summary>
        public string devicePassword { get; set; }

        /// <summary>
        /// Правила разбора алгорима
        /// </summary>
        public int deviceParseAlgoritm { get; set; }

        /// <summary>
        /// Идентификатор, показывающий, нужно ли выполнять запрос пароля для устройства
        /// </summary>
        public bool neededPassword { get; set; }

        public bool HypertermoinalEnabled { get; set; }
    }
}
