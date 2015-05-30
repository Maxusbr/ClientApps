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
    public sealed partial class MainMenuControl : UserControl
    {
        public MainMenuControl()
        {
            this.InitializeComponent();
        }

        public event EventHandler HideMenu;
        public event EventHandler ShowAlarm;

        private void OnShowAlarm()
        {
            if (ShowAlarm != null) ShowAlarm(this, EventArgs.Empty);
        }

        private void OnHideMenu(Type type)
        {
            if (HideMenu != null) HideMenu(type, EventArgs.Empty);
        }

        private void Map_Checked(object sender, RoutedEventArgs e)
        {
            //TODO сохранить в DataSource активный пункт меню & NavigateTo MapControl
            OnHideMenu(typeof(MainPage));
        }

        private void TO_Checked(object sender, RoutedEventArgs e)
        {
            //TODO сохранить в DataSource активный пункт меню & NavigateTo TOControl 
            OnHideMenu(typeof(TOPage));
        }

        private void PC_Checked(object sender, RoutedEventArgs e)
        {
            //TODO сохранить в DataSource активный пункт меню & NavigateTo PCControl
            OnHideMenu(typeof(EVMPage));
        }

        private void Money_Checked(object sender, RoutedEventArgs e)
        {
            //TODO сохранить в DataSource активный пункт меню & NavigateTo MoneyControl
            OnHideMenu(typeof(BudgetPage));
        }

        private void Alarm_Checked(object sender, RoutedEventArgs e)
        {
            //TODO сохранить в DataSource активный пункт меню &  Send Alarm_Checked
            OnShowAlarm();
        }

        private void Settings_Checked(object sender, RoutedEventArgs e)
        {
            //TODO сохранить в DataSource активный пункт меню & NavigateTo SettingsControl 
            //OnHideMenu(typeof(MainPage));
        }

        private void About_Checked(object sender, RoutedEventArgs e)
        {
            //TODO сохранить в DataSource активный пункт меню & NavigateTo AboutControl
            //OnHideMenu(typeof(MainPage));
        }

        private void AddProfile_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void ListView_Tapped(object sender, TappedRoutedEventArgs e)
        {
            appBarToggleButton.IsChecked = false;
        }
    }
}
