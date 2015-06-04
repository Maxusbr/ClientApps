using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
using KOT.DataModel.ViewModel;

namespace KOT
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class BudgetPage : Page
    {
        private BudgetViewModel _vm;

        public BudgetPage()
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

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _vm = DataContext as BudgetViewModel;
            MainMenuControl.HideMenu += MainMenuControl_HideMenu;
        }

        private void MainMenuControl_HideMenu(object sender, EventArgs e)
        {
            FlyoutMenu.Hide();
            var type = sender as Type;
            if (type == null || type == typeof(BudgetPage)) return;
            Frame.Navigate(type);
        }

        private async void All_Click(object sender, RoutedEventArgs e)
        {
            var butt = sender as ToggleMenuFlyoutItem;
            if (butt == null) return;
            Month1.IsChecked = butt.Name == Month1.Name;
            Month3.IsChecked = butt.Name == Month3.Name;
            Month6.IsChecked = butt.Name == Month6.Name;
            Year.IsChecked = butt.Name == Year.Name;
            await UpdateSelectedDate();
            
        }

        private async Task UpdateSelectedDate()
        {
            if (_vm == null) return;
            if (Month1.IsChecked)
            {
                await _vm.Update(1);
                return;
            }
            if (Month3.IsChecked)
            {
                await _vm.Update(3);
                return;
            }
            if (Month6.IsChecked)
            {
                await _vm.Update(6);
                return;
            }
            if (Year.IsChecked)
            {
                await _vm.Update(12);
                return;
            }
        }

        private void BudgetMonthControl_OnClick(object sender)
        {
            var control = sender as BudgetMonthControl;
            if(control == null || _vm == null) return;
            _vm.UpdateSelected(control.IdClass);
        }

        private void DetailBudgetControl_OnClick(object sender)
        {
            var control = sender as DetailBudgetControl;
            if (control == null || _vm == null) return;
            _vm.UpdateSelected(control.IdClass);
        }

        private void DetailBudgetControl_OnClose(object sender, EventArgs e)
        {
            if(_vm == null) return;
            _vm.CloseDetail();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof (InvoicePage), _vm);
        }
    }
}
