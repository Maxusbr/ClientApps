using DTCDev.Client.Cars.Service.Engine.Handlers;
using DTCDev.Client.Cars.Service.Engine.Storage;
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

namespace DTCDev.Client.Cars.Service.Controls.Car
{
    /// <summary>
    /// Interaction logic for CarDTCView.xaml
    /// </summary>
    public partial class CarDTCView : UserControl
    {
        private DISP_Car _car;
        DateTime dt = DateTime.Now;

        public CarDTCView()
        {
            InitializeComponent();
            _car = CarStorage.Instance.SelectedCar;
            DisplayErrors();
            CarsHandler.Instance.CarDTCMonthLoaded += Instance_CarDTCMonthLoaded;
        }

        void Instance_CarDTCMonthLoaded(List<Models.CarsSending.Car.CarDTCHistoryModel> data)
        {
            DisplayErrorsList(data);
        }

        private void DisplayErrorsList(List<Models.CarsSending.Car.CarDTCHistoryModel> data)
        {
            if (stkErrors != null)
                if (stkErrors.Children != null)
                    stkErrors.Children.Clear();
            if (data == null)
                return;
            if (_car == null)
                return;
            List<DTCReportModel> report = new List<DTCReportModel>();
            foreach (var item in data)
            {
                CarDTCRowView row = new CarDTCRowView(item);
                stkErrors.Children.Add(row);
                DTCReportModel temp = report.Where(p => p.ID == item.MessageCode).FirstOrDefault();
                if (temp == null)
                {
                    DateTime dt = new DateTime(item.Date.Y, item.Date.M, item.Date.D, item.Date.hh, item.Date.mm, item.Date.ss);
                    report.Add(new DTCReportModel
                    {
                        Count = 1,
                        ID = item.MessageCode,
                        LastDate = dt.ToString("dd.MM.yy hh:mm:ss"),
                        DT = dt
                    });
                }
                else
                {
                    temp.Count++;
                    if (dt > temp.DT)
                    {
                        temp.DT = dt;
                        temp.LastDate = dt.ToString("dd.MM.yy hh:mm:ss");
                    }
                }
            }

            dtgrdErrorsStat.ItemsSource = report;
        }

        private void DisplayErrors()
        {
            if (_car == null)
                return;
            DisplayErrorsList(_car.Errors);
            
        }

        private void chkLast_Checked(object sender, RoutedEventArgs e)
        {
            DisplayErrors();
            CheckLastVisSetter();

        }

        private void CheckLastVisSetter()
        {
            try
            {
                if (chkLast.IsChecked == true)
                {
                    imgLeft.Visibility = imgRight.Visibility = txtDate.Visibility = Visibility.Collapsed;
                }
            }
            catch { }
        }

        private void chkMonth_Checked(object sender, RoutedEventArgs e)
        {
            CallUpdate();
        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            dt = dt.AddMonths(-1);
            CallUpdate();
        }

        private void Image_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            if (dt.Month == DateTime.Now.Month)
                return;
            dt = dt.AddMonths(1);
            CallUpdate();
        }

        private void CallUpdate()
        {
            txtDate.Text = dt.ToString("MMMM yyyy");
            CarsHandler.Instance.GetDTCErrors(_car.CarModel.CarNumber, dt.Year, dt.Month);
            imgLeft.Visibility = Visibility.Visible;
            imgRight.Visibility = txtDate.Visibility = Visibility.Visible;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            CheckLastVisSetter();
        }


        public class DTCReportModel
        {
            public string ID { get; set; }

            public int Count { get; set; }

            public string LastDate { get; set; }

            public DateTime DT { get; set; }
        }
    }
}
