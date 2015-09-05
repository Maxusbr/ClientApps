using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DTCDev.Client.Cars.Controls.ViewModels.History;

namespace DTCDev.Client.Cars.Controls.Controls.History
{
    /// <summary>
    /// Логика взаимодействия для ChartDataControl.xaml
    /// </summary>
    public partial class ChartDataControl : UserControl
    {
        private ScaleValuesData _data;
        private double _controlWidth;
        private double _controlHeight;

        public ChartDataControl()
        {
            InitializeComponent();

        }

        public delegate void BorderClickHandler(DateTime date);
        public delegate void BorderMouseWeelHandler(DateTime date, MouseWheelEventArgs e);
        public event BorderClickHandler BorderClick;
        protected virtual void OnBorderClick(object sender, MouseButtonEventArgs e)
        {
            var obj = sender as Border;
            DateTime dt;
            if (BorderClick == null || obj == null || !DateTime.TryParse(obj.ToolTip.ToString(), out dt)) return;
            CurenDate = new DateTime(dt.Year, dt.Month, dt.Day);
            BorderClick(dt);
        }

        public event BorderMouseWeelHandler MouseWeel;
        protected virtual void OnMouseWeel(object sender, MouseWheelEventArgs e)
        {
            var obj = sender as Border;
            DateTime dt;
            if (MouseWeel != null && obj != null && DateTime.TryParse(obj.ToolTip.ToString(), out dt))
                MouseWeel.Invoke(dt, e);
        }

        public DateTime CurenDate { get; set; }

        private static readonly DependencyProperty DataProperty = DependencyProperty.Register("Data", typeof(ScaleValuesData),
            typeof(ChartDataControl),
            new PropertyMetadata(new ScaleValuesData(), DataPropertyChanged));


        public ScaleValuesData Data
        {
            get { return (ScaleValuesData)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        private double ControlHeight
        {
            get
            {
                return _controlHeight;
            }
            set
            {
                _controlHeight = value;
                if (value > 0)
                    DisplayData(_data);
            }
        }

        private static void DataPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var control = sender as ChartDataControl;
            if (control == null || e.NewValue == null) return;
            control.DisplayData((ScaleValuesData)e.NewValue);
        }
        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _controlWidth = stkData.ActualWidth > 0 && _controlWidth < 1 ?
                (int)stkData.ActualWidth : _controlWidth;
            ControlHeight = _controlHeight < 1 ? (int)stkData.ActualHeight - 2 : _controlHeight;
        }



        private void DisplayData(ScaleValuesData model)
        {
            _data = model;
            ClearEvent(); stkTicks.Children.Clear();

            if (_data == null || _data.Data == null || !_data.Data.Any() || Math.Abs(_controlWidth) < 0.1) return;
            Prescale(_data.Data.Max(o => o.Value));
            if (_data.Data.Any(o => o.Date.Hour > 0))
                DisplayHourData(model.UseMaxMin, model.MinVal, model.MaxVal);
            else DisplayDayData(model.UseMaxMin, model.MinVal, model.MaxVal);

        }

        private void DisplayDayData(bool useMaxMin, double minval, double maxval)
        {
            CurenDate = _data.Data.Min(o => o.Date);
            var min = 0;//_data.Data.Min(o => o.Value);
            var max = _data.Data.Max(o => o.Value);
            var w = _controlWidth / _data.Data.Count - 2;
            var h = max > 0 ? _controlHeight / (max - min): 0;
            h = h < 0 ? 0 : h;
            foreach (var el in _data.Data.OrderBy(o => o.Date))
            {
                var b = new Border
                {
                    VerticalAlignment = VerticalAlignment.Bottom,
                    Background = new SolidColorBrush(Colors.Blue),
                    Margin = new Thickness(1),
                    Height = (int)(h * el.Value),
                    Width = w,
                    ToolTip = el.Date.ToShortDateString(),
                    Cursor = Cursors.Hand,
                    Child = el.Value > 5 * h ? new TextBlock
                    {
                        Text = ((int)el.Value).ToString("###"),
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Foreground = new SolidColorBrush(Colors.White),
                        FontSize = 8
                    } : null
                };
                if (useMaxMin) b.Background = GetBrush(el.Value, minval, maxval);
                b.MouseLeftButtonUp += OnBorderClick;
                stkData.Children.Add(b);
                stkTicks.Children.Add(new Border
                {
                    Width = w,
                    Margin = new Thickness(1),
                    BorderThickness = new Thickness(1, 0, 0, 0),
                    BorderBrush = new SolidColorBrush(Colors.Gray),
                    Child = new TextBlock { Text = el.Date.ToString("dd.MM"), FontSize = 8 }
                });
            }
        }

        private void DisplayHourData(bool useMaxMin, double minval, double maxval)
        {
            var first = _data.Data.OrderBy(o => o.Date).FirstOrDefault();
            if (first == null) return;
            var ts = new TimeSpan(first.Date.Hour, 0, 0);
            var cnt = _data.Data.Max(o => o.Date.Hour) - _data.Data.Min(o => o.Date.Hour) + 1;
            var min = 0;//Math.Max(_data.Data.Min(o => o.Value), 0);
            var max = _data.Data.Max(o => o.Value);
            var w = (int)Math.Max(_controlWidth / cnt / multy, 1);
            var h = max > 0 ? _controlHeight / (max - min) : 0;

            for (var i = 0; i < cnt * multy; i++)
            {
                var crnt = (int)(ts.TotalMinutes + multyTs.TotalMinutes * i);
                var el = _data.Data.FirstOrDefault(
                        o => o.Date.TimeOfDay.TotalMinutes >= crnt &&
                            o.Date.TimeOfDay.TotalMinutes < crnt + multyTs.TotalMinutes);
                var b = new Border
                {
                    VerticalAlignment = VerticalAlignment.Bottom,
                    Background = el != null ? new SolidColorBrush(Colors.Blue) : null,
                    ToolTip = el != null ? el.Date.ToString("g") : null,
                    Height = el != null ? (int)(h * el.Value) : 0,
                    Width = w,
                    Cursor = Cursors.Hand
                };
                if (el != null && useMaxMin) b.Background = GetBrush(el.Value, minval, maxval);
                b.MouseWheel += OnMouseWeel;
                b.MouseLeftButtonUp += OnBorderClick;
                stkData.Children.Add(b);
            }
            for (var i = 0; i < cnt; i++)
                stkTicks.Children.Add(new Border
                {
                    Width = w * multy,
                    BorderThickness = new Thickness(1, 0, 0, 0),
                    BorderBrush = new SolidColorBrush(Colors.Gray),
                    Child = new TextBlock { Text = i.ToString(), FontSize = 8 }
                });
        }

        private int multy
        {
            get
            {
                switch (_data.Scale)
                {
                    case 1:
                        return 60;
                    case 2:
                        return 4;
                    case 3:
                        return 2;
                    default:
                        return 1;
                }
            }
        }

        private TimeSpan multyTs
        {
            get
            {
                switch (_data.Scale)
                {
                    case 1:
                        return new TimeSpan(0, 1, 0);
                    case 2:
                        return new TimeSpan(0, 15, 0);
                    case 3:
                        return new TimeSpan(0, 30, 0);
                    default:
                        return new TimeSpan(1, 0, 0);
                }
            }
        }

        private SolidColorBrush GetBrush(double vol, double min, double max)
        {
            if (vol < min)
                return new SolidColorBrush(Colors.Green);
            return vol < max ? new SolidColorBrush(Colors.Yellow) : new SolidColorBrush(Colors.Red);
        }

        private void Prescale(double max)
        {
            var pre = max / 4;
            txt0.Text = pre < 1 ? pre.ToString("F2") : ((int)pre).ToString();
            txt1.Text = pre < 1 ? (pre * 2).ToString("F2") : ((int)pre * 2).ToString();
            txt2.Text = pre < 1 ? (pre * 3).ToString("F2") : ((int)pre * 3).ToString();
            txt3.Text = pre < 1 ? max.ToString("F2") : ((int)Math.Ceiling(max)).ToString();
        }

        private void ClearEvent()
        {
            if (_data == null) return;
            if (_data.Scale == 5)
                foreach (var b in (from object el in stkData.Children select el as Border).Where(b => b == null))
                {
                    b.MouseWheel -= OnMouseWeel;
                    b.MouseLeftButtonUp -= OnBorderClick;
                }
            stkData.Children.Clear();
        }


    }
}
