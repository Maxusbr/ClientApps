using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.Device.SpecData
{
    public class PowerDataModel
    {
        public string CounterID { get; set; } //айди счетчика
        public int CurrentPowerP { get; set; } //текущая мощность {get;set;} далее - мощность по фазам
        public int CurrentPowerP_A { get; set; }
        public int CurrentPowerP_B { get; set; }
        public int CurrentPowerP_C { get; set; }
        public int CurrentPowerS { get; set; } //S-мощность
        public int CurrentPowerS_A { get; set; }
        public int CurrentPowerS_B { get; set; }
        public int CurrentPowerS_C { get; set; }
        public int Current_A { get; set; } //ток фазы 1
        public int Current_B { get; set; }
        public int Current_C { get; set; }
        public int Volt { get; set; }//напряжение
        public int Volt_A { get; set; }//напряжение фазы 1
        public int Volt_B { get; set; }
        public int Volt_C { get; set; }
        public int Freq_A { get; set; }//частота фазы 1
        public int Freq_B { get; set; }
        public int Freq_C { get; set; }
        public int PowerKoef_A { get; set; }//Коэфициент мощности фазы 1
        public int PowerKoef_B { get; set; }
        public int PowerKoef_C { get; set; }
        public int FullCounter { get; set; } //общее показание счетчика

        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }

    }
}
