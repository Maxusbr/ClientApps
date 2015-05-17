using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace KOT.DataModel.Model
{
    [JsonObject]
    public class CarStateFullModel
    {
        private SCarData _state;
        private List<KVP> _obd;
        private List<KVP> _errors;
        //состояние автомобиля
        [JsonProperty(PropertyName = "A")]
        public SCarData State
        {
            get { return _state; }
            set { _state = value; }
        }

        //Вин-код автомобиля
        [JsonProperty(PropertyName = "B")]
        public string VIN { get; set; }

        //Параметры OBD II
        [JsonProperty(PropertyName = "C")]
        public List<KVP> OBD
        {
            get { return _obd; }
            set { _obd = value; }
        }

        //Ошибки автомобиля
        [JsonProperty(PropertyName = "D")]
        public List<KVP> Errors
        {
            get { return _errors; }
            set { _errors = value; }
        }
    }
}
