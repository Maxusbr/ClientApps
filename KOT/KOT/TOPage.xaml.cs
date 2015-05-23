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

// Документацию по шаблону элемента пустой страницы см. по адресу http://go.microsoft.com/fwlink/?LinkID=390556
using KOT.Common;

namespace KOT
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class TOPage : Page
    {
        private readonly NavigationHelper navigationHelper;

        public TOPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
        }

        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            MainMenuControl.HideMenu += MainMenuControl_HideMenu;
            MainMenuControl.ShowAlarm += MainMenuControl_ShowAlarm;
        }

        private void MainMenuControl_ShowAlarm(object sender, EventArgs e)
        {
            FlyoutMenu.Hide();
            Frame.Navigate(typeof(MainPage));
        }

        private void MainMenuControl_HideMenu(object sender, EventArgs e)
        {
            FlyoutMenu.Hide();
            var type = sender as Type;
            if (type == null || type == typeof(TOPage)) return;
            Frame.Navigate(type);
        }

        /// <summary>
        /// Вызывается перед отображением этой страницы во фрейме.
        /// </summary>
        /// <param name="e">Данные события, описывающие, каким образом была достигнута эта страница.
        /// Этот параметр обычно используется для настройки страницы.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        private void AppBarToggleButton_Click(object sender, RoutedEventArgs e)
        {
            var bt = sender as AppBarToggleButton;
            if(bt == null) return;
            Pivot.SelectedIndex = (bt.IsChecked ?? false) ? 1 : 0;
        }
    }
}
