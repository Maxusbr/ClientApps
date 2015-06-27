using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace DTCDev.Models.Service
{
    [JsonObject]
    public class EmployeeModel
    {
        public EmployeeModel()
        {
            FunctionsAccess = new List<int>();
            DepartmentsIDs = new List<int>();
        }

        [JsonProperty(PropertyName = "a")]
        public int id { get; set; }

        [JsonProperty(PropertyName = "b")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "c")]
        public string Mail { get; set; }

        [JsonProperty(PropertyName = "d")]
        public string Phone { get; set; }

        [JsonProperty(PropertyName = "e")]
        public string Login { get; set; }

        [JsonProperty(PropertyName = "f")]
        public string Password { get; set; }

        [JsonProperty(PropertyName = "g")]
        public List<int> DepartmentsIDs { get; set; }

        [JsonProperty(PropertyName = "h")]
        public List<int> FunctionsAccess { get; set; }
    }
}
