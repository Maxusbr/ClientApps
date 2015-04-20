using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.CarsSending.Sending
{
    public class SendingClientsEnum
    {
        public enum SendModel
        {
            From,
            After,
            ByMark
        }

        public enum SendType
        {
            Email,
            SMS,
            Programm
        }
    }
}
