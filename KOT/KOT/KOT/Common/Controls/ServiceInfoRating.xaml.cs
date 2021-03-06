﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Шаблон элемента пользовательского элемента управления задокументирован по адресу http://go.microsoft.com/fwlink/?LinkId=234236
using KOT.DataModel;
using KOT.DataModel.Handlers;
using KOT.DataModel.ViewModel;

namespace KOT.Common.Controls
{
    public sealed partial class ServiceInfoRating : ContentControl
    {
        private ServiceInfoViewModel _vm;
        public ServiceInfoRating()
        {
            this.InitializeComponent();
            _vm = DataContext as ServiceInfoViewModel;
            toggleButton.Click += toggleButton_Checked;
        }

        public event EventHandler Checked;
        private void OnSChecked(object sender)
        {
            if (Checked != null) Checked(sender, EventArgs.Empty);
        }

        async void toggleButton_Checked(object sender, RoutedEventArgs e)
        {
            OnSChecked(sender);
            _vm.Details = await MapsElementsDataHandler.GetDetail(_vm.Model);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (_vm == null) return;
        }

        internal void UpdateDataContext(DataModel.Model.PlacesModel model)
        {
            DataContext = _vm = new ServiceInfoViewModel(model);
        }

        public bool IsChecked
        {
            get { return toggleButton.IsChecked ?? false; }
            set { toggleButton.IsChecked = value; }
        }
    }
}
