using DTCDev.Client.Cars.Service.Engine.Controls.ViewModels.Cars;
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

namespace DTCDev.Client.Cars.Service.Controls.Car
{
    /// <summary>
    /// Interaction logic for CarAddWorkView.xaml
    /// </summary>
    public partial class CarAddWorkView : UserControl
    {
        public CarAddWorkView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            grdAdd.Visibility = Visibility.Visible;
            CarAddWork_WorkList list = new CarAddWork_WorkList();
            list.CloseMe += list_CloseMe;
            list.DataContext = this.DataContext;
            grdAdd.Children.Add(list);
            grdMain.Visibility = Visibility.Collapsed;
        }

        void list_CloseMe(object sender, EventArgs e)
        {
            grdAdd.Children.Clear();
            grdAdd.Visibility = Visibility.Collapsed;
            grdMain.Visibility = Visibility.Visible;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            grdAdd.Visibility = Visibility.Collapsed;
        }
    }
}
