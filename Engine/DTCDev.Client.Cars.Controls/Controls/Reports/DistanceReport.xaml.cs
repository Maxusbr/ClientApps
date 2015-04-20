﻿using System;
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
using DTCDev.Client.Cars.Controls.ViewModels.Reports;

namespace DTCDev.Client.Cars.Controls.Controls.Reports
{
    /// <summary>
    /// Interaction logic for DistanceReport.xaml
    /// </summary>
    public partial class DistanceReport : UserControl
    {
        DistanceReportViewModel model = new DistanceReportViewModel();

        public DistanceReport()
        {
            InitializeComponent();
            this.DataContext = model;
        }
    }
}
