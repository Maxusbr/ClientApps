using DTCDev.Client.Cars.Service.Engine.Storage;
using DTCDev.Models.CarsSending.Car;
using DTCDev.Models.Date;
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

namespace DTCDev.Client.Cars.Service.Controls.Car
{
    /// <summary>
    /// Interaction logic for CarParamsPresenterControl.xaml
    /// </summary>
    public partial class CarParamsPresenterControl : UserControl
    {
        public CarParamsPresenterControl()
        {
            InitializeComponent();
        }


        private static DependencyProperty DataProperty = DependencyProperty.Register("Data",
            typeof(OBDHistoryDataModel),
            typeof(CarParamsPresenterControl),
            new PropertyMetadata(OnDataPropertyCahnged));

        public OBDHistoryDataModel Data
        {
            get { return (OBDHistoryDataModel)GetValue(DataProperty); }
            set
            {
                SetValue(DataProperty, value);
            }
        }

        private static void OnDataPropertyCahnged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            CarParamsPresenterControl control = (CarParamsPresenterControl)sender;
            OBDHistoryDataModel data = (OBDHistoryDataModel)e.NewValue;
            control.StartUpdate(data);
        }


        private void StartUpdate(OBDHistoryDataModel data)
        {
            stkData.Children.Clear();
            stkHeader.Children.Clear();
            if (data == null)
                return;
            List<string> PIDS = data.Data.Select(p => p.Code).Distinct().ToList();
            BuildVertical(PIDS);
            _pids = PIDS;
            _data = data;
            Thread tr = new Thread(BuildData);
            tr.Start();
        }

        private void BuildVertical(List<string> pids)
        {
            stkHeader.Children.Add(new Border
            {
                Width = 100
            });
            PIDConverter converter = new PIDConverter();
            foreach (var item in pids)
            {
                TextBlock txt = new TextBlock();
                txt.Width = 80;
                txt.Margin = new Thickness(5, 0, 5, 0);
                txt.TextWrapping = TextWrapping.Wrap;
                txt.FontSize = 10;
                txt.Text = converter.GetPidInfo(item);
                txt.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                stkHeader.Children.Add(txt);
            }
        }

        OBDHistoryDataModel _data;
        List<string> _pids;


        private void BuildData()
        {
            List<TimeModel> times = _data.Data.Select(p => p.Time).Distinct().ToList();
            foreach (var item in times)
            {
                List<OBDHistoryDataModel.OBDParam> temp = _data.Data.Where(p => p.Time.H == item.H && p.Time.M==item.M && p.Time.S==item.S).ToList();
                this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        StackPanel sp = new StackPanel
                        {
                            Orientation = System.Windows.Controls.Orientation.Horizontal
                        };
                        sp.Children.Add(new TextBlock
                        {
                            VerticalAlignment = System.Windows.VerticalAlignment.Center,
                            Text = item.ToString(),
                            Width = 100
                        });

                        foreach (var pid in _pids)
                        {
                            OBDHistoryDataModel.OBDParam pd = temp.Where(p => p.Code == pid).FirstOrDefault();
                            if (pd != null)
                            {
                                sp.Children.Add(new TextBlock
                                {
                                    VerticalAlignment = System.Windows.VerticalAlignment.Center,
                                    Text = pd.Vol.ToString(),
                                    Width = 80,
                                    TextAlignment = TextAlignment.Center,
                                    Margin = new Thickness(5, 0, 5, 0)
                                });
                            }
                            else
                            {
                                sp.Children.Add(new Border
                                {
                                    VerticalAlignment = System.Windows.VerticalAlignment.Center,
                                    Margin = new Thickness(5, 0, 5, 0),
                                    Width = 80
                                });
                            }
                        }
                        stkData.Children.Add(sp);
                    }));
                Thread.Sleep(1);
            }
        }
    }
}
