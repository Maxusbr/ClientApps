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
using DTCDev.Client.Cars.Controls.ViewModels.History;

namespace DTCDev.Client.Cars.Controls.Controls.History
{
    /// <summary>
    /// Логика взаимодействия для HistoruChartControl.xaml
    /// </summary>
    public partial class HistoryChartControl : UserControl
    {
        private readonly HistoryChartViewModel _vm;

        public HistoryChartControl()
        {
            InitializeComponent();
            DataContext = _vm = new HistoryChartViewModel(Dispatcher);
            _vm.AddControl += Vm_AddControl;
            _vm.ClearControls += Vm_ClearControls;
            ChartDataControl.MouseWeel += ChartDataControl_MouseWheel;
        }

        private void Vm_ClearControls(object sender, EventArgs e)
        {
            foreach (var cntrl in stCharts.Children.OfType<ChartDataControl>())
                cntrl.MouseWeel -= ChartDataControl_MouseWheel;
            stCharts.Children.Clear();
            stImages.Children.Clear();
        }

        private void Vm_AddControl(ScaleValuesData model)
        {
            var cntrls = stCharts.Children.OfType<ChartDataControl>();
            var cntrl = cntrls.FirstOrDefault(o => o.Name.Equals(model.Name ?? "OBD" + model.Code));
            if (cntrl == null)
            {
                cntrl = new ChartDataControl { Name = string.IsNullOrEmpty(model.Name) ? "OBD" + model.Code: model.Name, MinHeight = 80};
                cntrl.MouseWeel += ChartDataControl_MouseWheel;
                stCharts.Children.Add(cntrl);
                var value = model.Data.Average(o => o.Value);
                var element = new Border
                {
                    Child = GetElement(model.Name ?? model.Code, ((int)value).ToString()),
                    //BorderThickness = new Thickness(1),
                    //BorderBrush = new SolidColorBrush(Colors.Gray)
                };
                var bind = new Binding("ActualHeight") { Source = cntrl };
                element.SetBinding(HeightProperty, bind);
                stImages.Children.Add(element);
            }
            cntrl.Data = model;
        }

        private void ChartDataControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var cntrl = sender as ChartDataControl;
            if (cntrl != null && cntrl.CurenDate > new DateTime(1, 1, 1)) _vm.SelectedDate = cntrl.CurenDate;
            _vm.Scale += e.Delta < 0 ? 1 : -1;
        }

        private FrameworkElement GetElement(string name, string val = "")
        {
            var element = new FrameworkElement {Width = 24};
            switch (name)
            {
                case "speed": element = new Image
                   {
                       Width = 32,
                       HorizontalAlignment = HorizontalAlignment.Center,
                       Source = new BitmapImage(new Uri("/DTCDev.Client.Cars.Controls;component/Assets/Content/Images/speed.png", UriKind.Relative))
                   };
                    break;
                case "satelites": element = new Image
                   {
                       Width = 24,
                       HorizontalAlignment = HorizontalAlignment.Center,
                       Source = new BitmapImage(new Uri("/DTCDev.Client.Cars.Controls;component/Assets/Content/Images/satellite-dish-icon.png", UriKind.Relative))
                   };
                    break;
                case "XChart":
                    element = new TextBlock {Width = 24, Text = "X", FontSize = 16, FontWeight = FontWeights.Bold, HorizontalAlignment = HorizontalAlignment.Stretch};
                    break;
                case "YChart":
                    element = new TextBlock { Width = 24, Text = "Y", FontSize = 16, FontWeight = FontWeights.Bold, HorizontalAlignment = HorizontalAlignment.Stretch };
                    break;
                case "ZChart":
                    element = new TextBlock { Width = 24, Text = "Z", FontSize = 16, FontWeight = FontWeights.Bold, HorizontalAlignment = HorizontalAlignment.Stretch };
                    break;
                default:
                    Sensors.OBD.OBDSensorDetector detector = new Sensors.OBD.OBDSensorDetector();
                    return detector.GetControl(name, val);
            }
            return element;
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            scrlImage.ScrollToVerticalOffset(e.VerticalOffset);
        }

        private void scrlImage_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {

        }
    }
}
