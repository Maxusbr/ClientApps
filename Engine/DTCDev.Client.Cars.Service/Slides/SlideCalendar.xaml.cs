using DTCDev.Client.Cars.Service.Controls.CalendarControls;
using DTCDev.Client.Cars.Service.Engine.Handlers;
using DTCDev.Client.Cars.Service.Engine.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DTCDev.Client.Cars.Service.Slides
{
    /// <summary>
    /// Interaction logic for SlideCalendar.xaml
    /// </summary>
    public partial class SlideCalendar : UserControl
    {
        public SlideCalendar()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DisplayMonthes();
            DisplayWorks(0);
            DisplayOlder();
        }

        private void DisplayMonthes()
        {
            DateTime dt = DateTime.Now;
            btnCurrMonth.Content = dt.ToString("MMMM");
            if (dt.Month < 12)
                dt += new TimeSpan(DateTime.DaysInMonth(dt.Year, dt.Month + 1), 0, 0, 0);
            else
                dt += new TimeSpan(DateTime.DaysInMonth(dt.Year + 1, 1), 0, 0, 0);
            btnNextMonth.Content = dt.ToString("MMMM");
            if (dt.Month < 12)
                dt += new TimeSpan(DateTime.DaysInMonth(dt.Year, dt.Month + 1), 0, 0, 0);
            else
                dt += new TimeSpan(DateTime.DaysInMonth(dt.Year + 1, 1), 0, 0, 0);
            btnAfterNext.Content = dt.ToString("MMMM");
        }

        int start = 0;
        int stop = 0;
        int dayStart = 0;
        int dayStop = 0;

        private void DisplayWorks(int monthPlus)
        {
            wrpData.Children.Clear();
            start = stop = dayStart = dayStop = 0;
            
            DateTime dt = DateTime.Now;
            if (monthPlus == 0)
            {
                dayStart=dt.Day;
                dayStop = DateTime.DaysInMonth(dt.Year, dt.Month);
                stop = dayStop - dayStart;
            }
            else if (monthPlus == 1)
            {
                dayStart = 1;
                start = DateTime.DaysInMonth(dt.Year, dt.Month) - dt.Day;
                DateTime temp;
                if (dt.Month < 12)
                    temp = new DateTime(dt.Year, dt.Month + 1, 1);
                else
                    temp = new DateTime(dt.Year + 1, 1, 1);
                dayStop = DateTime.DaysInMonth(temp.Year, temp.Month);
                stop = start + dayStop;
            }
            else
            {
                dayStart = 1;
                start = DateTime.DaysInMonth(dt.Year, dt.Month) - dt.Day;
                DateTime temp;
                if (dt.Month < 11)
                {
                    temp = new DateTime(dt.Year, dt.Month + 2, 1);
                    start += DateTime.DaysInMonth(dt.Year, dt.Month + 1);
                }
                else if (dt.Month == 11)
                {
                    temp = new DateTime(dt.Year + 1, 1, 1);
                    start += DateTime.DaysInMonth(dt.Year + 1, 1);
                }
                else
                {
                    temp = new DateTime(dt.Year + 1, 2, 1);
                    start += DateTime.DaysInMonth(dt.Year + 1, 2);
                }
                dayStop = DateTime.DaysInMonth(temp.Year, temp.Month);
                stop = start + dayStop;
            }
            if (monthPlus == 0)
            {
                new Thread(TrDisplayCurrent).Start();
            }
            else
            {
                new Thread(TrDisplayOther).Start();
            }
        }

        private void TrDisplayCurrent()
        {
            int a = 0;
            for (int i = start; i <= stop; i++)
            {
                this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        List<DISP_Car> cars = CarStorage.Instance.Cars.Where(p => p.CarModel.DaysToService == a).ToList();
                        CalendarCarItem cci = new CalendarCarItem();
                        cci.Init(dayStart + i, cars);
                        wrpData.Children.Add(cci);
                    }));
                Thread.Sleep(20);
                a++;
            }
        }

        private void TrDisplayOther()
        {
            for (int i = dayStart; i <= dayStop; i++)
            {
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    int dReq = start + i - 1;
                    List<DISP_Car> cars = CarStorage.Instance.Cars.Where(p => p.CarModel.DaysToService == dReq).ToList();
                    CalendarCarItem cci = new CalendarCarItem();
                    cci.Init(dayStart + i - 1, cars);
                    wrpData.Children.Add(cci);
                }));
                Thread.Sleep(20);
            }
        }

        private void DisplayOlder()
        {
            List<DISP_Car> cars = CarStorage.Instance.Cars.Where(p => p.CarModel.DaysToService <0).ToList();
            lstOlder.ItemsSource = cars;
        }

        private void btnNextMonth_Click(object sender, RoutedEventArgs e)
        {
            DisplayWorks(1);
        }

        private void btnAfterNext_Click(object sender, RoutedEventArgs e)
        {
            DisplayWorks(2);
        }

        private void btnCurrMonth_Click(object sender, RoutedEventArgs e)
        {
            DisplayWorks(0);
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstOlder.SelectedIndex < 0)
                return;
            else
            {
                try
                {
                    DISP_Car car = (DISP_Car)lstOlder.SelectedItem;
                    if (car == null)
                        return;
                    CarStorage.Instance.GetCarDetails(car.CarModel.CarNumber);
                }
                catch { }
            }
        }
    }
}
