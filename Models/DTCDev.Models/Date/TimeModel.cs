using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.Date
{
    public class TimeModel
    {
        public int H { get; set; }

        public int M { get; set; }

        public int S { get; set; }

        public override string ToString()
        {
            return string.Format("{0:00}:{1:00}:{2:00}", H, M, S);
        }

        public override bool Equals(Object obj)
        {
            TimeModel personObj = obj as TimeModel;
            if (personObj == null)
                return false;
            else
                return (personObj.H == this.H && personObj.M == this.M && personObj.S == this.S);
        }

        public override int GetHashCode()
        {
            return (this.H*360+this.M*60+this.S).GetHashCode();
        }
    }
}
