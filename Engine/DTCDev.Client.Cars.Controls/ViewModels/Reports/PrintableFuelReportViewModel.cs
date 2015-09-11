using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using DTCDev.Client.ViewModel;
using DTCDev.Models.CarBase.CarStatData;
using DTCDev.Models.CarsSending.Report;
using DTCDev.Models.Date;

namespace DTCDev.Client.Cars.Controls.ViewModels.Reports
{
    public class PrintableFuelReportViewModel : ViewModelBase
    {
        private DateTime _selectedDate;
        private string _carNumber = "";
        private ReportFuelModel _result;
        private List<KVPBase> _report = new List<KVPBase>();

        public PrintableFuelReportViewModel()
        {
            _result = new ReportFuelModel(); _selectedDate = DateTime.Now;
            if (!DesignerProperties.GetIsInDesignMode(new DependencyObject())) return;
            Report.Add(new KVPBase { Name = DateTime.Now.ToShortTimeString(), id = 0 });
        }

        public PrintableFuelReportViewModel(FuelReportViewModel model, DateTime dt)
        {
            CarNumber = model.CarNumber;
            var res = new ReportFuelModel();
            if (model.Result != null)
            {
                res.DID = model.Result.DID;
                res.Report = model.Result.Report.Where(o => dt.Equals(o.DT.ToDate())).OrderBy(r => r.DT.ToDateTime()).ToList();
                _report = res.Report.GroupBy(x =>
                {
                    var stamp = x.DT.ToDateTime();
                    stamp = stamp.AddMinutes(-(stamp.Minute % 15));
                    stamp = stamp.AddMilliseconds(-stamp.Millisecond - 1000 * stamp.Second);
                    return stamp;
                })
                .Select(g => new KVPBase { Name = string.Format("{0:00}:{1:00}", g.Key.Hour, g.Key.Minute), id = g.Min(f => f.Vol) }).ToList();
            }
            Result = res;
            _selectedDate = dt;
        }

        public string SelectedDate
        {
            get { return _selectedDate.ToShortDateString(); }
        }

        public string CarNumber
        {
            get { return _carNumber; }
            set
            {
                _carNumber = value;
                this.OnPropertyChanged("CarNumber");
            }
        }
        public ReportFuelModel Result
        {
            get { return _result; }
            set
            {
                _result = value;
                this.OnPropertyChanged("Result");
            }
        }

        public List<KVPBase> Report { get { return _report; } }
    }
}
