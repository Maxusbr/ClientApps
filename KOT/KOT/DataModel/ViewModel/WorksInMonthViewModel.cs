using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;
using KOT.Annotations;

namespace KOT.DataModel.ViewModel
{
    public class WorksInMonthViewModel : INotifyPropertyChanged
    {
        private readonly ObservableCollection<WorkTypeViewModel> _workTypes = new ObservableCollection<WorkTypeViewModel>();
        private readonly ObservableCollection<WorksInMonthViewModel> _worksInDay = new ObservableCollection<WorksInMonthViewModel>();

        public WorksInMonthViewModel(DateTime dt, bool isout = false)
        {
            Date = dt;
            IsOutMonth = isout;
        }

        public void UpdateDayInMonth()
        {
            var dayweek = ((int)Date.DayOfWeek == 0) ? 7 : (int)Date.DayOfWeek;
            for (var i = 1 - dayweek; i < DateTime.DaysInMonth(Date.Year, Date.Month); i++)
            {
                var day = new WorksInMonthViewModel(new DateTime(Date.AddDays(i).Year, Date.AddDays(i).Month, Date.AddDays(i).Day),
                    i<0);
                if (i > 0)
                    foreach (var work in WorkTypes.Where(o => 
                        o.DateWork.Year == Date.AddDays(i).Year &&
                        o.DateWork.Month == Date.AddDays(i).Month && 
                        o.DateWork.Day == i + 1))
                    {
                        day.WorkTypes.Add(work);
                    }
                WorksInDay.Add(day);
            }
        }

        public ObservableCollection<WorksInMonthViewModel> WorksInDay
        {
            get { return _worksInDay; }
        }
        public ObservableCollection<WorkTypeViewModel> WorkTypes
        {
            get { return _workTypes; }
        }

        public SolidColorBrush ColorBox
        {
            get
            {
                if (WorkTypes.Count > 2) return new SolidColorBrush(Color.FromArgb(255, 233, 30, 99));
                if (WorkTypes.Count > 1) return new SolidColorBrush(Color.FromArgb(255, 255, 160, 0));
                return new SolidColorBrush(WorkTypes.Count > 0 ? Color.FromArgb(255, 255, 193, 7) : Color.FromArgb(255, 232, 232, 232));
            }
        }

        public string CountTxt { get { return HasWorks ? WorkTypes.Count.ToString() : ""; } }

        public bool HasWorks { get { return WorkTypes.Count > 0; } }

        public DateTime Date { get; set; }

        public int DayName { get { return Date.Day; } }

        public string MonthName { get { return Date.ToString("MMMM", CultureInfo.CurrentCulture); } }

        public string MonthYearName { get { return Date.ToString("MMMM`yy", CultureInfo.CurrentCulture); } }

        public bool IsOutMonth { get; set; }

        public SolidColorBrush ColorText
        {
            get
            {
                if (IsOutMonth) return new SolidColorBrush(Color.FromArgb(255, 201, 201, 201));
                if (WorkTypes.Count > 0) return new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                return new SolidColorBrush(Color.FromArgb(255, 33, 33, 33));
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
