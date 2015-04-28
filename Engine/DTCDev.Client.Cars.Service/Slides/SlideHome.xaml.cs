using DTCDev.Client.Cars.Service.Engine.Controls.ViewModels.Slides;
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

namespace DTCDev.Client.Cars.Service.Slides
{
    /// <summary>
    /// Interaction logic for SlideHome.xaml
    /// </summary>
    public partial class SlideHome : UserControl
    {
        public SlideHome()
        {
            InitializeComponent();
            this.DataContext = new SlideHomeViewModel();
        }

        public event EventHandler ClickCars;

        private void HomeItem_Click(object sender, EventArgs e)
        {
            if (ClickCars != null)
                ClickCars(this, new EventArgs());
        }

        public event EventHandler ClickOrders;

        private void HomeItem_Click_1(object sender, EventArgs e)
        {
            if (ClickOrders != null)
                ClickOrders(this, new EventArgs());
        }
    }
}
