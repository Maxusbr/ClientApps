using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace DTCDev.Models.CarBase.Driver
{
    [JsonObject]
    public class DriverDataModel
    {
        [JsonProperty(PropertyName = "0")]
        public int ID { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        [JsonProperty (PropertyName = "A")]
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        [JsonProperty(PropertyName = "B")]
        public string SecondName { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        [JsonProperty(PropertyName = "C")]
        public string FamilyName { get; set; }

        /// <summary>
        /// Номер телефона
        /// </summary>
        [JsonProperty(PropertyName = "D")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Категоря А. 0 - нет, 1 - есть
        /// </summary>
        [JsonProperty(PropertyName = "E")]
        public int CatA { get; set; }

        /// <summary>
        /// Категоря B. 0 - нет, 1 - есть
        /// </summary>
        [JsonProperty(PropertyName = "F")]
        public int CatB { get; set; }

        /// <summary>
        /// Категоря C. 0 - нет, 1 - есть
        /// </summary>
        [JsonProperty(PropertyName = "G")]
        public int CatC { get; set; }

        /// <summary>
        /// Категоря D. 0 - нет, 1 - есть
        /// </summary>
        [JsonProperty(PropertyName = "H")]
        public int CatD { get; set; }

        /// <summary>
        /// Категоря E. 0 - нет, 1 - есть
        /// </summary>
        [JsonProperty(PropertyName = "I")]
        public int CatE { get; set; }
    }
}
