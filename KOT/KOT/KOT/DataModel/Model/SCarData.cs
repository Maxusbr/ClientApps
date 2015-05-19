using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace KOT.DataModel.Model
{
    [JsonObject]
    public class SCarData
    {
        private SNaviData _navigation;
        private DateTimeDataModel _dateUpdate;
        //Идентификатор контроллера
        [JsonProperty(PropertyName = "E")]
        public string ID { get; set; }

        //Данные о навигации
        [JsonProperty(PropertyName = "A")]
        public SNaviData Navigation
        {
            get { return _navigation; }
            set { _navigation = value; }
        }

        //Уровень сигнала сети
        [JsonProperty(PropertyName = "B")]
        public int GSM_Level { get; set; }

        //Напряжение питания
        [JsonProperty(PropertyName = "C")]
        public int PowerLevel { get; set; }

        //Уровень топлива
        [JsonProperty(PropertyName = "D")]
        public int FuelLevel { get; set; }

        //Температура контроллера
        [JsonProperty(PropertyName = "F")]
        public int Temperature { get; set; }

        //Дата получения последнего пакета от автомобиля
        [JsonProperty(PropertyName = "G")]
        public DateTimeDataModel DateUpdate
        {
            get { return _dateUpdate; }
            set { _dateUpdate = value; }
        }

        //Состояние аналоговых датчиков
        [JsonProperty(PropertyName = "I")]
        public List<int> Sensors { get; set; }

        //состояние дискретных датчиков
        [JsonProperty(PropertyName = "J")]
        public int DisByte { get; set; }


        //Ускорение 
        [JsonProperty(PropertyName = "K")]
        public int AcsX { get; set; }

        //Ускорение 
        [JsonProperty(PropertyName = "L")]
        public int AcsY { get; set; }

        //Ускорение 
        [JsonProperty(PropertyName = "M")]
        public int AcsZ { get; set; }

        //Максимальное ускорение
        [JsonProperty(PropertyName = "N")]
        public int AcsXMax { get; set; }

        //Максимальное ускорение
        [JsonProperty(PropertyName = "O")]
        public int AcsYMax { get; set; }

        //Максимальное ускорение
        [JsonProperty(PropertyName = "P")]
        public int AcsZMax { get; set; }
    }
}
