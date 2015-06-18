using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace DTCDev.Client.Cars.Service.Engine.Controls.ViewModels
{
    public interface IDateList
    {
        List<DateListModel> DatesList { get; }
        DateTime Date { get; }
        int StartWorkTime { get;}
        int EndWorkTime { get;}
        bool WeekStyle { get; }
    }

    public class DateListModel
    {
        public DateTime DateWork { get; set; }
        public string ToolTip { get; set; }
    }
}
