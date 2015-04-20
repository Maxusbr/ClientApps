using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.Proxy
{
    public class ProxyCardsModel
    {
        public int id { get; set; }
        public int CompanyID { get; set; }

        private string _name = "";
        public string Name 
        { 
            get { return _name; } 
            set { _name = value; } 
        }

        private string _number = "";
        public string Number
        {
            get { return _number; }
            set { _number = value; }
        }
    }
}
