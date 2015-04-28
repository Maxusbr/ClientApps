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
using DTCDev.Client.Cars.Service.Engine.Controls.ViewModels;

namespace DTCDev.Client.Cars.Service.Controls.Car
{
    /// <summary>
    /// Interaction logic for CarHistoryView.xaml
    /// </summary>
    public partial class CarHistoryView : UserControl
    {
        ShowHistoryViewModel _vm = new ShowHistoryViewModel();

        public CarHistoryView()
        {
            InitializeComponent();
            this.DataContext = _vm;
        }

        public ShowHistoryViewModel VM
        {
            get { return _vm; }
        }
    }
}
