using System;
using System.Collections.Generic;
using System.IO;
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
using System.Xml.Serialization;

namespace DTCDev.Client.Cars.Service.SideMenu
{
    /// <summary>
    /// Interaction logic for SM_Help.xaml
    /// </summary>
    public partial class SM_Help : UserControl
    {
        public SM_Help()
        {
            InitializeComponent();
        }

        public event EventHandler CloseClick;

        private void headerLine_CloseClick(object sender, EventArgs e)
        {
            if(CloseClick!=null)
                CloseClick(this, new EventArgs());
        }
    }
}
