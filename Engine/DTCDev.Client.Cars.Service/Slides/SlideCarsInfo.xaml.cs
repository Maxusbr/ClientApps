﻿using DTCDev.Client.Cars.Service.Engine.Controls.ViewModels;
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
    /// Interaction logic for SlideCarsInfo.xaml
    /// </summary>
    public partial class SlideCarsInfo : UserControl
    {
        SlideCarInfoViewModel vm = new SlideCarInfoViewModel();
        public SlideCarsInfo()
        {
            InitializeComponent();
            this.DataContext = vm;
        }
    }
}
