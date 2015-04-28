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

namespace DTCDev.Client.Cars.Service.Controls.Car
{
    /// <summary>
    /// Interaction logic for CarAddWork_WorkList.xaml
    /// </summary>
    public partial class CarAddWork_WorkList : UserControl
    {
        public CarAddWork_WorkList()
        {
            InitializeComponent();
        }

        public event EventHandler CloseMe;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (CloseMe != null)
                CloseMe(this, new EventArgs());
        }
    }
}
