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
using DTCDev.Client.Cars.Engine.DisplayModels;

namespace DTCDev.Client.Cars.Controls.Controls.Car
{
    /// <summary>
    /// Interaction logic for CarsView.xaml
    /// </summary>
    public partial class CarsView : UserControl
    {
        private ViewModels.Car.CarsViewModel _vm;

        public CarsView()
        {
            InitializeComponent();
            this.DataContext = _vm = new ViewModels.Car.CarsViewModel(Dispatcher);
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var cntrl = sender as FrameworkElement;
            if (cntrl == null) return;
            var car = cntrl.DataContext as DISP_Car;
            if (car == null) return;
            car.VisableOBDDetails = !car.VisableOBDDetails;
        }

        
    }
}
