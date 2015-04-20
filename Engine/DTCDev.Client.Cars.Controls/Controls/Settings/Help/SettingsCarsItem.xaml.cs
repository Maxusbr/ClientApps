using DTCDev.Models.CarBase.CarStatData;
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

namespace DTCDev.Client.Cars.Controls.Controls.Settings.Help
{
    /// <summary>
    /// Interaction logic for SettingsCarsItem.xaml
    /// </summary>
    public partial class SettingsCarsItem : UserControl
    {
        public SettingsCarsItem()
        {
            InitializeComponent();
        }

        public SettingsCarsItem(CarSettings model)
        {
            InitializeComponent();
            _model = model;
        }

        private CarSettings _model;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            txtName.Text = _model.CarName;
        }
    }
}
