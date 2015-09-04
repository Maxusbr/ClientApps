using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.Device
{
   public class DeviceLinksModel
    {
       public int id { get; set; }
       public string DeviceGUID { get; set; }
       public string Name { get; set; }
       public int id_OwnerCompany { get; set; }
    }
}
