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
using DTCDev.Client.Cars.Controls.ViewModels.Reports;

namespace DTCDev.Client.Cars.Controls.Controls.Reports
{
    /// <summary>
    /// Interaction logic for CompilateReport.xaml
    /// </summary>
    public partial class CompilateReport : UserControl
    {
        private CompilateReportViewModel _vm;

        public CompilateReport()
        {
            InitializeComponent();
            DataContext = _vm = new CompilateReportViewModel(Dispatcher);
            _vm.AddHistoryRow += _vm_AddHistoryRow;
            _vm.ClearHistory += _vm_ClearHistory;
        }

        private void _vm_ClearHistory(object sender, EventArgs e)
        {
            stkData.Children.Clear();
        }

        private void _vm_AddHistoryRow(HistoryCarStateViewModel item)
        {
            stkData.Children.Add(new CompilateReportItem {DataContext = item});
        }
    }
}
