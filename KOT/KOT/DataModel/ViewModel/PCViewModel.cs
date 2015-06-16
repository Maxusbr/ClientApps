using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using KOT.Annotations;
using KOT.DataModel.Handlers;
using KOT.DataModel.Model;

namespace KOT.DataModel.ViewModel
{
    public class PCViewModel : INotifyPropertyChanged
    {
        public PCViewModel()
        {
            PCHandler.Instance.SourceChenged += Instance_SourceChenged;
            CarsHandler.SelectionChanged += CarsHandler_SelectionChanged;
            if (PCHandler.Instance.ListTrip.Count == 0) PCHandler.UpdateSource();
        }

        private void Instance_SourceChenged(string property)
        {
            switch (property)
            {
                case "StartDate":
                    UpdateStartTrip(true);
                    break;
                case "EndDate":
                    UpdateEndTrip(true);
                    break;
                case "":
                    UpdateStartTrip();
                    UpdateEndTrip();
                    _selectedStart = StartTrip[0];
                    _selectedEnd = EndtTrip[0];
                    OnPropertyChanged("SelectedStart");
                    OnPropertyChanged("SelectedEnd");
                    //Update();
                    break;
                case "Model":
                    if (PCHandler.Instance.TripModel == null)break;
                    Distance = string.Format("{0} км", PCHandler.Instance.TripModel.CurrentDistance);
                    TripTime = string.Format("{0} минут", PCHandler.Instance.TripModel.TripTime);
                    MedianSpeed = string.Format("{0} км/ч", PCHandler.Instance.TripModel.MedianSpeed);

                    IsReady = true;
                    break;
            }
        }

        void UpdateStartTrip(bool setFirst = false)
        {
            StartTrip.Clear();
            StartTrip.Add(new TripAdvisorViewModel("00:00", string.Empty));
            foreach (var el in PCHandler.Instance.ListTrip.Where(o => o.StartEng >= StartDate &&
                    o.StartEng <= StartDate.AddHours(23).AddMinutes(59)))
                StartTrip.Add(el);
            if(setFirst)
                SelectedStart = StartTrip[0];
        }

        void UpdateEndTrip(bool setFirst = false)
        {
            EndtTrip.Clear();
            EndtTrip.Add(new TripAdvisorViewModel(string.Empty, "Текущее"));
            EndtTrip.Add(new TripAdvisorViewModel(string.Empty, "23:59"));
            foreach (var el in PCHandler.Instance.ListTrip.Where(o => o.EndEng >= EndDate &&
                o.EndEng <= EndDate.AddHours(23).AddMinutes(59)))
                EndtTrip.Add(el);
            if (setFirst)
                SelectedEnd = EndtTrip[0];
        }

        void CarsHandler_SelectionChanged(object sender, EventArgs e)
        {
            Update();
            TotalDistance = string.Format("{0} км", PCHandler.Instance.ListTrip.Sum(o => o.CurrentDistance));
        }

        private readonly ObservableCollection<TripAdvisorViewModel> _starTrip = new ObservableCollection<TripAdvisorViewModel>();
        public ObservableCollection<TripAdvisorViewModel> StartTrip { get { return _starTrip; } }
        private readonly ObservableCollection<TripAdvisorViewModel> _endTrip = new ObservableCollection<TripAdvisorViewModel>();
        private TripAdvisorViewModel _selectedEnd;
        private TripAdvisorViewModel _selectedStart;
        private string _medianSpeed;
        private string _tripTime;
        private string _distance;
        private bool _isReady;
        private string _totalDistance;
        public ObservableCollection<TripAdvisorViewModel> EndtTrip { get { return _endTrip; } }

        public bool IsReady
        {
            get { return _isReady; }
            set
            {
                if (value.Equals(_isReady)) return;
                _isReady = value;
                OnPropertyChanged("IsReady");
            }
        }

        private DateTime StartDate
        {
            get { return PCHandler.StartDate; }
            set
            {
                if(PCHandler.StartDate.Equals(value)) return;
                IsReady = false;
                PCHandler.StartDate = value;
            }
        }

        private DateTime EndDate
        {
            get { return PCHandler.EndDate; }
            set
            {
                if (PCHandler.StartDate.Equals(value)) return;
                IsReady = false;
                PCHandler.EndDate = value;
            }
        }

        public string StartDateString
        {
            get { return StartDate.ToString("d"); }
            set
            {
                DateTime dt;
                if (DateTime.TryParse(value, out dt))
                    StartDate = dt;
            }
        }

        public string EndDateString
        {
            get { return EndDate.ToString("d"); }
            set
            {
                DateTime dt;
                if (DateTime.TryParse(value, out dt))
                    EndDate = dt;
            }
        }

        public TripAdvisorViewModel SelectedStart
        {
            get { return _selectedStart; }
            set
            {
                if (Equals(value, _selectedStart)) return;
                IsReady = false;
                _selectedStart = value;
                OnPropertyChanged("SelectedStart");
                if(value != null)
                    Update();
            }
        }

        public TripAdvisorViewModel SelectedEnd
        {
            get { return _selectedEnd; }
            set
            {
                if (Equals(value, _selectedEnd)) return;
                IsReady = false;
                _selectedEnd = value;
                OnPropertyChanged("SelectedEnd");
                if (value != null)
                    Update();
            }
        }

        private void Update()
        {
            TimeSpan ts;
            if (SelectedStart == null || !TimeSpan.TryParse(SelectedStart.TimeStart + ":00", out ts))
                ts = new TimeSpan(0, 0, 0);
            var startdt = StartDate + ts;
            var enddt = DateTime.Now;
            if (SelectedEnd != null && TimeSpan.TryParse(SelectedEnd.TimeEnd + ":00", out ts))
                enddt = EndDate + ts;
            PCHandler.UpdateSource(startdt, enddt);
            //var list = PCHandler.Instance.ListTrip.Where(o => o.StartEng >= startdt && o.EndEng <= enddt).ToList();
            //if (list.Count == 0)
            //{
            //    Distance = TripTime = MedianSpeed = string.Empty; return;
            //}
            //Distance = string.Format("{0} км", list.Sum(o => o.CurrentDistance));
            //TripTime = string.Format("{0} минут", list.Sum(o => o.TripTime));
            //MedianSpeed = string.Format("{0} км/ч", Math.Round(list.Average(o => o.MedianSpeed), 2));

            
        }

        public string Distance
        {
            get { return _distance; }
            set
            {
                if (value == _distance) return;
                _distance = value;
                OnPropertyChanged("Distance");
            }
        }

        public string TripTime
        {
            get { return _tripTime; }
            set
            {
                if (value == _tripTime) return;
                _tripTime = value;
                OnPropertyChanged("TripTime");
            }
        }

        public string MedianSpeed
        {
            get { return _medianSpeed; }
            set
            {
                if (value == _medianSpeed) return;
                _medianSpeed = value;
                OnPropertyChanged("MedianSpeed");
            }
        }

        public string TotalDistance
        {
            get { return _totalDistance; }
            set
            {
                if (value == _totalDistance) return;
                _totalDistance = value;
                OnPropertyChanged("TotalDistance");
            }
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
