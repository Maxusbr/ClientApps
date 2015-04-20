using DTCDev.Models.CarsSending.Car;
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

namespace DTCDev.Client.Cars.Controls.Controls.History
{
    /// <summary>
    /// Interaction logic for AccDataPresenter.xaml
    /// </summary>
    public partial class AccDataPresenter : UserControl
    {
        public AccDataPresenter()
        {
            InitializeComponent();
        }


        private static DependencyProperty _dataProperty = DependencyProperty.Register("Data",
            typeof(CarAccHistoryModel),
            typeof(AccDataPresenter),
            new PropertyMetadata(new CarAccHistoryModel(), OnDataChanged));

        public CarAccHistoryModel Data
        {
            get { return (CarAccHistoryModel)GetValue(_dataProperty); }
            set { SetValue(_dataProperty, value); }
        }

        private static void OnDataChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            AccDataPresenter control = sender as AccDataPresenter;
            if(e.NewValue==null)
            {
                control.grdLoad.Visibility = Visibility.Visible;
            }
            else
            {
                control.grdLoad.Visibility = Visibility.Collapsed;
                control.DisplayData((CarAccHistoryModel)e.NewValue);
            }
        }

        private void DisplayData(CarAccHistoryModel model)
        {
            CreateData(model);
            stkX.Children.Clear();
            stkY.Children.Clear();
            stkZ.Children.Clear();
            foreach (var item in NData)
            {
                stkX.Children.Add(GetBorder(item.MaxX));
                stkY.Children.Add(GetBorder(item.MaxY));
                stkZ.Children.Add(GetBorder(item.MaxZ));
            }
        }

        private Border GetBorder(double acc)
        {
            Border bx = new Border();
            bx.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
            bx.Width = 1;
            bx.Height = Math.Abs(Math.Round(acc * 20.0f));
            bx.Background = new SolidColorBrush(Colors.Green);
            if (acc > 1.5f)
                bx.Background = new SolidColorBrush(Colors.Red);
            else if (acc > 1)
                bx.Background = new SolidColorBrush(Colors.Yellow);
            return bx;
        }

        private void CreateData(CarAccHistoryModel model)
        {
            NData.Clear();
            if(model.Data.Count()<1)
                return;
            double width = stkX.ActualWidth;
            double step = width / 24 / 60;
            double minutesInStep = 1 / step;
            int w = (int)width;
            double graphHeight = 160;
            List<NormalizedData> nd = new List<NormalizedData>();
            foreach (var item in model.Data)
            {
                nd.Add(new NormalizedData
                    {
                        MaxZ = Math.Abs(item.MaxZ),
                        MaxX = Math.Abs(item.MaxX),
                        MaxY = Math.Abs(item.MaxY),
                        DT = new DateTime(item.Date.Y, item.Date.M, item.Date.D, item.Date.hh, item.Date.mm, item.Date.ss)
                    });
            }
            DateTime dt = new DateTime(model.Data[0].Date.Y, model.Data[0].Date.M, model.Data[0].Date.D);
            for (int i = 0; i < w; i++)
            {
                DateTime dtTemp = dt + TimeSpan.FromMinutes(minutesInStep);
                NormalizedData temp = new NormalizedData();
                List<NormalizedData> models = nd.Where(p => p.DT > dt && p.DT <= dtTemp).ToList();
                if(models.Count()>0)
                {
                    temp.MaxX = models.Max(p => p.MaxX) / 100.0f;
                    temp.MaxY = models.Max(p => p.MaxY) / 100.0f;
                    temp.MaxZ = models.Max(p => p.MaxZ) / 100.0f;
                }
                NData.Add(temp);
                dt = dtTemp;
            }
            if (NData == null)
                return;
            if (NData.Count() < 1)
                return;

            //find min data
            double x=0;
            if (NData.Where(p => p.MaxX > 0.1f).Count() > 0)
                x = NData.Where(p => p.MaxX > 0.1f).Min(p => p.MaxX);
            double y = 0;
            if (NData.Where(p => p.MaxY > 0.1f).Count() > 0)
                y = NData.Where(p => p.MaxY > 0.1f).Min(p => p.MaxY);
            double z = 0;
            if (NData.Where(p => p.MaxZ > 0.1f).Count() > 0)
                z = NData.Where(p => p.MaxZ > 0.1f).Min(p => p.MaxZ);

            foreach (var item in NData)
            {
                item.MaxX -= x;
                item.MaxY -= y;
                item.MaxZ -= z;
            }
        }

        private List<NormalizedData> NData = new List<NormalizedData>();

        private class NormalizedData
        {
            public NormalizedData()
            {
                MaxX = 0;
                MaxY = 0;
                MaxZ = 0;
            }

            public DateTime DT { get; set; }

            public double MaxX { get; set; }

            public double MaxY { get; set; }

            public double MaxZ { get; set; }
        }
    }
}
