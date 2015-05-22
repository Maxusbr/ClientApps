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
    /// Interaction logic for History_SatCount.xaml
    /// </summary>
    public partial class History_SatCount : UserControl
    {
        private double _currentWidth = 300;

        private List<CarStateModel> _data = new List<CarStateModel>();

        public History_SatCount()
        {
            InitializeComponent();
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _currentWidth = this.ActualWidth;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
        }





        private static DependencyProperty DataProperty = DependencyProperty.Register("Data", typeof(List<CarStateModel>),
            typeof(History_SatCount),
            new PropertyMetadata(new List<CarStateModel>(), DataPropertyChanged));

        public List<CarStateModel> Data
        {
            get { return (List<CarStateModel>)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        private static void DataPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            History_SatCount control = sender as History_SatCount;
            control._data = (List<CarStateModel>)e.NewValue;
            control.CalculateDisplayed();
        }


        private void CalculateDisplayed()
        {
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
                    AddBorder(step, currentSecond, secondsInStep, 0);
                else
                {
                    int vol = temp.Sum(p => p.St) / temp.Count();
                    AddBorder(step, currentSecond, secondsInStep, vol);
                }
                currentSecond += secondsInStep;
            }
        }

        private void AddBorder(double step, int startSeconds, int widthSeconds, int vol)
        {
            double height = vol;
            height = Math.Round(height) + 1;
            Border b = new Border();
            b.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
            if (vol < 5)
                b.Background = new SolidColorBrush(Colors.Red);
            else if (vol < 8)
                b.Background = new SolidColorBrush(Colors.Yellow);
            else
                b.Background = new SolidColorBrush(Colors.Green);
            b.Height = (int)height;
            double width = widthSeconds * step;
            if (width < 1)
                width = 1;
            b.Width = width;
            cnvData.Children.Add(b);
        }

        private void CalculateDisplayedOld()
        {
            if (_currentWidth < 1)
                return;
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
                    int minSat = temp.Min(p => p.St);
                    if (minSat > 15)
                        minSat = 15;
                    if (minSat < 2)
                        minSat = 2;
                    Border b = new Border
                    {
                        Width = 1,
                        Height = minSat * 2,
                        VerticalAlignment = System.Windows.VerticalAlignment.Bottom
                    };
                    if (minSat < 3)
                        b.Background = new SolidColorBrush(Colors.Red);
                    else if (minSat < 5)
                        b.Background = new SolidColorBrush(Colors.Yellow);
                    else
                        b.Background = new SolidColorBrush(Colors.Green);
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
    }
}
