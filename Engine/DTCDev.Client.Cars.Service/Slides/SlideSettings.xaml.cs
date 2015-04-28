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
    /// Interaction logic for SlideSettings.xaml
    /// </summary>
    public partial class SlideSettings : UserControl
    {
        public SlideSettings()
        {
            InitializeComponent();
        }

        public event EventHandler ClickWorksSettings;
        public event EventHandler ClickPersonalSettings;

        private void brdrWorksSettings_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (ClickWorksSettings != null)
                ClickWorksSettings(this, new EventArgs());
        }

        private void brdrPersonalSettings_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (ClickPersonalSettings != null)
                ClickPersonalSettings(this, new EventArgs());
        }
    }
}
