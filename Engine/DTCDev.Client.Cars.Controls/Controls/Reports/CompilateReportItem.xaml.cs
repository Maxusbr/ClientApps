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

        public CompilateReportItem(List<CarStateModel> historyTrack)
        {

        }
    }
}
