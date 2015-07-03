using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.User
{
    public class UserLightModel
    {
        public int ID { get; set; }

        public string Nm { get; set; }

        public string Ph { get; set; }

        public string Em { get; set; }

        /// <summary>
        /// тип того, кто записывается на ТО
        /// </summary>
        public int Tp { get; set; }
    }
}
