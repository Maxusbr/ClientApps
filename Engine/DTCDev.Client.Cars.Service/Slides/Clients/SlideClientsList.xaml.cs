﻿using DTCDev.Client.Cars.Service.Engine.Controls.ViewModels.Clients;
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

namespace DTCDev.Client.Cars.Service.Slides.Clients
{
    /// <summary>
    /// Interaction logic for SlideClientsList.xaml
    /// </summary>
    public partial class SlideClientsList : UserControl
    {
        public SlideClientsList()
        {
            InitializeComponent();
            this.DataContext = new SlideClientsListViewModel();
        }
    }
}
