using DTCDev.Client.Cars.Service.Engine.Controls.ViewModels.Settings;
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

namespace DTCDev.Client.Cars.Service.Slides.Settings
{
    /// <summary>
    /// Interaction logic for SlideSettingsWorks.xaml
    /// </summary>
    public partial class SlideSettingsWorks : UserControl
    {
        public SlideSettingsWorks()
        {
            InitializeComponent();
            this.DataContext = new SettingsWorksViewModel();
        }

        public SlideSettingsWorks(string carNumber)
        {
            InitializeComponent();
            this.DataContext = new SettingsWorksViewModel();
        }
    }
}
