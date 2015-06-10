using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace KOT.DataModel.Model
{
    [JsonObject]
    public class CarListWorkDataModel: CarListBaseDataModel
    {
        //Идентификатор устройства, его серийный номер. На основе него в дальнейшем ведется вся обработка входящих сообщений
        [JsonProperty(PropertyName = "P")]
        public WorkType[] WorkTypes { get; set; }

        [JsonProperty(PropertyName = "R")]
        public string NameUser { get; set; }

        [JsonProperty(PropertyName = "S")]
        public string Phone { get; set; }
    }

}
