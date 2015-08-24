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

        private void Vm_AddControl(string name, ScaleValuesData model)
        {
            var cntrls = stCharts.Children.OfType<ChartDataControl>();
            var cntrl = cntrls.FirstOrDefault(o => o.Name.Equals(name));
            if (cntrl == null)
            {
                cntrl = new ChartDataControl();
                cntrl.MouseWeel += ChartDataControl_MouseWheel;
                stCharts.Children.Add(cntrl);
            }
            cntrl.Data = model;
        }

        private void ChartDataControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var cntrl = sender as ChartDataControl;
            if (cntrl != null) _vm.SelectedDate = cntrl.CurenDate;
            _vm.Scale += e.Delta < 0 ? 1 : -1;
        }
    }
}
