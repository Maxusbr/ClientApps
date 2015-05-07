using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DTCDev.Client.Cars.Service.Controls.CalendarControls
{
    /// <summary>
    /// Логика взаимодействия для TimeSelectControl.xaml
    /// </summary>
    public partial class TimeSelectControl : UserControl
    {
        public static readonly DependencyProperty StartTimeProperty =
                DependencyProperty.Register("StartTime", typeof(int), typeof(TimeSelectControl),
                        new PropertyMetadata(-1, OnTimePropertyChanged));
        public static readonly DependencyProperty EndTimeProperty =
                DependencyProperty.Register("EndTime", typeof(int), typeof(TimeSelectControl),
                        new PropertyMetadata(-1, OnTimePropertyChanged));

        public int StartTime
        {
            get { return (int)GetValue(StartTimeProperty); }
            set
            {
                SetValue(StartTimeProperty, value);
            }
        }

        public int EndTime
        {
            get { return (int)GetValue(EndTimeProperty); }
            set
            {
                SetValue(EndTimeProperty, value);
            }
        }

        private static void OnTimePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var source = d as TimeSelectControl;
            if (source != null) source.UpdateUI();
        }

        public void UpdateUI()
        {
            if (EndTime < 0 || StartTime < 0) return;
            ListSelected.ToList().ForEach(o => o.IsSelect = false);
            if (StartTime < EndTime)
                for (var i = StartTime; i <= EndTime; i++)
                {
                    var el = List.FirstOrDefault(o => o.ID == i);
                    if (el != null) el.IsSelect = true;
                }
            if (StartTime <= EndTime) return;
            for (var i = 0; i <= StartTime; i++)
            {
                var el = List.FirstOrDefault(o => o.ID == i);
                if (el != null) el.IsSelect = true;
            }
            for (var i = EndTime; i <= 23; i++)
            {
                var el = List.FirstOrDefault(o => o.ID == i);
                if (el != null) el.IsSelect = true;
            }
        }

        private void Update()
        {
            StartTime = ListSelected.Any() ? ListSelected.Min(m => m.ID) : -1;
            EndTime = ListSelected.Any() ? ListSelected.Max(m => m.ID) : -1;
        }

        public TimeSelectControl()
        {
            this.InitializeComponent();
            InitTimeLine();
            SetSelected(9, 18);
        }

        private readonly List<TimeLineItem> _list = new List<TimeLineItem>();
        private bool _isSelected = false;
        private bool _isEdit = false;

        private IEnumerable<TimeLineItem> ListSelected
        {
            get { return List.Where(o => o.IsSelect); }
        }

        private IEnumerable<TimeLineItem> List
        {
            get { return (from object el in TimeLine.Children select el as TimeLineItem); }
        }

        public void SetSelected(int start, int end)
        {
            ListSelected.ToList().ForEach(o => o.IsSelect = false);
            List.Where(o => o.ID >= start && o.ID <= end).ToList().ForEach(e => e.IsSelect = true);
            Update();
        }

        private void InitTimeLine()
        {
            TimeLine.Children.Clear();
            for (var i = 0; i < 24; i++)
            {
                var el = new TimeLineItem { ID = i, Content = i.ToString("00"), IsSelect = false };
                el.MouseMove += TimeSelectControl_MouseMove;
                el.MouseLeftButtonDown += TimeLine_MouseLeftButtonDown;
                TimeLine.Children.Add(el);
            }
        }

        void TimeSelectControl_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var el = sender as TimeLineItem;
            if (el == null || !_isEdit || el.IsSelect == _isSelected) return;
            el.IsSelect = _isSelected;
            Update();
        }

        void TimeLine_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var el = sender as TimeLineItem;
            if (el == null) return;
            _isSelected = !el.IsSelect;
            if (el.IsSelect == _isSelected) return;
            el.IsSelect = _isSelected;
            Update();
        }

        private void UserControl_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _isEdit = true;
        }

        private void UserControl_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _isEdit = false;
        }

    }
    public class TimeLineItem : ContentControl
    {
        public int ID { get; set; }

        public override string ToString()
        {
            return Content.ToString();
            
        }

        #region IsSelect  (DependencyProperty)

        /// <summary>
        /// IsSelect Dependency Property
        /// </summary>
        public static readonly DependencyProperty IsSelectingProperty =
                DependencyProperty.Register("IsSelect", typeof(bool), typeof(TimeLineItem),
                        new PropertyMetadata((bool)false, OnSelectingPropertyChanged));

        private static void OnSelectingPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var source = d as TimeLineItem;
            if (source != null)
            source.UpdateLayout();
        }

        /// <summary>
        /// Gets or sets the IsSelect property.  This dependency property 
        /// indicates is control value total waiting.
        /// </summary>
        public bool IsSelect
        {
            get { return (bool)GetValue(IsSelectingProperty); }
            set { SetValue(IsSelectingProperty, value); }
        }


        #endregion
    }
}