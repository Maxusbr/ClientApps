using DTCDev.Models.CarsSending.Order;
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

namespace DTCDev.Client.Cars.Service.Controls.PrintControls
{
    /// <summary>
    /// Interaction logic for OrderReportControl.xaml
    /// </summary>
    public partial class OrderReportPrintControl : UserControl
    {
        public OrderReportPrintControl()
        {
            InitializeComponent();
        }

        List<OrderModel> _orders;
        DateTime _dtFrom;
        DateTime _dtTo;

        public OrderReportPrintControl(List<OrderModel> orders, DateTime dtFrom, DateTime dtTo)
        {
            InitializeComponent();
            _orders = orders;
            _dtFrom = dtFrom;
            _dtTo = dtTo;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            BuildData();
        }

        private void BuildData()
        {
            if (_orders == null)
                return;
            txtFrom.Text = _dtFrom.ToString("dd.MM.yyyy");
            txtTo.Text = _dtTo.ToString("dd.MM.yyyy");
            foreach (var item in _orders)
            {
                StackPanel sp = new StackPanel();
                sp.Margin = new Thickness(5);
                sp.Orientation = Orientation.Horizontal;
                TextBlock t = new TextBlock
                {
                    Text = "№ " + item.OrderNumer,
                    Margin = new Thickness(10, 0, 10, 0),
                    FontSize = 14,
                    FontWeight = FontWeights.Bold
                };
                sp.Children.Add(t);

                TextBlock t3 = new TextBlock
                {
                    Text = "Госномер - " + item.CarNumber,
                    Margin = new Thickness(20, 0, 20, 0),
                    FontSize = 14
                };
                sp.Children.Add(t3);

                TextBlock t1 = new TextBlock
                {
                    Text = "Дата - "+item.DTCreate.ToString(),
                    Margin = new Thickness(20, 0, 20, 0),
                    FontSize = 14
                };
                sp.Children.Add(t1);

                TextBlock t2 = new TextBlock
                {
                    Text = "Сумма - " + item.Cost.ToString(),
                    Margin = new Thickness(20, 0, 20, 0),
                    FontSize = 14
                };
                sp.Children.Add(t2);
                stkDataReport.Children.Add(sp);
            }

            Border b = new Border
            {
                Height = 30,
                Background = new SolidColorBrush(Colors.LightGray)
            };
            stkDataReport.Children.Add(b);

            TextBlock txt = new TextBlock
            {
                Text = "Заказов - " + _orders.Count().ToString() + "; Сумма - " + _orders.Sum(p => p.Cost).ToString(),
                FontWeight = FontWeights.Bold,
                FontSize = 16,
                TextAlignment = TextAlignment.Center
            };
            stkDataReport.Children.Add(txt);
        }
    }
}
