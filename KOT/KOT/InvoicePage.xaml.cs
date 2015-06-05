using System;
using System.Collections.Generic;
using System.ComponentModel;
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

// Документацию по шаблону элемента пустой страницы см. по адресу http://go.microsoft.com/fwlink/?LinkID=390556
using KOT.Common.Controls;
using KOT.DataModel.Handlers;
using KOT.DataModel.ViewModel;

namespace KOT
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class InvoicePage : Page
    {
        private int _selectedCategoryId;
        public InvoicePage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Вызывается перед отображением этой страницы во фрейме.
        /// </summary>
        /// <param name="e">Данные события, описывающие, каким образом была достигнута эта страница.
        /// Этот параметр обычно используется для настройки страницы.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var vm = e.Parameter as BudgetViewModel;
            if (vm == null) return;
            _selectedCategoryId = vm.SelectedCategoryId;
            switch (vm.SelectedCategoryId)
            {
                case 1:
                    GasButton_Click(null, null);
                    break;
                case 2:
                    ParkingButton_Click(null, null);
                    break;
                case 3:
                    CarwashButton_Click(null, null);
                    break;
                case 4:
                    ShopButton_Click(null, null);
                    break;
                case 5:
                    RashodButton_Click(null, null);
                    break;
            }
            AlarmHandler.Instance.Alarm += Instance_Alarm;
        }

        private void Instance_Alarm(object sender, PropertyChangedEventArgs e)
        {
            var alarm = new AlarmControl(e.PropertyName);
            alarm.Close += alarm_Close;
            grdContext.Children.Add(alarm);
            AlarmBorder.Visibility = Visibility.Visible;
        }

        private void alarm_Close(object sender, EventArgs e)
        {
            grdContext.Children.Remove(sender as UIElement);
            if (grdContext.Children.Count == 0)
                AlarmBorder.Visibility = Visibility.Collapsed;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            tbDate.Visibility = Visibility.Collapsed;
            Frame.GoBack();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            //TODO SaveInvoice
            DateTime dt;
            double val;
            if (!DateTime.TryParse(tbDate.Text, out dt) || !double.TryParse(tbCost.Text, out val)) return;
            BudgetHandler.SaveInvoice(_selectedCategoryId, dt, val, tbComment.Text);
            tbDate.Visibility = Visibility.Collapsed;
            Frame.GoBack();
        }

        private void GasButton_Click(object sender, RoutedEventArgs e)
        {
            _selectedCategoryId = 1;
            SetIcon(GasButton.Icon as BitmapIcon);
            SetChecked(GasButton.Name);
        }

        private void ParkingButton_Click(object sender, RoutedEventArgs e)
        {
            _selectedCategoryId = 2;
            SetIcon(ParkingButton.Icon as BitmapIcon);
            SetChecked(ParkingButton.Name);
        }

        private void CarwashButton_Click(object sender, RoutedEventArgs e)
        {
            _selectedCategoryId = 3;
            SetIcon(CarwashButton.Icon as BitmapIcon);
            SetChecked(CarwashButton.Name);
        }

        private void ShopButton_Click(object sender, RoutedEventArgs e)
        {
            _selectedCategoryId = 4;
            SetIcon(ShopButton.Icon as BitmapIcon);
            SetChecked(ShopButton.Name);
        }

        private void RashodButton_Click(object sender, RoutedEventArgs e)
        {
            _selectedCategoryId = 5;
            SetIcon(RashodButton.Icon as BitmapIcon);
            SetChecked(RashodButton.Name);
        }

        private void SetIcon(BitmapIcon icon)
        {
            if (icon != null)
                ccBox.Icon = new BitmapIcon { UriSource = icon.UriSource };
            tbDate.Visibility = Visibility.Visible;
        }

        private void SetChecked(string name)
        {
            GasButton.IsChecked = name == GasButton.Name;
            ParkingButton.IsChecked = name == ParkingButton.Name;
            CarwashButton.IsChecked = name == CarwashButton.Name;
            ShopButton.IsChecked = name == ShopButton.Name;
            RashodButton.IsChecked = name == RashodButton.Name;

        }

        private void DateSelect_OnClose(object sender)
        {
            DateFlyout.Hide();
            var dt = sender as DateWeekSelectControl;
            if (dt == null) return;
            tbDate.Text = dt.Date.ToString("d");
        }

        private void Update()
        {
            DateTime dt;
            double val;
            OK.IsEnabled = _selectedCategoryId > 0 && DateTime.TryParse(tbDate.Text, out dt) && double.TryParse(tbCost.Text, out val);
        }

        private void tbCost_TextChanged(object sender, TextChangedEventArgs e)
        {
            Update();
        }

        private void tbDate_TextChanged(object sender, TextChangedEventArgs e)
        {
        	Update();
        }
    }
}
