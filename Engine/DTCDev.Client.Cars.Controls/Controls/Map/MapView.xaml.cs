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
using DTCDev.Client.Cars.Controls.ViewModels.Map;

namespace DTCDev.Client.Cars.Controls.Controls.Map
{
    /// <summary>
    /// Interaction logic for MapView.xaml
    /// </summary>
    public partial class MapView : UserControl
    {
        public MapView()
        {
            InitializeComponent();
            DataContext = new MapViewModel(Dispatcher);
        }


        private void MapItemTouchDown(object sender, TouchEventArgs e)
        {
            /*var mapItem = (DTCDev.Cars.Controls.Map.MapItem)sender;
            mapItem.IsSelected = !mapItem.IsSelected;
            e.Handled = true;*/
        }
    }
}
