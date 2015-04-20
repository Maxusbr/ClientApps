using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DTCDev.Client
{
    /// <summary>
    /// Interaction logic for BusyControl.xaml
    /// </summary>
    public partial class BusyControl : UserControl
    {
        public BusyControl()
        {
            InitializeComponent();
        }

        private void Storyboard_Completed(object sender, EventArgs e)
        {
            StartSB();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }
            else
                StartSB();
        }

        private string _text = "";
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                textBlock.Text = _text;
            }
        }

        private void StartSB()
        {
            Storyboard sb = this.FindResource("stbAnimate") as Storyboard;
            sb.Begin();
        }
    }
}
