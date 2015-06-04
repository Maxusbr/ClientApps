using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
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

// Шаблон элемента пользовательского элемента управления задокументирован по адресу http://go.microsoft.com/fwlink/?LinkId=234236
using KOT.DataModel;
using KOT.DataModel.Handlers;
using KOT.DataModel.ViewModel;

namespace KOT.Common.Controls
{
    public sealed partial class MapUserControl : UserControl
    {
        private readonly ObservableDictionary _defaultViewModel = new ObservableDictionary();
        public ObservableDictionary DefaultViewModel
        {
            get { return _defaultViewModel; }
        }

        public event EventHandler SelectionPoinChanged;
        private void OnSelectionPoinChanged(DataModel.Model.PlacesModel model)
        {
            if (SelectionPoinChanged != null) SelectionPoinChanged(model, EventArgs.Empty);
        }

        public MapUserControl()
        {
            this.InitializeComponent();
            InitializeControls();
            //MapSourceHandler.CenterUpdate += SetCenterLocation;
            CarsHandler.SelectionChanged += CarsHandler_SelectionChanged;
        }

        private async void InstanceOnLoadSourceComplete(object sender, EventArgs eventArgs)
        {
            
        }

        void CarsHandler_SelectionChanged(object sender, EventArgs e)
        {
            Map.Routes.Clear();
            UpdateMapElements();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateMapElements();
        }

        private async void UpdateMapElements()
        {
            await MapSourceHandler.GetMapElements();
            if (MapSourceHandler.ServicePoints.Count == 0)
            {
                SetCenterLocation(MapSourceHandler.Kot.Location);
                return;
            }
            var positions = MapSourceHandler.ServicePoints.Select(el => el.Location.Position).ToList();
            positions.Add(MapSourceHandler.Kot.Location.Position);
            var box = GeoboundingBox.TryCompute(positions);

            await Map.TrySetViewBoundsAsync(box, null, MapAnimationKind.Default);
            SetCenterLocation(new Geopoint(box.Center));
        }

        private void InitializeControls()
        {
            DefaultViewModel["Center"] = MapSourceHandler.CenterMap;
            DefaultViewModel["ServiceToken"] = "7Z4mDC8JoJ496vUs6bQDzQ";
            if (!DesignMode.DesignModeEnabled)
            {
                Map.Children.Add(MapSourceHandler.Phone);
                Map.Children.Add(MapSourceHandler.Kot);
            }
            MapControls.ItemsSource = MapSourceHandler.ServicePoints;
        }

        public async void AddRoute(bool p)
        {
            Map.Routes.Clear();
            Busy.IsActive = true;
            var routeResult = await MapSourceHandler.UpdatePhone(p);
            Busy.IsActive = false;
            if (!p) return;
            if (routeResult == null || routeResult.Status != MapRouteFinderStatus.Success)
            {
                var positions = new List<BasicGeoposition>();
                positions.Add(MapSourceHandler.Kot.Location.Position);
                positions.Add(MapSourceHandler.Phone.Location.Position);
                var box = GeoboundingBox.TryCompute(positions);
                var kot = MapSourceHandler.Kot.Location.Position;
                var phone = MapSourceHandler.Phone.Location.Position;
                var nw = new BasicGeoposition
                {
                    Latitude = Math.Max(kot.Latitude, phone.Latitude),
                    Longitude = Math.Min(kot.Longitude, phone.Longitude)
                };
                var se = new BasicGeoposition
                {
                    Latitude = Math.Min(kot.Latitude, phone.Latitude),
                    Longitude = Math.Max(kot.Longitude, phone.Longitude)
                };

                await Map.TrySetViewBoundsAsync(
                box//new GeoboundingBox(nw, se)
                ,null,
                MapAnimationKind.Bow);
                return;
            }
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

        public async void SetCenterLocation(Geopoint point)
        {
            await Map.TrySetViewAsync(point ?? MapSourceHandler.Kot.Location);
        }

        private void ServiceElement_Click(object sender, RoutedEventArgs e)
        {
            var se = sender as ServiceElement;
            if (se == null) return;
            var vm = se.DataContext as ServicePointViewModel;
            if (vm != null) OnSelectionPoinChanged(vm.Model);
        }

    }
}
