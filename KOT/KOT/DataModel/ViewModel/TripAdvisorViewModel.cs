using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using KOT.Annotations;
using KOT.DataModel.Model;

namespace KOT.DataModel.ViewModel
{
    public class TripAdvisorViewModel: INotifyPropertyChanged
    {
        private readonly TripAdvisorModel _model;
        public TripAdvisorViewModel(TripAdvisorModel model)
        {
            _model = model;
            //StartEng = ToDateTime(_model.StartEng);
            //EndEng = ToDateTime(_model.EndEng);
            TimeStart = StartEng.ToString("t");
            TimeEnd = EndEng.ToString("t");
        }

        public TripAdvisorViewModel(string start, string end)
        {
            TimeStart = start;
            TimeEnd = end;
        }

        public int CurrentDistance { get { return _model.CurrentDistance; } }

        public int TripTime { get { return _model.TripTime; } }

        public int MedianSpeed { get { return _model.MedianSpeed; } }

        public DateTime StartEng { get; set; }

        public string TimeStart { get; set; }

        public DateTime EndEng { get; set; }

        public string TimeEnd { get; set; }

        private DateTime ToDateTime(DateTimeDataModel d)
        {
            return new DateTime(d.Y, d.M, d.D, d.hh, d.mm, d.ss);
        }

        private DateTime ToDate(DateTimeDataModel d)
        {
            return new DateTime(d.Y, d.M, d.D);
        }

        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;


        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

    }
}
