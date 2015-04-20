using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.Car.AbstractModels
{
    public abstract class AbstractCar
    {
        public string DateProduse { get; set; }
        public string DateBuy { get; set; }
        public int StartDistance { get; set; }
        public int CurrentDistance { get; set; }
    }
}
