using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;


namespace KOT.Common.Controls
{
    public partial class RatingControl : UserControl
    {
        public RatingControl()
        {
            this.InitializeComponent();
        }
        static readonly DependencyProperty BrushValueProperty =
            DependencyProperty.Register("Brush", typeof(Color), typeof(RatingControl), new PropertyMetadata(Color.FromArgb(255,233,30,99)));

        static readonly DependencyProperty RateValueProperty =
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

        private void Rectangle_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            Value = .2;
        }

        private void Rectangle_Tapped_1(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            Value = .4;
        }

        private void Rectangle_Tapped_2(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            Value = .6;
        }

        private void Rectangle_Tapped_3(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            Value = .8;
        }

        private void Rectangle_Tapped_4(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            Value = 1;
        }

        private static readonly DependencyProperty RateProperty =
            DependencyProperty.Register("Rate", typeof(double), typeof(RatingControl), new PropertyMetadata(""));

        public string Rate
        {
            get { return (string)GetValue(RateProperty); }
            private set { SetValue(RateProperty, value); }
        }


        private string UpdateRate(double value)
        {
            if (value < 0) return "";
            if (value <= .2) return "Ужасно";
            if (value <= .4) return "Плохо";
            if (value <= .6) return "Неплохо";
            return value <= .8 ? "Хорошо" : "Отлично";
        }

    }
}
