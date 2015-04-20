using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.DeviceSender
{
    public class SensorFullModel
    {
        public int id { get; set; }
        /// <summary>
        /// Название датчика
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Отображаемое название
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// Является ли аналоговым
        /// </summary>
        public int isNumeric { get; set; }

        /// <summary>
        /// Модель представления (используется для визуализации в интерфейсе)
        /// </summary>
        public int PresentModel { get; set; }
        /// <summary>
        /// Масштаб АЦП
        /// </summary>
        public string SensorScale { get; set; }
        /// <summary>
        /// Еденица  измерения
        /// </summary>
        public string Measure { get; set; }
        /// <summary>
        /// ссылка на картинку
        /// </summary>
        public string InageURL { get; set; }
        //NULL possible
        /// <summary>
        /// минимум нормального значения
        /// </summary>
        public string Min { get; set; }
        /// <summary>
        /// Максимум нормального значения
        /// </summary>
        public string Max { get; set; }
        /// <summary>
        /// Нормальное значение для дискретной линии
        /// </summary>
        public int BoolNormalValue { get; set; }
        /// <summary>
        /// Начальное значение. Используется для указания нижней границы измерения датчика
        /// </summary>
        public string StartValue { get; set; }
        /// <summary>
        /// Является ли виртуальным
        /// </summary>
        public int isVirtual { get; set; }
        /// <summary>
        /// Является ли счетчиком
        /// </summary>
        public int isCounter { get; set; }

    }
}
