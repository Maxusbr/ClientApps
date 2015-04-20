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

namespace DTCDev.Client.Window.SlideNavigation
{
    /// <summary>
    /// Interaction logic for SlideFrameControl.xaml
    /// </summary>
    public partial class SlideFrameControl : UserControl
    {
        public SlideFrameControl()
        {
            InitializeComponent();
        }

        public string Name { get; private set; }

        public UIElement DisplayedControl { get; private set; }

        public void Init(string name, UIElement control)
        {
            Name = name;
            DisplayedControl = control;
            grdContent.Children.Add(control);
        }
    }
}
