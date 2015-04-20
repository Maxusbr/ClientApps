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
using DTCDev.Client.Cars.Controls.ViewModels.Driver;
using DTCDev.Client.Cars.Engine.Handlers.Cars;

namespace DTCDev.Client.Cars.Controls.Controls.Driver
{
    /// <summary>
    /// Interaction logic for DriversList.xaml
    /// </summary>
    public partial class DriversList : UserControl
    {
        public DriversList()
        {
            InitializeComponent();
            this.DataContext = new DriversWinViewModel();
        }

        private void pnlAdd_AddClick(object sender, EventArgs e)
        {
            AddNewDriver control = new AddNewDriver();
            control.ShowDialog();
            if (control.DialogResult == true)
                DriverHandler.Instance.EditDriver(control.Driver);
        }
    }
}
