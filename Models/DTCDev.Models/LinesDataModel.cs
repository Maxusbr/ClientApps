using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models
{
    public class LinesDataModel
    {
        public string DevID { get; set; }

        private List<LineRow> _rows = new List<LineRow>();

        public List<LineRow> Rows
        {
            get { return _rows; }
            set { _rows = value; }
        }

        public class LineRow
        {
            private List<int> _values = new List<int>();

            public List<int> Values
            {
                get { return _values; }
                set { _values = value; }
            }

            public Date.DateTimeDataModel DT { get; set; }
        }
    }
}
