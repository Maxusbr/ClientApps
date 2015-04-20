using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.Car.CarInfoModels
{
    public class WorkModel
    {
        public string WorkName { get; set; }

        public string WorkType { get; set; }

        public string WorkTypeEnum { get; set; }

        public string CarGUID { get; set; }

        public int idWork { get; set; }

        public int PeriodicMonth { get; set; }

        public int PeriodicDistance { get; set; }

        public string WorkGUID { get; set; }
    }
}
