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

namespace KOT
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class NotificationPage : Page
    {
        public NotificationPage()
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
            Frame.GoBack();
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
                AlarmHandler.UpdateDate(nulldate, dt);
                return;
            }
            if (Month1.IsChecked)
            {
                AlarmHandler.UpdateDate(dt.AddMonths(-1), dt);
                return;
            }
            if (Month3.IsChecked)
            {
                AlarmHandler.UpdateDate(dt.AddMonths(-3), dt);
                return;
            }
            if (Month6.IsChecked)
            {
                AlarmHandler.UpdateDate(dt.AddMonths(-6), dt);
                return;
            }
            FlyOut.Visibility = Visibility.Visible;
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

        private void CancelDate_Click(object sender, RoutedEventArgs e)
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
                AlarmHandler.UpdateDate(start, end);
        }
    }
}
