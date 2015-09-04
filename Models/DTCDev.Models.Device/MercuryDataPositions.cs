using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.Device
{
    /// <summary>
    /// Класс для фомирования последовательности следования данных в протоколе счетчика
    /// </summary>
    public class MercuryDataPositions
    {
        /// <summary>
        /// Перечень данных и их позиций в протоколе. Менять запрещено. За самовольное изменение - 
        /// отрыв яиц. СВН помнит все, блять!
        /// </summary>
        public enum MercPositionsEnum : int
        {
            CounterID = 0, //айди счетчика
            CurrentPowerP = 1, //текущая мощность, далее - мощность по фазам
            CurrentPowerP_A = 2,
            CurrentPowerP_B = 3,
            CurrentPowerP_C = 4,
            CurrentPowerS = 5, //S-мощность
            CurrentPowerS_A = 6,
            CurrentPowerS_B = 7,
            CurrentPowerS_C = 8,
            Current_A = 9, //ток фазы 1
            Current_B = 10,
            Current_C = 11,
            Volt = 12,//напряжение
            Volt_A = 13,//напряжение фазы 1
            Volt_B = 14,
            Volt_C = 15,
            Freq_A = 16,//частота фазы 1
            Freq_B = 17,
            Freq_C = 18,
            PowerKoef_A = 19,//Коэфициент мощности фазы 1
            PowerKoef_B = 20,
            PowerKoef_C = 21,
            FullCounter=22, //общее показание счетчика
            ConnectResult = 23 //результат опроса. 1 - удачно, 0 - неудачно, при этом остальные данные заполняются нулями
        }
    }
}
