using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCDev.Client.Cars.Engine.AppLogic;
using DTCDev.Client.Controls.Map;
using DTCDev.Client.ViewModel;

namespace DTCDev.Client.Cars.Controls.Models
{
    public class ParkingModel : ViewModelBase
    {
        private DateTime _dateStart = new DateTime(1, 1, 1);
        private DateTime _dateEnd = new DateTime(1, 1, 1);

        public ParkingModel(DateTime dateStart, Location pos)
        {
            _dateStart = dateStart;
            Position = pos;
        }

        public ParkingModel(DateTime dateStart, DateTime dateEnd, Location pos)
        {
            _dateStart = dateStart;
            _dateEnd = dateEnd;
            Position = pos;
        }

        public void SetEndDates(DateTime dateEnd)
        {
            _dateEnd = dateEnd;
            OnPropertyChanged("StrDateEnd");
            OnPropertyChanged("TotallTime");
        }

        public Location Position { get; set; }

        public string StrDateStart
        {
            get { return _dateStart.ToString("g"); }
        }

        public string StrDateEnd
        {
            get { return _dateEnd.ToString("g"); }
        }

        public string TotallTime
        {
            get { return _dateEnd != new DateTime(1, 1, 1) ? (_dateEnd - _dateStart).ToString("g") : ""; }
        }

        public bool Equals(ParkingModel obj)
        {
            return new DistanceCalculator().Calculate(Position, obj.Position) < .5;
        }
    }
}
