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

namespace KOT.Common.Controls
{
    public sealed partial class AutoEVMControl : UserControl
    {
        public AutoEVMControl()
        {
            this.InitializeComponent();
        }

        private void EnebleSelect_OnClose(object sender)
        {
            var dt = sender as DateWeekSelectControl;
            if(dt == null) return;
            tbEneble.Text = dt.Date.ToString("d");
            FlyoutEnable.Hide();
        }


        private void DisableSelect_OnClose(object sender)
        {
            var dt = sender as DateWeekSelectControl;
            if (dt == null) return;
            tbDisable.Text = dt.Date.ToString("d");
            FlyoutDisable.Hide();
        }

    }
}
