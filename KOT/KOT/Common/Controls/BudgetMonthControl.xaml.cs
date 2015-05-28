using System;
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
    public sealed partial class BudgetMonthControl : UserControl
    {
        public BudgetMonthControl()
        {
            this.InitializeComponent();
        }

        public int IdClass { get; set; }

        private void GasButton_Click(object sender, RoutedEventArgs e)
        {
            IdClass = 0;
            OnClose();
        }

        private void ParkingButton_Click(object sender, RoutedEventArgs e)
        {
            IdClass = 1;
            OnClose();
        }

        private void CarwashButton_Click(object sender, RoutedEventArgs e)
        {
            IdClass = 2;
            OnClose();
        }

        private void ShopButton_Click(object sender, RoutedEventArgs e)
        {
            IdClass = 3;
            OnClose();
        }

        private void RashodButton_Click(object sender, RoutedEventArgs e)
        {
            IdClass = 4;
            OnClose();
        }

        public delegate void CustomClickEvent(object sender);
        public event CustomClickEvent Click;

        private void OnClose()
        {
            CustomClickEvent handler = Click;
            if (handler != null) handler(this);
        }
    }
}
