using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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

            DefaultViewModel["Center"] = new Geopoint(new BasicGeoposition { Altitude = 0, Latitude = 55.75, Longitude = 37.62 });
            DefaultViewModel["ServiceToken"] = "dIjWUGuzClDyimeHLXa9bw";            
        }

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            
        }

        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            DefaultViewModel["Source"] = DataSource.Instance;
            Map.Children.Add(DataSource.Phone);
            Map.Children.Add(DataSource.Kot);

            DataSource.ServicePoints.CollectionChanged += ServicePoints_CollectionChanged;
            DataSource.GetMapElements();
            //await AddMapElements();
            //AddServiceElement(new PlacesModel { Latitude = 557500, Longitude = 376300 });

            MapControls.ItemsSource = DataSource.ServicePoints;
            

        }

        async void ServicePoints_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var positions = DataSource.ServicePoints.Select(el => el.Location.Position).ToList();
            positions.Add(DataSource.Kot.Location.Position);
            var box = GeoboundingBox.TryCompute(positions);

            //Map.Center = new Geopoint(box.Center);
            await Map.TrySetViewBoundsAsync(box, null, MapAnimationKind.Default);
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
            if (butt == null)  return;
            All.IsChecked = butt.Name == All.Name;
            Gas.IsChecked = butt.Name == Gas.Name;
            Wash.IsChecked = butt.Name == Wash.Name;
            Shops.IsChecked = butt.Name == Shops.Name;
            Tire.IsChecked = butt.Name == Tire.Name;

        }

        private async void AppBarToggleButton_Click(object sender, RoutedEventArgs e)
        {
            var but = sender as AppBarToggleButton;
            if(but == null) return;
            Map.Routes.Clear();
            AddRoute(but.IsChecked ?? false);
        }

        private async void AddRoute(bool p)
        {
            Busy.IsActive = true;
            var routeResult = await DataSource.UpdatePhone(p);
            Busy.IsActive = false;
            if (!p || routeResult == null || routeResult.Status != MapRouteFinderStatus.Success) return;
            var viewOfRoute = new MapRouteView(routeResult.Route)
            {
                RouteColor = Color.FromArgb(255, 233, 30, 99),
                OutlineColor = Colors.Transparent
            };

            Map.Routes.Add(viewOfRoute);

            await Map.TrySetViewBoundsAsync(
                routeResult.Route.BoundingBox,
                null,
                MapAnimationKind.Bow);
        }

        private async void btCenter_Click(object sender, RoutedEventArgs e)
        {
            await Map.TrySetViewAsync(DataSource.Kot.Location);
        }

        private void AddServiceElement(PlacesModel model)
        {
            var pin = new ServiceElement("", new Geopoint(new BasicGeoposition
            {
                Altitude = 0,
                Latitude = model.Latitude / 10000.0,
                Longitude = model.Longitude / 10000.0
            })) { DataContext = model };
            pin.Click += pin_Click;
            MapControl.SetLocation(pin, pin.Location);
            Map.Children.Add(pin);
        }

        private void pin_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
