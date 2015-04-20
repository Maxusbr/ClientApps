using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.Car.ShiftModel
{
   public class ShiftModel
    {
       public int id { get; set; }
       public string Name { get; set; }
       public int  id_Company { get; set; }
       public int HStart { get; set; }
       public int MStart { get; set; }
       public int HStop { get; set; }
       public int MStop { get; set; }
       public int id_ShiftScheme { get; set; }
      

    }
}
