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
            trDetector = new Thread(DetectCallRefresh);
            trDetector.Start();
        }

        Thread trDetector;
        private double _lastWidth=0;

        private void DetectCallRefresh()
        {
            while (this != null)
            {
                if (this.ActualWidth != _lastWidth)
                {
                    _lastWidth = this.ActualWidth;
                }
                else
                {
                    _currentWidth = _lastWidth;
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            CalculateDisplayed();
                        }));
                }
                Thread.Sleep(200);
            }
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
            if (_data.Count() < 1)
                return;
            double step = (24.0 * 60.0) / _currentWidth;
            //step = Math.Round(step, 0) + 1;

            TimeSpan ts = TimeSpan.FromMinutes(step);

            int displayedDay = 0;

            DateTime d1 = new DateTime(_data[0].yy, _data[0].Mnth, _data[0].dd, 0, 0, 0);
            DateTime d2 = new DateTime();
            displayedDay = d1.Day;

            while (d1.Day == displayedDay)
            {
                d2 = d1;
                d1 += ts;
                List<CarStateModel> temp = _data.Where(p => p.hh >= d2.Hour && p.mm >= d2.Minute).ToList();
                temp = temp.Where(p => p.hh <= d1.Hour).ToList();
                temp = temp.Where(p => p.mm <= d1.Minute).ToList();
                if (temp.Count() > 0)
                {
                    int maxSpeed = temp.Max(p => p.Spd);
                    Border b = new Border
                    {
                        Width = 1,
                        Height = maxSpeed / prescale,
                        VerticalAlignment = System.Windows.VerticalAlignment.Bottom
                    };
                    if (maxSpeed < 900)
                        b.Background = new SolidColorBrush(Colors.Green);
                    else if (maxSpeed < 1100)
                        b.Background = new SolidColorBrush(Colors.Yellow);
                    else
                        b.Background = new SolidColorBrush(Colors.Red);
                    cnvData.Children.Add(b);
                }
                else
                {
                    Border b = new Border();
                    b.Width = 1;
                    b.Height = 30;
                    cnvData.Children.Add(b);
                }
            }
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
