using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Services.Maps;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using KOT.Common;
// Документацию по шаблону элемента пустой страницы см. по адресу http://go.microsoft.com/fwlink/?LinkID=390556
using KOT.Common.Controls;
using KOT.DataModel;
using KOT.DataModel.Handlers;
using KOT.DataModel.Model;
using KOT.DataModel.ViewModel;


namespace KOT
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly NavigationHelper navigationHelper;
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();
        private readonly ResourceLoader resourceLoader = ResourceLoader.GetForCurrentView("Resources");

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
            CarsHandler.SelectionChanged += CarsHandler_SelectionChanged;
            Map.SelectionPoinChanged += Map_SelectionPoinChanged;
        }

        void CarsHandler_SelectionChanged(object sender, EventArgs e)
        {
            PhoneToggle.IsChecked = false;
        }

        private void Map_SelectionPoinChanged(object sender, EventArgs e)
        {
            var model = sender as PlacesModel;
            if (model == null) return;
            ServiceInfo.UpdateDataContext(model);
            ServiceInfo.IsChecked = false;
            ServiceInfo.Visibility = Visibility.Visible;
            if (AlarmLine.Visibility == Visibility.Visible) AlarmLine.Visibility = Visibility.Collapsed;
        }

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {

        }

        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            DefaultViewModel["Source"] = MapSourceHandler.Instance;
            MainMenuControl.HideMenu += MainMenuControl_HideMenu;
            MainMenuControl.ShowAlarm += MainMenuControl_ShowAlarm;
        }

        private void MainMenuControl_ShowAlarm(object sender, EventArgs e)
        {
            FlyoutMenu.Hide();
            AlarmLine.Visibility = Visibility.Visible;
        }

        void MainMenuControl_HideMenu(object sender, EventArgs e)
        {
            FlyoutMenu.Hide();
            var type = sender as Type;
            if (type == null || type == typeof(MainPage)) return;
            Frame.Navigate(type);
        }

        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Вызывается перед отображением этой страницы во фрейме.
        /// </summary>
        /// <param name="e">Данные события, описывающие, каким образом была достигнута эта страница.
        /// Этот параметр обычно используется для настройки страницы.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
            AlarmHandler.Instance.Alarm += Instance_Alarm;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddAppBarButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void All_Click(object sender, RoutedEventArgs e)
        {
            var butt = sender as ToggleMenuFlyoutItem;
            if (butt == null) return;
            All.IsChecked = butt.Name == All.Name;
            Gas.IsChecked = butt.Name == Gas.Name;
            Wash.IsChecked = butt.Name == Wash.Name;
            Shops.IsChecked = butt.Name == Shops.Name;
            Tire.IsChecked = butt.Name == Tire.Name;

        }

        private void AppBarToggleButton_Click(object sender, RoutedEventArgs e)
        {
            var but = sender as AppBarToggleButton;
            if (but == null) return;
            Map.AddRoute(but.IsChecked ?? false);
        }

        private void btCenter_Click(object sender, RoutedEventArgs e)
        {
            Map.SetCenterLocation(null);
        }

        private void ServiceInfo_Checked(object sender, EventArgs e)
        {
            var but = sender as ToggleButton;
            if (but == null) return;
            ServiceInfo.VerticalAlignment = (but.IsChecked ?? false) ? VerticalAlignment.Stretch : VerticalAlignment.Bottom;
            BackButton.Visibility = (but.IsChecked ?? false) ? Visibility.Visible : Visibility.Collapsed;
            Tools.Visibility = !(but.IsChecked ?? false) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            BackButton.Visibility = ServiceInfo.Visibility = Visibility.Collapsed;
            Tools.Visibility = Visibility.Visible;
            ServiceInfo.VerticalAlignment = VerticalAlignment.Bottom;
            ServiceInfo.IsChecked = false;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MainMenuControl.Height = this.ActualHeight;
        }

        private void LightsOn_Click(object sender, RoutedEventArgs e)
        {
            UpdateAlarmLine();
            if(!LightsOn.IsChecked ?? false) return;
            var alarm = new AlarmControl("IsLightsOn");
            alarm.Close += alarm_Close;
            grdContext.Children.Add(alarm);
        }

        private void DoorClosed_Click(object sender, RoutedEventArgs e)
        {
            UpdateAlarmLine();
            if (!DoorClosed.IsChecked ?? false) return;
            var alarm = new AlarmControl("IsDoorClosed");
            alarm.Close += alarm_Close;
            grdContext.Children.Add(alarm);

        }

        private void Evacuation_Click(object sender, RoutedEventArgs e)
        {
            UpdateAlarmLine();
            if (!Evacuation.IsChecked ?? false) return;
            var alarm = new AlarmControl("IsEvacuation");
            alarm.Close += alarm_Close;
            grdContext.Children.Add(alarm);
        }

        private void Alarm_Click(object sender, RoutedEventArgs e)
        {
            UpdateAlarmLine();
            if (!Alarm.IsChecked ?? false) return;
            var alarm = new AlarmControl("AlarmLevel");
            alarm.Close += alarm_Close;
            grdContext.Children.Add(alarm);
        }

        private void Instance_Alarm(object sender, PropertyChangedEventArgs e)
        {
            UpdateAlarmLine();
            AlarmLine.Visibility = Visibility.Visible;
        }


        private void alarm_Close(object sender, EventArgs e)
        {
            var alarm = sender as AlarmControl;
            if (alarm != null)
                UpdateModel(alarm.PropertyName);
            grdContext.Children.Remove(alarm);
            if (!IsAlarmed())
                AlarmLine.Visibility = Visibility.Collapsed;
        }

        private void UpdateModel(string propertyName)
        {
            switch (propertyName)
            {
                case "AlarmLevel":
                    AlarmHandler.Instance.CurentModel.AlarmLevel = 0;
                    break;
                case "IsLightsOn":
                    AlarmHandler.Instance.CurentModel.IsLightsOn = 0;
                    break;
                case "IsDoorClosed":
                    AlarmHandler.Instance.CurentModel.IsDoorClosed = 0;
                    break;
                case "IsEvacuation":
                    AlarmHandler.Instance.CurentModel.IsEvacuation = 0;
                    break;
            }
            UpdateAlarmLine();
        }

        private void UpdateAlarmLine()
        {
            Alarm.IsChecked = AlarmHandler.Instance.CurentModel.AlarmLevel != 0;
            LightsOn.IsChecked = AlarmHandler.Instance.CurentModel.IsLightsOn != 0;
            DoorClosed.IsChecked = AlarmHandler.Instance.CurentModel.IsDoorClosed != 0;
            Evacuation.IsChecked = AlarmHandler.Instance.CurentModel.IsEvacuation != 0;
            UpdateLayout();
        }

        private bool IsAlarmed()
        {
            return (Alarm.IsChecked ?? false) || (LightsOn.IsChecked ?? false) 
                || (DoorClosed.IsChecked ?? false) || (Evacuation.IsChecked ?? false);
        }
    }
}
