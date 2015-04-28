using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTCDev.Client.Help.DataModels
{
    public class HelpMenuModel
    {
        public string Header { get; set; }

        public string File { get; set; }

        public Guid ID { get; set; }

        private List<HelpMenuModel> _children = new List<HelpMenuModel>();
        public List<HelpMenuModel> Children
        {
            get { return _children; }
            set { _children = value; }
        }
    }
}
