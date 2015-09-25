using DTCDev.Client.Busy;
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
            ccBusy.Visibility = Visibility.Collapsed;
            ccBusy.Content = null;
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
            var report = new List<DTCReportModel>();
            foreach (var item in data)
            {
                CarDTCRowView row = new CarDTCRowView(item);
                stkErrors.Children.Add(row);
                DTCReportModel temp = report.FirstOrDefault(p => p.ID == item.MessageCode);
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
            ccBusy.Visibility = Visibility.Visible;
            ccBusy.Content = new MacBusyControl { IsWaiting = true };
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

        private void Button_Print_Click(object sender, RoutedEventArgs e)
        {
            var printDialog = new PrintDialog();
            if (printDialog.ShowDialog() != true) return;
            var myPanel = new StackPanel { Orientation = Orientation.Vertical, HorizontalAlignment = HorizontalAlignment.Stretch };
            myPanel.Children.Add(new TextBlock
            {
                Text = "Статистика по ошибкам",
                TextAlignment = TextAlignment.Center,
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 10)
            });
            myPanel.Children.Add(new TextBlock
            {
                Text = string.Format("Госномер: {0}, марка: {1}, модель автомобиля: {2}",
                _car.CarModel.CarNumber, _car.CarModel.Mark, _car.CarModel.Model),
                TextAlignment = TextAlignment.Center,
                FontSize = 14,
                Margin = new Thickness(0, 0, 0, 15)
            });

            var trr = new DataGrid { AutoGenerateColumns = false, HorizontalAlignment = HorizontalAlignment.Stretch };
            var wd = new DataGridLength(1, DataGridLengthUnitType.Star);
            trr.Columns.Add(new DataGridTextColumn { Header = "Номер ошибки", Binding = new Binding("ID"), Width = wd });
            trr.Columns.Add(new DataGridTextColumn { Header = "Кол-во", Binding = new Binding("Count"), Width = wd });
            trr.Columns.Add(new DataGridTextColumn { Header = "Зафикисирована", Binding = new Binding("LastDate"), Width = wd });
            trr.ItemsSource = dtgrdErrorsStat.ItemsSource;
            myPanel.Children.Add(trr);

            myPanel.Measure(new Size(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight));
            //myPanel.Arrange(new Rect(new Point(0, 0), myPanel.DesiredSize));
            printDialog.PrintVisual(myPanel, "Статистика по ошибкам");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDropError_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("Вы уверены, что хотите удалить коды неисправностей в автомобиле? Коды неисправностей будут удалены не в базе программы, а очищены непосредственно в автомобиле.", "Очистка кодов неисправностей", MessageBoxButton.YesNo)==MessageBoxResult.Yes)
            {
                CarsHandler.Instance.ClearDTCCodesSended += Instance_ClearDTCCodesSended;
                CarsHandler.Instance.ClearTroubleCodes(_car.CarModel.DID);
            }
        }

        void Instance_ClearDTCCodesSended(object sender, EventArgs e)
        {
            bool result = (bool)sender;
            if(result)
            {
                MessageBox.Show("Отправка запроса на сброс кодов выполнена успешно. Коды неисправностей будут сброшены в ближайшее время");
            }
            else
            {
                MessageBox.Show("Во время выполнения запроса произошла ошибка. Попробуйте повторить действие.");
            }
            CarsHandler.Instance.ClearDTCCodesSended -= Instance_ClearDTCCodesSended;
        }
    }
}
