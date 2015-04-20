using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.Car.ShiftModel
{
   public class ScheduleModel
    {
       public int id { get; set; }
       public DateTime Date { get; set; }
       public int id_Driver { get; set; }
       public int id_Scheme { get; set; }
       public int id_Shift { get; set; }
       
    }
}
