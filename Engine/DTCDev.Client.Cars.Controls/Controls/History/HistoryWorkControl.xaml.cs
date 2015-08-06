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
using DTCDev.Client.Cars.Controls.ViewModels.History;

namespace DTCDev.Client.Cars.Controls.Controls.History
{
    /// <summary>
    /// Логика взаимодействия для HistoryWorkControl.xaml
    /// </summary>
    public partial class HistoryWorkControl : Window
    {
        private HistoryWorkViewModel _hvm;
        public HistoryWorkControl()
        {
            InitializeComponent();
            DataContext = _hvm = new HistoryWorkViewModel(Dispatcher);
            
        }

        private void grdHistoryWork_MouseLeave(object sender, MouseEventArgs e)
        {
            
        }

        private void grdHistoryWork_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            
        }

        private void grdHistoryWork_MouseEnter(object sender, MouseEventArgs e)
        {
            
        }

        private void btnDistance_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
