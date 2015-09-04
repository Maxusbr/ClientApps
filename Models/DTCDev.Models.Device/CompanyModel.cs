using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.Device
{
    /// <summary>
    /// Модель компании для привязки устройства
    /// </summary>
    public class CompanyModel
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string CUID { get; set; }
    }
}
