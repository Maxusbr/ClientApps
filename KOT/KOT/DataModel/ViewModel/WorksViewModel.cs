using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.UI.Xaml.Data;
using KOT.Annotations;
using KOT.DataModel.Handlers;
using KOT.DataModel.Model;

namespace KOT.DataModel.ViewModel
{
    public class WorksViewModel : INotifyPropertyChanged
    {
        private int _selectedYear;
        private readonly List<int> _yearList = new List<int>();
        private readonly ObservableCollection<WorksInMonthViewModel> _yearWorks = new ObservableCollection<WorksInMonthViewModel>();
        private WorksInMonthViewModel _selectedMonth;

        public WorksViewModel()
        {
            for (var i = DateTime.Now.Year; i < DateTime.Now.Year + 10; i++)
                YearList.Add(i);
            SelectedYear = DateTime.Now.Year;
            WorkTypes.CollectionChanged += WorkTypes_CollectionChanged;
            if (DesignMode.DesignModeEnabled)
                SelectedMonth = YearWorks[4];
        }

        public ObservableCollection<WorkTypeViewModel> WorkTypes { get { return WorksDataHandler.RecomendetWorkTypes; } }

        public ObservableCollection<WorksInMonthViewModel> YearWorks { get { return _yearWorks; } }

        public string DateTxt { get { return DateTime.Now.ToString("d"); } }

        public List<int> YearList { get { return _yearList; } }

        public int SelectedYear
        {
            get { return _selectedYear; }
            set
            {
                if (value == _selectedYear) return;
                _selectedYear = value;
                OnPropertyChanged("SelectedYearTxt");
                UpdateListWorks();
            }
        }

        public string SelectedYearTxt
        {
            get { return string.Format("{0} год", SelectedYear); }
        }

        public WorksInMonthViewModel SelectedMonth
        {
            get { return _selectedMonth; }
            set
            {
                _selectedMonth = value;
                OnPropertyChanged("SelectedMonth");
                OnPropertyChanged("MonthYearName");

                OnPropertyChanged("SelectedMotnthWorkTypes");
            }
        }

        public string MonthYearName
        {
            get { return SelectedMonth != null ? SelectedMonth.MonthYearName : ""; }
        }

        public ObservableCollection<WorkTypeViewModel> SelectedMotnthWorkTypes
        {
            get { return SelectedMonth != null ? SelectedMonth.WorkTypes : new ObservableCollection<WorkTypeViewModel>(); }
        }

        private void WorkTypes_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {

        }

        private void UpdateListWorks()
        {
            YearWorks.Clear();
            for (var i = 1; i <= 12; i++)
            {
                var month = new WorksInMonthViewModel(new DateTime(SelectedYear, i, 1));
                foreach (var work in WorkTypes.Where(o => o.DateWork.Year == SelectedYear && o.DateWork.Month == i))
                {
                    month.WorkTypes.Add(work);
                }
                month.UpdateDayInMonth();
                YearWorks.Add(month);
            }
        }

        public void SelectedMotnthWorkUpdate(DateTime dt)
        {
            foreach (var el in SelectedMotnthWorkTypes)
            {
                el.IsSelected = el.DateWork == dt;
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
