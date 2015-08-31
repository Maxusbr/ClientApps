using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Client.Cars.Engine.DisplayModels.CarModelHelper
{
    public class OutStateModel
    {
        public bool Out1 { get; set; }

        public bool Out2 { get; set; }

        public bool Out3 { get; set; }


        public void SetOuts(int o1, int o2, int o3)
        {
            if (o1 == 1)
                Out1 = true;
            else
                Out1 = false;

            if (o2 == 1)
                Out2 = true;
            else
                Out2 = false;

            if (o3 == 1)
                Out3 = true;
            else
                Out3 = false;
        }
    }
}
