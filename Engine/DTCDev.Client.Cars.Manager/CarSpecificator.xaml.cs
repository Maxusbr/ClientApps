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
using DTCDev.Client.Cars.Manager.ViewModels;

namespace DTCDev.Client.Cars.Manager
{
    /// <summary>
    /// Interaction logic for CarSpecificator.xaml
    /// </summary>
    public partial class CarSpecificator : UserControl
    {
        public CarSpecificator()
        {
            InitializeComponent();
            this.DataContext = new CarSpecificatorViewModel();
        }
    }
}
