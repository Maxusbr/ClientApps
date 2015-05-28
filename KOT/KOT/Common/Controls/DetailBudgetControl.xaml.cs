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
using KOT.DataModel.ViewModel;

namespace KOT.Common.Controls
{
    public sealed partial class DetailBudgetControl : UserControl
    {
        public DetailBudgetControl()
        {
            this.InitializeComponent();
        }

        public int IdClass { get; set; }

        private void GasButton_Click(object sender, RoutedEventArgs e)
        {
            IdClass = 0;
            OnClick("GasButton");
        }

        private void ParkingButton_Click(object sender, RoutedEventArgs e)
        {
            IdClass = 1;
            OnClick("ParkingButton");
        }

        private void CarwashButton_Click(object sender, RoutedEventArgs e)
        {
            IdClass = 2;
            OnClick("CarwashButton");
        }

        private void ShopButton_Click(object sender, RoutedEventArgs e)
        {
            IdClass = 3;
            OnClick("ShopButton");
        }

        private void RashodButton_Click(object sender, RoutedEventArgs e)
        {
            IdClass = 4;
            OnClick("RashodButton");
        }

        public delegate void CustomClickEvent(object sender);
        public event CustomClickEvent Click;

        private void OnClick(string name )
        {
            GasButton.IsChecked = name == GasButton.Name;
            ParkingButton.IsChecked = name == ParkingButton.Name;
            CarwashButton.IsChecked = name == CarwashButton.Name;
            ShopButton.IsChecked = name == ShopButton.Name;
            RashodButton.IsChecked = name == RashodButton.Name;
            CustomClickEvent handler = Click;
            if (handler != null) handler(this);
        }

        public event EventHandler Close;
        private void OnClose()
        {
            EventHandler handler = Close;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OnClose();
        }

        private void UserControl_LayoutUpdated(object sender, object e)
        {
            var vm = DataContext as BudgetItemViewModel;
            if (vm == null) return;
            GasButton.IsChecked = vm.SelectedCategoryId == 0;
            ParkingButton.IsChecked = vm.SelectedCategoryId == 1;
            CarwashButton.IsChecked = vm.SelectedCategoryId == 2;
            ShopButton.IsChecked = vm.SelectedCategoryId == 3;
            RashodButton.IsChecked = vm.SelectedCategoryId == 4;
        }
    }
}
