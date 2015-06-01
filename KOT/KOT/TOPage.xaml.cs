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
using KOT.Common.Controls;
using KOT.DataModel.Handlers;

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

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CalendarListToggle.IsChecked = Pivot.SelectedIndex == 1;
            if (Pivot.SelectedIndex < 2)
            {
                Lable.Text = "TO";
                Tools.Visibility = History.Visibility= CalendarListToggle.Visibility = BarButton.Visibility = Visibility.Visible;
                BackButton.Visibility = More.Visibility = Visibility.Collapsed;
            }
            else
            {
                Lable.Text = "История TO";
                Tools.Visibility = History.Visibility = CalendarListToggle.Visibility = BarButton.Visibility = Visibility.Collapsed;
                BackButton.Visibility = More.Visibility = Visibility.Visible;
            }
        }

        private void All_Click(object sender, RoutedEventArgs e)
        {
            var butt = sender as ToggleMenuFlyoutItem;
            if (butt == null) return;
            AllMonth.IsChecked = butt.Name == AllMonth.Name;
            Month1.IsChecked = butt.Name == Month1.Name;
            Month3.IsChecked = butt.Name == Month3.Name;
            Month6.IsChecked = butt.Name == Month6.Name;
            SelectDate.IsChecked = butt.Name == SelectDate.Name;
            UpdateSelectedDate();
        }

        private void UpdateSelectedDate()
        {
            var dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var nulldate = new DateTime(1, 1, 1);
            if (AllMonth.IsChecked)
            {
                HistoryWorkHandler.UpdateDate(nulldate, dt);
                return;
            }
            if (Month1.IsChecked)
            {
                HistoryWorkHandler.UpdateDate(dt.AddMonths(-1), dt);
                return;
            }
            if (Month3.IsChecked)
            {
                HistoryWorkHandler.UpdateDate(dt.AddMonths(-3), dt);
                return;
            }
            if (Month6.IsChecked)
            {
                HistoryWorkHandler.UpdateDate(dt.AddMonths(-6), dt);
                return;
            }
            FlyOut.Visibility = Visibility.Visible;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Pivot.SelectedIndex = 0;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            FlyOut.Visibility = Visibility.Collapsed;
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            FlyOut.Visibility = Visibility.Collapsed;
            DateTime start;
            DateTime end;
            if (!DateTime.TryParse(EndDate.Text, out end))
                end = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            if (DateTime.TryParse(StartDate.Text, out start))
                HistoryWorkHandler.UpdateDate(start, end); 
        }

        private void History_Click(object sender, RoutedEventArgs e)
        {
            Pivot.SelectedIndex = 2;
        }

        private void StartDateSelect_OnClose(object sender)
        {
            var dt = sender as DateWeekSelectControl;
            if (dt == null) return;
            StartDate.Text = dt.Date.ToString("d");
            StartDateSelect.Hide();
        }

        private void EndDateSelect_OnClose(object sender)
        {
            var dt = sender as DateWeekSelectControl;
            if (dt == null) return;
            EndDate.Text = dt.Date.ToString("d");
            EndDateSelect.Hide();
        }
    }
}
