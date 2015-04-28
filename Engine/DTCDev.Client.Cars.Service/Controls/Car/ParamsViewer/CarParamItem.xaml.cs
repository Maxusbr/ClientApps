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

namespace DTCDev.Client.Cars.Service.Controls.Car.ParamsViewer
{
    /// <summary>
    /// Interaction logic for CarParamItem.xaml
    /// </summary>
    public partial class CarParamItem : UserControl
    {
        public CarParamItem()
        {
            InitializeComponent();
        }

        private string _pid;
        private string _vol;
        private List<string> _previos = new List<string>();

        public CarParamItem(string pid, string vol)
        {
            InitializeComponent();
            _pid = pid;
            _vol = vol;
        }

        public CarParamItem(string pid, string vol, List<string> previos)
        {
            InitializeComponent();
            _pid = pid;
            _vol = vol;
            _previos = previos;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            PIDConverter converter = new PIDConverter();
            txtName.Text = converter.GetPidInfo(_pid);
            int v = 0;
            Int32.TryParse(_vol, out v);
            txtVol.Text = _vol;
            int max = converter.GetMaxVol(_pid);
            if (max == -1)
            {
                pgrs.Visibility = Visibility.Collapsed;
                return;
            }
            pgrs.Maximum = max;
            int min = converter.GetMinVol(_pid);
            pgrs.Minimum = min;
            pgrs.Value = v;

            if (_previos.Count() < 20 && max!=min)
            {
                double step = 50.0f / (double)(max - min);
                foreach (var item in _previos)
                {
                    int h = 0;
                    Int32.TryParse(item, out h);
                    if (h >= min && h <= max)
                    {
                        double b = (double)(h - min) * step;
                        b = Math.Round(b, 2);
                        b = Math.Abs(b);
                        if (b < 2)
                            b = 2;
                        Border b1 = new Border()
                        {
                            Width = 10,
                            Height = b,
                            VerticalAlignment = System.Windows.VerticalAlignment.Bottom
                        };
                        Border b2 = new Border()
                        {
                            Background = new SolidColorBrush(Colors.Blue),
                            VerticalAlignment = System.Windows.VerticalAlignment.Top,
                            Height = 2
                        };
                        b1.Child = b2;
                        cnvChart.Children.Add(b1);
                    }
                    else
                    {
                        cnvChart.Children.Add(new Border
                        {
                            Width = 10
                        });
                    }
                }
            }
        }
    }
}
