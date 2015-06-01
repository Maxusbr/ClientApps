﻿using DTCDev.Client.Cars.Service.Engine.Controls.ViewModels.Cars;
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
    /// Interaction logic for CarSettingsView.xaml
    /// </summary>
    public partial class CarSettingsView : UserControl
    {
        public CarSettingsView()
        {
            InitializeComponent();
            this.DataContext = new CarSettingsViewModel();
        }
    }
}