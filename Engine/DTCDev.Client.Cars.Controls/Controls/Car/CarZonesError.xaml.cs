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
using DTCDev.Client.Cars.Controls.ViewModels.Car;
using DTCDev.Client.Cars.Engine.DisplayModels;

namespace DTCDev.Client.Cars.Controls.Controls.Car
{
    /// <summary>
    /// Interaction logic for CarZonesError.xaml
    /// </summary>
    public partial class CarZonesError : UserControl
    {
        public CarZonesError()
        {
            InitializeComponent();
            //this.DataContext = CarZonesErrorViewModel.Instance;
        }
        //private List<DISP_Car> _data = new List<DISP_Car>();
        //private static DependencyProperty DataProperty = DependencyProperty.Register("Data", typeof(List<DISP_Car>),
        //    typeof(CarZonesError),
        //    new PropertyMetadata(new List<DISP_Car>(), DataPropertyChanged));

        //public List<DISP_Car> Data
        //{
        //    get { return (List<DISP_Car>)GetValue(DataProperty); }
        //    set { SetValue(DataProperty, value); }
        //}

        //private static void DataPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        //{
        //    CarZonesError control = sender as CarZonesError;
        //    if (control == null) return;
        //    control._data = (List<DISP_Car>)e.NewValue;
        //    control.CalculateDisplayed();
        //}

        //private void CalculateDisplayed()
        //{
            
        //}
    }
}
