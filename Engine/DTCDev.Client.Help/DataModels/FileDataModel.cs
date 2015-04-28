using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTCDev.Client.Help.DataModels
{
    public class FileDataModel
    {
        public string FileName { get; set; }

        private List<FileElement> _elements = new List<FileElement>();
        public List<FileElement> Elements
        {
            get { return _elements; }
            set { _elements = value; }
        }


        public class FileElement
        {

            public FileElement()
            {
                Text = "";
                ImageURL = "";
            }

            public string Text { get; set; }

            public string ImageURL { get; set; }

            public string MenuRow { get; set; }
        }
    }
}
