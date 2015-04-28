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

namespace DTCDev.Client.Cars.Service.SideMenu.SideMenuHelp
{
    /// <summary>
    /// Interaction logic for HeaderLine.xaml
    /// </summary>
    public partial class HeaderLine : UserControl
    {
        public HeaderLine()
        {
            InitializeComponent();
        }

        public event EventHandler CloseClick;

        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (CloseClick != null)
                CloseClick(this, new EventArgs());
        }

        public string HeaderText
        {
            get { return txtHeader.Text; }
            set { txtHeader.Text = value; }
        }
    }
}
