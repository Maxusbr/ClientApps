using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KOT.DataModel.Model;

namespace KOT.DataModel.ViewModel
{
    public class WorkFromHistoryViewModel
    {
        private readonly CarHistoryWorkReport _model;
        private readonly DateTime _date;

        public WorkFromHistoryViewModel(CarHistoryWorkReport model)
        {
            _model = model;
            _date = new DateTime(model.Date.Y, model.Date.M, model.Date.D);
        }

        public string Distance { get { return string.Format("{0} км", _model.Distance); } }

        public string DateString { get { return Date.ToString("d"); } }

        public DateTime Date
        {
            get { return _date; }
        }

        public string Price { get { return string.Format("Цена: {0} руб.", Cost); } }

        public string WorkName { get { return _model.WorkName; } }

        public string NameWorker { get { return _model.Worker; } }

        public double Cost { get { return _model.Cost/10.0; } }
    }
}
