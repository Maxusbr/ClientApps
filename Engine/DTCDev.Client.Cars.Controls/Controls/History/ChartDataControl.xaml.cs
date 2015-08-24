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
        private ICollection<ChartValuesData> _data;
        private int _scale = 5;
        private double _controlWidth;
        private double _controlHeight;

        public ChartDataControl()
        {
            InitializeComponent();

        }

        public event MouseWheelEventHandler MouseWeel;
        protected virtual void OnMouseWeel(object sender, MouseWheelEventArgs e)
        {
            var obj = sender as Border;
            DateTime dt;
            if (obj != null && DateTime.TryParse(obj.ToolTip.ToString(), out dt)) CurenDate = dt;
            if (MouseWeel != null) MouseWeel.Invoke(this, e);
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

        private static void DataPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var control = sender as ChartDataControl;
            if (control == null || e.NewValue == null) return;
            control.DisplayData((ScaleValuesData)e.NewValue);
        }
        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _controlWidth = stkData.ActualWidth > 0 && (stkData.ActualWidth < _controlWidth || _controlWidth < 1) ?
                (int)stkData.ActualWidth - 2 : _controlWidth;
            _controlHeight = stkData.ActualHeight > 0 && (stkData.ActualHeight - 18 < _controlHeight || _controlHeight < 1) ?
                (int)stkData.ActualHeight - 2 : _controlHeight;
        }

        private void DisplayData(ScaleValuesData model)
        {
            _data = model.Data;
            _scale = model.Scale;
            ClearEvent(); stkTicks.Children.Clear();

            if (_data == null || !_data.Any() || Math.Abs(stkData.ActualWidth) < 0.1) return;
            Prescale(_data.Max(o => o.Value));
            if (_data.Any(o => o.Date.Hour > 0))
                DisplayHourData(model.UseMaxMin, model.MinVal, model.MaxVal);
            else DisplayDayData(model.UseMaxMin, model.MinVal, model.MaxVal);

        }

        private void DisplayDayData(bool useMaxMin, int minval, int maxval)
        {
            CurenDate = _data.Min(o => o.Date);
            var min = _data.Min(o => o.Value);
            if (min > 0) min = 0;
            var max = _data.Max(o => o.Value);
            var w = _controlWidth / _data.Count - 2;
            var h = _controlHeight / (max - min);
            h = h < 0 ? 0 : h;
            foreach (var el in _data.OrderBy(o => o.Date))
            {
                var b = new Border
                {
                    VerticalAlignment = VerticalAlignment.Bottom,
                    Background = new SolidColorBrush(Colors.Blue),
                    Margin = new Thickness(1),
                    Height = (int)(h * el.Value),
                    Width = (int)w,
                    ToolTip = el.Date.ToShortDateString(),
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
                b.MouseWheel += OnMouseWeel;
                stkData.Children.Add(b);
            }
        }

        private void DisplayHourData(bool useMaxMin, int minval, int maxval)
        {
            var first = _data.OrderBy(o => o.Date).FirstOrDefault();
            if (first == null) return;
            var ts = new TimeSpan(first.Date.Hour, 0, 0);
            var cnt = _data.Max(o => o.Date.Hour) - _data.Min(o => o.Date.Hour) + 1;
            var min = Math.Max(_data.Min(o => o.Value), 0);
            var max = _data.Max(o => o.Value);
            var w = (int)Math.Max(_controlWidth / cnt / multy, 1);
            var h = _controlHeight / (max - min);
            stkData.MouseWheel += OnMouseWeel;
            for (var i = 0; i < cnt * multy; i++)
            {
                var crnt = (int)(ts.TotalMinutes + multyTs.TotalMinutes * i);
                var el = _data.FirstOrDefault(
                        o => o.Date.TimeOfDay.TotalMinutes >= crnt &&
                            o.Date.TimeOfDay.TotalMinutes < crnt + multyTs.TotalMinutes);
                var b = new Border
                {
                    VerticalAlignment = VerticalAlignment.Bottom,
                    Background = el != null ? new SolidColorBrush(Colors.Blue) : null,
                    ToolTip = el != null ? el.Date.ToString("g") : null,
                    Height = el != null ? (int)(h * el.Value): 0,
                    Width = w
                };
                if (el != null && useMaxMin) b.Background = GetBrush(el.Value, minval, maxval);
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
                switch (_scale)
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
                switch (_scale)
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

        private SolidColorBrush GetBrush(double vol, int min, int max)
        {
            if (vol < min)
                return new SolidColorBrush(Colors.Green);
            return vol < max ? new SolidColorBrush(Colors.Yellow) : new SolidColorBrush(Colors.Red);
        }

        private void Prescale(double max)
        {
            var pre = (int)max / 3;
            txt0.Text = pre.ToString();
            txt1.Text = (pre * 2).ToString();
            txt2.Text = ((int)Math.Ceiling(max)).ToString();
        }

        private void ClearEvent()
        {
            if (_scale == 5)
                foreach (var b in (from object el in stkData.Children select el as Border).Where(b => b == null))
                    b.MouseWheel -= OnMouseWeel;
            stkData.MouseWheel -= OnMouseWeel;
            stkData.Children.Clear();
        }

    }
}
