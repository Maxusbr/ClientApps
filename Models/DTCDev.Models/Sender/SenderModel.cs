using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.Sender
{
    public class SenderModel
    {
        public SendingMessageType MType { get; set; }

        public string Message { get; set; }

        public string DevID { get; set; }
    }
}
