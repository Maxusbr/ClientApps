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

namespace DTCDev.Client.Cars.Controls.Controls.Map
{
    /// <summary>
    /// Interaction logic for MapViewElement.xaml
    /// </summary>
    public partial class MapViewElement : UserControl
    {
        public MapViewElement()
        {
            InitializeComponent();
        }

        private bool _isClicked;

        private void UserControl_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            detail.Visibility = Visibility.Visible;
        }

        private void UserControl_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
        	if(!_isClicked)
                detail.Visibility = Visibility.Collapsed;
        }

        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            detail.Visibility = Visibility.Visible;
            _isClicked = true;
        }

        private void UserControl_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            detail.Visibility = Visibility.Collapsed;
            _isClicked = false;
        }
    }
}
