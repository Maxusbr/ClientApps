using DTCDev.Client.Cars.Engine.DisplayModels;
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

        public bool IsSelected
        {
            get { return (bool)GetValue(CarSelectedProperty); }
            set { SetValue(CarSelectedProperty, value); }
        }

        private static readonly DependencyProperty CarSelectedProperty = DependencyProperty.Register("IsSelected",
            typeof(bool),
            typeof(MapViewElement),
            new PropertyMetadata(false, OnCarChanged));

        private static void OnCarChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var map = sender as MapViewElement;
            if (map != null) map.CarChanged(sender, e);
        }

        private void CarChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                VisualStateManager.GoToState(this, "Selected", true);
            }
            else
            {
                VisualStateManager.GoToState(this, "Normal", false);
            }
        }

        private bool _isClicked = false;

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
            if (_isClicked)
                detail.Visibility = Visibility.Collapsed;
            else
                detail.Visibility = Visibility.Visible;
            _isClicked = ! _isClicked;
        }
    }
}
