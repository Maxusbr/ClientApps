using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using KOT.Annotations;


namespace KOT.Common.Controls
{
    public partial class RatingControl : UserControl, INotifyPropertyChanged
    {
        public RatingControl()
        {
            this.InitializeComponent();
        }
        static readonly DependencyProperty BrushValueProperty =
            DependencyProperty.Register("Brush", typeof(Color), typeof(RatingControl), new PropertyMetadata(Color.FromArgb(255, 233, 30, 99)));

        public static readonly DependencyProperty RateValueProperty =
            DependencyProperty.Register("RateValue", typeof(double), typeof(RatingControl), new PropertyMetadata(0.5, UpdateValue));

        private static void UpdateValue(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as RatingControl;
            if (control != null) control.Value = (double)e.NewValue;
        }

        public Color Brush
        {
            get { return (Color)GetValue(BrushValueProperty); }
            set { SetValue(BrushValueProperty, value); }
        }

        private double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set
            {
                SetValue(ValueProperty, value);
                Rate = UpdateRate(value);
                OnPropertyChanged("Rate1");
                OnPropertyChanged("Rate2");
                OnPropertyChanged("Rate3");
                OnPropertyChanged("Rate4");
                OnPropertyChanged("Rate5");
                OnPropertyChanged("VisableText");
            }
        }

        private static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(RatingControl), new PropertyMetadata(0.5));


        public double RateValue
        {
            get
            {
                return (double)GetValue(RateValueProperty);
            }
            set
            {
                SetValue(RateValueProperty, value);
            }
        }

        public static readonly DependencyProperty ViewTextPrperty =
            DependencyProperty.Register("ViewText", typeof(bool), typeof(RatingControl), new PropertyMetadata(false, ViewTextChange));

        private static void ViewTextChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as RatingControl;
            if (control != null) control.ViewText = (bool)e.NewValue;
        }

        public bool ViewText
        {
            get { return (bool)GetValue(ViewTextPrperty); }
            set { SetValue(ViewTextPrperty, value); }
        }

        public bool VisableText { get { return ViewText && Value > 0; } }

        private void Rectangle_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            RateValue = .2;
        }

        private void Rectangle_Tapped_1(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            RateValue = .4;
        }

        private void Rectangle_Tapped_2(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            RateValue = .6;
        }

        private void Rectangle_Tapped_3(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            RateValue = .8;
        }

        private void Rectangle_Tapped_4(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            RateValue = 1;
        }

        private static readonly DependencyProperty RateProperty =
            DependencyProperty.Register("Rate", typeof(double), typeof(RatingControl), new PropertyMetadata(""));

        public string Rate
        {
            get { return (string)GetValue(RateProperty); }
            private set { SetValue(RateProperty, value); }
        }

        public static readonly DependencyProperty IsUserRateProperty =
            DependencyProperty.Register("IsUserRate", typeof(bool), typeof(RatingControl), new PropertyMetadata(false, IsUserRateChange));

        private static void IsUserRateChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as RatingControl;
            if (control == null) return;
            control.IsUserRate = (bool)e.NewValue;
            control.OnPropertyChanged("IsNotUserRate");
        }

        public bool IsUserRate
        {
            get { return (bool)GetValue(IsUserRateProperty); }
            set { SetValue(IsUserRateProperty, value); }
        }

        public bool IsNotUserRate
        {
            get { return !(bool)GetValue(IsUserRateProperty); }
            set { SetValue(IsUserRateProperty, !value); }
        }
        private string UpdateRate(double value)
        {
            if (value < 0) return "";
            if (value <= .2) return "Ужасно";
            if (value <= .4) return "Плохо";
            if (value <= .6) return "Неплохо";
            return value <= .8 ? "Хорошо" : "Отлично";
        }

        //private static readonly DependencyProperty Rate1Property =
        //    DependencyProperty.Register("Rate1", typeof(bool), typeof(RatingControl), new PropertyMetadata(false));

        public bool Rate1
        {
            get { return Value > 0; }
        }

        //private static readonly DependencyProperty Rate2Property =
        //    DependencyProperty.Register("Rate2", typeof(bool), typeof(RatingControl), new PropertyMetadata(false));

        public bool Rate2
        {
            get { return Value > .2; }
        }

        //private static readonly DependencyProperty Rate3Property =
        //    DependencyProperty.Register("Rate3", typeof(bool), typeof(RatingControl), new PropertyMetadata(false));

        public bool Rate3
        {
            get { return Value > .4; }
        }

        //private static readonly DependencyProperty Rate4Property =
        //    DependencyProperty.Register("Rate4", typeof(bool), typeof(RatingControl), new PropertyMetadata(false));

        public bool Rate4
        {
            get { return Value > .6; }
        }

        //private static readonly DependencyProperty Rate5Property =
        //    DependencyProperty.Register("Rate5", typeof(bool), typeof(RatingControl), new PropertyMetadata(false));

        public bool Rate5
        {
            get { return Value > .8; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
