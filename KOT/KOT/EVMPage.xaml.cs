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
    public sealed partial class EVMPage : Page
    {
        public EVMPage()
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
            var kothandler = MapSourceHandler.Instance;
            AlarmHandler.Instance.Alarm += Instance_Alarm;
        }

        void Instance_Alarm(object sender, PropertyChangedEventArgs e)
        {
            var alarm = new AlarmControl(e.PropertyName);
            alarm.Close += alarm_Close;
            grdContext.Children.Add(alarm);
            AlarmBorder.Visibility = Visibility.Visible;
        }

        private void alarm_Close(object sender, EventArgs e)
        {
            grdContext.Children.Remove(sender as UIElement);
            if(grdContext.Children.Count == 0)
                AlarmBorder.Visibility = Visibility.Collapsed;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MainMenuControl.HideMenu += MainMenuControl_HideMenu;
            MainMenuControl.Height = this.ActualHeight;
        }

        private void MainMenuControl_HideMenu(object sender, EventArgs e)
        {
            var fl = FlyoutBase.GetAttachedFlyout(this);
            if (fl != null)
                fl.Hide();
            var type = sender as Type;
            if (type == null || type == typeof(EVMPage)) return;
            Frame.Navigate(type);
        }

        private void Tools_Click(object sender, RoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout(this);
        }



    }
}
