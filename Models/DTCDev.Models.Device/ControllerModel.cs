using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.Device
{
    /// <summary>
    /// Модель одного типа контроллера
    /// </summary>
    public class ControllerModel
    {
        public int id {get; set;}
        /// <summary>
        /// Название
        /// </summary>
        public string Name {get; set;}
        /// <summary>
        /// Описание контроллера
        /// </summary>
        public string Description {get; set;}
        /// <summary>
        /// префикс протокола для детектирования контроллера
        /// </summary>
        public string ProtocolPrefix {get; set;}
        /// <summary>
        /// Ссылка на картинку для отображения внешнего вида контроллера
        /// </summary>
        public string ImageURL {get; set;}
        /// <summary>
        /// Идентификатор алгоритма разбора сообщения
        /// </summary>
        public string ParserMessagesAlgoritmID {get; set;}

        public int DSI { get; set; }
    }
}
