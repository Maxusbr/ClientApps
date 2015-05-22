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
using DTCDev.Models.CarsSending.Car;
using System.Threading;

namespace DTCDev.Client.Cars.Controls.Controls.History
{
    /// <summary>
    /// Interaction logic for History_OnWay.xaml
    /// </summary>
    public partial class History_OnWay : UserControl
    {
        private double _currentWidth = 300;

        private List<CarStateModel> _data = new List<CarStateModel>();

        public History_OnWay()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
        }


        private void UserControl_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
            _currentWidth = this.ActualWidth;
        }





        private static DependencyProperty DataProperty = DependencyProperty.Register("Data", typeof(List<CarStateModel>),
            typeof(History_OnWay),
            new PropertyMetadata(new List<CarStateModel>(), DataPropertyChanged));

        public List<CarStateModel> Data
        {
            get { return (List<CarStateModel>)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        private static void DataPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            History_OnWay control = sender as History_OnWay;
            control._data = (List<CarStateModel>)e.NewValue;
            control.CalculateDisplayed();
        }






        private void Calculate()
        {
            try
            {
                cnvData.Children.Clear();
                cnvData.Width = _currentWidth;
                double step = _currentWidth / (24 * 60 * 60);
                TimeSpan ts = new TimeSpan(0);
                if (_data == null)
                {
                    LayoutRoot.Children.Add(new Border
                    {
                        Background = new SolidColorBrush(Colors.White)
                    });
                    return;
                }
                if (_data.Count() < 1)
                {
                    LayoutRoot.Children.Add(new Border
                    {
                        Background = new SolidColorBrush(Colors.White)
                    });
                    return;
                }
                foreach (var item in _data)
                {
                    TimeSpan tsCurrent = new TimeSpan(item.hh, item.mm, item.ss);
                    TimeSpan delta = tsCurrent - ts;
                    if (delta.TotalSeconds > 300)
                    {
                        AddBorder(step, (int)ts.TotalSeconds, (int)delta.TotalSeconds, 0);
                    }
                    else
                    {
                        AddBorder(step, (int)ts.TotalSeconds, (int)delta.TotalSeconds, item.Spd);
                    }
                    ts = tsCurrent;
                }
                AddBorder(step, (int)ts.TotalSeconds, (int)(new TimeSpan(1, 0, 0, 0).TotalSeconds), 0);
            }
            catch (Exception ex)
            {
                int i = 0;
            }
        }

        private void AddBorder(double step, int startSeconds, int widthSeconds, int speed)
        {
            try
            {
                int kmSpeed = speed / 50;
                Border b = new Border();
                b.Background = new SolidColorBrush(Colors.Blue);
                int h = 30 - kmSpeed;
                b.Height = h;
                b.Width = widthSeconds * step;
                cnvData.Children.Add(b);
                //Canvas.SetLeft(b, startSeconds * step);
                //Canvas.SetTop(b, 0);
                //b.Margin = new Thickness(startSeconds * step, 0, 0, 0);
            }
            catch (Exception ex)
            {
                int i = 0;
            }
        }

        private int prescale;

        private void CalculateDisplayed()
        {
            if (_currentWidth < 1)
                return;
            CalcPrescale();
            cnvData.Children.Clear();
            double step = _currentWidth / (24 * 60 * 60);

            int secondsInStep = 86400;
            double sStep = _currentWidth;
            while (sStep > 1)
            {
                secondsInStep = secondsInStep / 2;
                sStep = sStep / 2;
            }

            int currentSecond = 0;
            while (currentSecond < 86400)
            {
                List<CarStateModel> temp = _data.Where(p => p.Seconds >= currentSecond && p.Seconds < currentSecond + secondsInStep).ToList();
                if (temp.Count() < 1)
                    AddBorderRow(step, currentSecond, secondsInStep, 0);
                else
                {
                    int vol = temp.Sum(p => p.Spd) / temp.Count();
                    AddBorderRow(step, currentSecond, secondsInStep, vol);
                }
                currentSecond += secondsInStep;
            }
        }

        private void AddBorderRow(double step, int startSeconds, int widthSeconds, int vol)
        {
            Border b = new Border();
            b.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
            if (vol < 900)
                b.Background = new SolidColorBrush(Colors.Green);
            else if (vol < 1100)
                b.Background = new SolidColorBrush(Colors.Yellow);
            else
                b.Background = new SolidColorBrush(Colors.Red);
            b.Height = (int)(vol / prescale);
            double width = widthSeconds * step;
            if (width < 1)
                width = 1;
            b.Width = width;
            cnvData.Children.Add(b);
        }

        private void CalcPrescale()
        {
            if (_data.Count() < 1)
            {
                prescale = 1;
                return;
            }
            else
            {
                int maxSpd = Data.Max(p => p.Spd);
                if (maxSpd < 1)
                {
                    prescale = 1;
                    return;
                }
                decimal step = (maxSpd / 3.0m);
                prescale = ((int)step / 10) + 1;
                if (prescale < 1)
                    prescale = 1;
                txt0.Text = prescale.ToString();
                txt1.Text = (prescale * 2).ToString();
                txt2.Text = (prescale * 3).ToString();
            }
        }

    }
}
