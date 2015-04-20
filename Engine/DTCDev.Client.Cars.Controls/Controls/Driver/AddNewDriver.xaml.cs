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
using System.Windows.Shapes;
using DTCDev.Models.CarsSending;

namespace DTCDev.Client.Cars.Controls.Controls.Driver
{
    /// <summary>
    /// Interaction logic for AddNewDriver.xaml
    /// </summary>
    public partial class AddNewDriver : Window
    {
        public AddNewDriver()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            Driver.Name = txtName.Text;
            Driver.Phone = txtPhone.Text;
            Driver.SName = txtSecondName.Text;
            Driver.FName = txtFamilyName.Text;
            Driver.CatA = ((bool)cbx1.IsChecked) ? 1 : 0;
            Driver.CatB = ((bool)cbx2.IsChecked) ? 1 : 0;
            Driver.CatC = ((bool)cbx3.IsChecked) ? 1 : 0;
            Driver.CatD = ((bool)cbx4.IsChecked) ? 1 : 0;
            Driver.CatE = ((bool)cbx5.IsChecked) ? 1 : 0;
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private DriverModel _driver = new DriverModel();
        public DriverModel Driver
        {
            get { return _driver; }
            set { _driver = value; }
        }
    }
}
