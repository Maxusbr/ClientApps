using DTCDev.Client.Cars.Service.Engine.Handlers;
using DTCDev.Client.Cars.Service.Engine.Storage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace DTCDev.Client.Cars.Service.Controls.CalendarControls
{
    /// <summary>
    /// Interaction logic for CalendarCarItem.xaml
    /// </summary>
    public partial class CalendarCarItem : UserControl
    {
        public CalendarCarItem()
        {
            InitializeComponent();
        }

        public void Init(int day, List<DISP_Car> cars)
        {
            txtDate.Text = day.ToString();
            if (cars == null)
            {
                return;
            }
            else if (cars.Count() == 0)
            {
                return;
            }
            else
            {
                txtNoCar.Visibility = Visibility.Collapsed;
                foreach (var item in cars)
                {
                    TextBlock txt = new TextBlock();
                    txt.Tag = item;
                    txt.Text = item.CarModel.CarNumber;
                    txt.Foreground = new SolidColorBrush(Colors.Blue);
                    txt.Margin = new Thickness(0, 5, 0, 5);
                    txt.Cursor = Cursors.Hand;
                    txt.TextDecorations = TextDecorations.Underline;
                    txt.MouseLeftButtonUp += txt_MouseLeftButtonUp;
                    stkCars.Children.Add(txt);
                }
            }
        }

        void txt_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DISP_Car car = (DISP_Car)(((TextBlock)sender).Tag);
                CarStorage.Instance.GetCarDetails(car.CarModel.CarNumber);
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message, ex.InnerException.ToString());
            }
        }
    }
}
