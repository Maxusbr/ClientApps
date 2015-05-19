using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Map.Children.Add(DataSource.Phone);
            Map.Children.Add(DataSource.Kot);
            MapControls.ItemsSource = DataSource.ServicePoints;
            DataSource.ServicePoints.CollectionChanged += ServicePoints_CollectionChanged;
            UpdateMapElements();
        }

        private void UpdateMapElements()
        {
            DataSource.GetMapElements();
        }

        private void InitializeControls()
        {
            DefaultViewModel["Center"] = new Geopoint(new BasicGeoposition { Altitude = 0, Latitude = 55.75, Longitude = 37.62 });
            DefaultViewModel["ServiceToken"] = "dIjWUGuzClDyimeHLXa9bw";
        }

        private async void ServicePoints_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var positions = DataSource.ServicePoints.Select(el => el.Location.Position).ToList();
            positions.Add(DataSource.Kot.Location.Position);
            var box = GeoboundingBox.TryCompute(positions);

            SetCenterLocation(new Geopoint(box.Center)) ;
            await Map.TrySetViewBoundsAsync(box, null, MapAnimationKind.Default);
        }

        public async void AddRoute(bool p)
        {
            Map.Routes.Clear();
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

        public async void SetCenterLocation(Geopoint point)
        {
            await Map.TrySetViewAsync(point ?? DataSource.Kot.Location);
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
