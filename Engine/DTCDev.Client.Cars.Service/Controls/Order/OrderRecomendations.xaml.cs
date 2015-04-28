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

namespace DTCDev.Client.Cars.Service.Controls.Order
{
    /// <summary>
    /// Interaction logic for OrderRecomendations.xaml
    /// </summary>
    public partial class OrderRecomendations : UserControl
    {
        public OrderRecomendations()
        {
            InitializeComponent();
        }

        public event EventHandler CloseClick;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (CloseClick != null)
                CloseClick(this, new EventArgs());
        }
    }
}
