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
    /// Interaction logic for SlideSettingsWorkDic.xaml
    /// </summary>
    public partial class SlideSettingsWorkDic : UserControl
    {
        SettingsWorkDicViewModel _vm = new SettingsWorkDicViewModel();

        public SlideSettingsWorkDic()
        {
            InitializeComponent();
            this.DataContext = _vm;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            grdAddWork.Visibility = Visibility.Collapsed;
            grdworks.Visibility = Visibility.Visible;
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            _vm.SelectedWorkTree = (WorkTreeModel)e.NewValue;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            grdAddWork.Visibility = Visibility.Visible;
            grdworks.Visibility = Visibility.Collapsed;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            grdAddWork.Visibility = Visibility.Collapsed;
            grdworks.Visibility = Visibility.Visible;
        }
    }
}
