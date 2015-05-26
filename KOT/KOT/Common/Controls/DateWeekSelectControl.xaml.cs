using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Шаблон элемента пользовательского элемента управления задокументирован по адресу http://go.microsoft.com/fwlink/?LinkId=234236
using KOT.Annotations;

namespace KOT.Common.Controls
{
    public sealed partial class DateWeekSelectControl : UserControl, INotifyPropertyChanged
    {
        public DateWeekSelectControl()
        {
            this.InitializeComponent();
            //if (DesignMode.DesignModeEnabled)
            {
                SelectedYear = new YearClass(2015);
                
            }
        }

        public delegate void CustomClickEvent(object sender);
        public event CustomClickEvent Close;

        private void OnClose()
        {
            CustomClickEvent handler = Close;
            if (handler != null) handler(this);
        }

        private YearClass _vm;
        public static readonly DependencyProperty YearProperty =
            DependencyProperty.Register("Year", typeof(int), typeof(RatingControl), new PropertyMetadata(2015, UpdateValue));

        private YearClass _selectedYear;
        private static void UpdateValue(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as DateWeekSelectControl;
            if (control != null) control.SelectedYear = new YearClass((int)e.NewValue);
        }

        public int Year
        {
            get { return (int)GetValue(YearProperty); }
            set { SetValue(YearProperty, value); }
        }

        public DateTime Date
        {
            get
            {
                return new DateTime(SelectedYear.Number, SelectedYear.SelectedMonth.Number,
                    SelectedYear.SelectedDay.Number);
            }
        }

        public YearClass SelectedYear
        {
            get { return _selectedYear; }
            set
            {
                if (Equals(value, _selectedYear)) return;
                _selectedYear = value;
                OnPropertyChanged("SelectedYear");
                UpdateLists();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = SelectedYear;
            UpdateLists();
        }

        private void UpdateLists()
        {
            UpdateLayout();
            Month.ScrollIntoView(SelectedYear.SelectedMonth, ScrollIntoViewAlignment.Default);
            Days.ScrollIntoView(SelectedYear.SelectedDay, ScrollIntoViewAlignment.Default);
        }

        public static ScrollViewer GetScrollViewer(DependencyObject depObj)
        {
            if (depObj is ScrollViewer) return depObj as ScrollViewer;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);

                var result = GetScrollViewer(child);
                if (result != null) return result;
            }
            return null;
        }

        private void Month_Loaded(object sender, RoutedEventArgs e)
        {
            GetScrollViewer(Month).ViewChanged += MonthEvent_ViewChanged;
        }

        private void MonthEvent_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            var scroll = sender as ScrollViewer;
            if (scroll == null || !e.IsIntermediate) return;
            var wd = (int)Math.Round(scroll.HorizontalOffset / scroll.ViewportWidth);
            if (wd >= 0 && wd < SelectedYear.Months.Count)
            {
                SelectedYear.SelectedMonth = SelectedYear.Months[wd];
            }
        }

        private void Days_Loaded(object sender, RoutedEventArgs e)
        {
            GetScrollViewer(Days).ViewChanged += DaysEvent_ViewChanged;
        }

        private void DaysEvent_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            var scroll = sender as ScrollViewer;
            if (scroll == null || !e.IsIntermediate) return;
            var wd = (int)Math.Round(scroll.VerticalOffset / scroll.ViewportHeight);
            if (wd >= 0 && wd < SelectedYear.SelectedMonth.Days.Count)
            {
                SelectedYear.SelectedDay = SelectedYear.SelectedMonth.Days[wd];
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OnClose();
        }

        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }

    public class YearClass:INotifyPropertyChanged
    {
        private readonly int _number;
        private readonly ObservableCollection<MonthClass> _months = new ObservableCollection<MonthClass>();
        private DayClass _selectedDay;
        private MonthClass _selectedMonth;
        public int Number { get { return _number; } }

        public YearClass()
        {
            _number = 2015;
            Init();
        }

        public YearClass(int number)
        {
            _number = number;
            Init();
        }

        private void Init()
        {
            for (var i = 1; i <= 12; i++)
            {
                Months.Add(new MonthClass(i, Number));
            }
            if (Number != DateTime.Now.Year)
            {
                SelectedMonth = Months[0];
                SelectedDay = SelectedMonth.Days[0];
            }
            else
            {
                SelectedMonth = Months[DateTime.Now.Month - 1];
                SelectedDay = SelectedMonth.Days[DateTime.Now.Day - 1];
            }
            
        }

        public ObservableCollection<MonthClass> Months { get { return _months; } }
        public ObservableCollection<DayClass> Days { get { return SelectedMonth.Days; } }

        public DayClass SelectedDay
        {
            get { return _selectedDay; }
            set
            {
                if (Equals(value, _selectedDay)) return;
                _selectedDay = value;
                OnPropertyChanged("SelectedDay");
                OnPropertyChanged("DayWeek");
            }
        }

        public MonthClass SelectedMonth
        {
            get { return _selectedMonth; }
            set
            {
                if (Equals(value, _selectedMonth)) return;
                _selectedMonth = value;
                OnPropertyChanged("SelectedMonth");
                OnPropertyChanged("Days");
            }
        }

        public string DayWeek
        {
            get { return SelectedDay.DayWeek; }
        }

        public override string ToString()
        {
            return Number.ToString("0000");
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

    public class MonthClass
    {
        private readonly int _number;
        private readonly ObservableCollection<DayClass> _days = new ObservableCollection<DayClass>();
        private DateTime _date;
        public MonthClass(int month, int year)
        {
            _number = month;
            _date = new DateTime(year, month, 1);
            Init(year);
        }

        private void Init(int year)
        {
            for (var i = 1; i <= DateTime.DaysInMonth(year, Number); i++)
                Days.Add(new DayClass(i, Number, year));
        }

        public int Number { get { return _number; } }
        public ObservableCollection<DayClass> Days { get { return _days; } }

        public override string ToString()
        {
            return _date.ToString("MMMM", CultureInfo.CurrentCulture); ;
        }
    }

    public class DayClass
    {
        private readonly int _number;
        private DateTime _date;
        public DayClass(int day, int month, int year)
        {
            _date = new DateTime(year, month, day);
            _number = day;
        }
        public int Number { get { return _number; } }
        public string DayWeek { get { return _date.ToString("dddd", CultureInfo.CurrentCulture); } }

        public override string ToString()
        {
            return Number.ToString("00");
        }
    }
}
