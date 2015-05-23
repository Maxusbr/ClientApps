using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.UI;
using KOT.Annotations;
using KOT.DataModel.Model;

namespace KOT.DataModel.ViewModel
{
    public class WorkTypeViewModel : INotifyPropertyChanged
    {
        private bool _details;
        private DateTimeOffset _selectedDate;
        private string _date;
        public WorkType Model { get; set; }

        public WorkTypeViewModel()
        {
            if (DesignMode.DesignModeEnabled)
            {
                Model = new WorkType
                {
                    DaysToMake = 2,
                    ID = "0",
                    DistanceToMake = 200,
                    Name = "Work 1",
                    LastMakeDist = 250,
                    LastMakeTime = new DateDataModel(
                        DateTime.Now.AddDays(-5)),
                    PeriodicDist = 100,
                    PeriodicTime = 5,
                    PresentModel = 1
                };
            }
        }
        public WorkTypeViewModel(WorkType model)
        {
            Model = model;
        }

        public string Name
        {
            get { return Model.Name.Length > 29 ? Model.Name.Substring(0, 29) + "..." : Model.Name; }
        }

        public Color Foreground
        {
            get
            {
                return Pereprobeg ? Color.FromArgb(255, 233, 30, 99) : Color.FromArgb(255, 166, 166, 166);
            }
        }

        public bool Pereprobeg { get { return Model.DaysToMake < 0; } }

        public string DistanceToMake
        {
            get
            {
                return Pereprobeg ? string.Format("перепробег {0} км.", -Model.DaysToMake) : string.Format("через {0} км.", Model.DaysToMake);
            }
        }

        public string DaysToMake { get { return new DateDataModel(DateTime.Now.AddDays(Model.DaysToMake)).ToString(); } }

        public DateTimeOffset SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                if (value.Equals(_selectedDate)) return;
                _selectedDate = value;
                Date = value.ToString("d");
                OnPropertyChanged("SelectedDate");
            }
        }

        public string Date
        {
            get { return _date; }
            set
            {
                if (value == _date) return;
                _date = value;
                OnPropertyChanged("Date");
            }
        }

        public bool ShowDetails
        {
            get { return _details; }
            set
            {
                _details = value;
                OnPropertyChanged("ShowDetails");
            }
        }

        internal void ShowDetail()
        {
            ShowDetails = true;
        }

        internal void HideDetail(bool saveChange)
        {
            ShowDetails = false;
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
