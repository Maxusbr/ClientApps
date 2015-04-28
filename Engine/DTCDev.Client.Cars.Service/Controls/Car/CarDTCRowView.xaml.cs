using DTCDev.Models.CarsSending.Car;
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
    /// Interaction logic for CarDTCRowView.xaml
    /// </summary>
    public partial class CarDTCRowView : UserControl
    {
        public CarDTCRowView()
        {
            InitializeComponent();
        }

        CarDTCHistoryModel _model;

        public CarDTCRowView(CarDTCHistoryModel model)
        {
            InitializeComponent();
            _model = model;
            DisplayData();
        }

        private void DisplayData()
        {
            if (_model == null)
                return;
            else
            {
                txtDate.Text = _model.Date.D.ToString() + "." + _model.Date.M.ToString() + "." + _model.Date.Y.ToString() +
                    " " + _model.Date.hh.ToString() + ":" + _model.Date.mm.ToString() + ":" + _model.Date.ss.ToString();
                txtPID.Text = _model.MessageCode;
                txtText.Text = _model.Text;
            }
        }
    }
}
