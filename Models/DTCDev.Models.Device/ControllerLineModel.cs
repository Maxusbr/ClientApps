using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.Device
{
    /// <summary>
    /// Модель одной линии контроллера как типа. Используется для точного информирования, что это за ножка
    /// </summary>
    public class ControllerLineModel
    {
        public int id { get; set; }
        /// <summary>
        /// Ссылка на контроллер
        /// </summary>
        public int id_Controller { get; set; }
        /// <summary>
        /// номер порта (ножки)
        /// </summary>
        public int PortNumber { get; set; }
        /// <summary>
        /// Позиция в протоколе. Информация о том, где находится представление именно этой ножки
        /// </summary>
        public int PostionInProtocol { get; set; }
        /// <summary>
        /// Является ли числовым. Если фолс - то дискретный
        /// </summary>
        public bool isNumber { get; set; }
        /// <summary>
        /// Является ли входом. если нет - то управляющий выход
        /// </summary>
        public bool isInput { get; set; }
        /// <summary>
        /// Является ли счетчиком. Если это счетчик, то данные плюсуются (например, текущий расход эл-ва)
        /// </summary>
        public bool isCounter { get; set; }
    }
}
