using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace KOT.DataModel.Model
{
    [JsonObject]
    public class PointDetailsModel
    {
        public PointDetailsModel()
        {
            Comments = new List<CommentModel>();
            Price = new List<PriceModel>();
        }

        //Комментарии
        [JsonProperty(PropertyName = "A")]
        public List<CommentModel> Comments { get; set; }
        //прайс
        [JsonProperty(PropertyName = "B")]
        public List<PriceModel> Price { get; set; }

        [JsonObject]
        public class CommentModel
        {
            //дата комментария
            [JsonProperty(PropertyName = "C")]
            public DateDataModel Date { get; set; }
            //Балл, от 0 до 100
            [JsonProperty(PropertyName = "D")]
            public int Score { get; set; }
            //Текст
            [JsonProperty(PropertyName = "E")]
            public string Text { get; set; }
        }

        [JsonObject]
        public class PriceModel
        {
            //Название
            [JsonProperty(PropertyName = "F")]
            public string Name { get; set; }
            //Стоимость, в копейках
            [JsonProperty(PropertyName = "G")]
            public int Cost { get; set; }

            public override string ToString()
            {
                return string.Format("{0} - {1} руб.", Name, Cost);
            }
        }
    }

}
