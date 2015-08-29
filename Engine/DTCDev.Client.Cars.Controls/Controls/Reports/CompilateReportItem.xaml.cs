using DTCDev.Models.CarsSending.Car;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
    /// Interaction logic for CompilateReportItem.xaml
    /// </summary>
    public partial class CompilateReportItem : UserControl
    {
        public CompilateReportItem()
        {
            InitializeComponent();
            
        }

        private void _vm_CenterUpdates(Client.Controls.Map.Location southWest, Client.Controls.Map.Location northEast)
        {
            cMap.Visibility = Visibility.Visible;
            var slowTask = new Task(delegate
            {
                Thread.Sleep(100);
            });
            slowTask.ContinueWith(delegate
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    cMap.ZoomToBounds(southWest, northEast);
                })); 
            });
            slowTask.Start();
        }

        public CompilateReportItem(List<CarStateModel> historyTrack)
        {

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as HistoryCarStateViewModel;
            if (vm != null) vm.CenterUpdates += _vm_CenterUpdates;
        }
    }
}
