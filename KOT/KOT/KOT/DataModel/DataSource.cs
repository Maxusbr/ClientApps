using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Services.Maps;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using KOT.Common.Controls;
using KOT.DataModel.Handlers;
using KOT.DataModel.Model;
using KOT.DataModel.ViewModel;
using Newtonsoft.Json;

namespace KOT.DataModel
{
    public sealed class DataSource
    {
        private static DataSource _instance;
        public static DataSource Instance
        {
            get { return _instance ?? (_instance = new DataSource()); }
        }

        public DataSource()
        {
            _instance = this;
            UpdatePositionPhone();
            UpdatePositionKot();
        }
        public static ObservableCollection<ServicePointViewModel> ServicePoints = new ObservableCollection<ServicePointViewModel>();

        private readonly KotElement _kot = new KotElement{Location = new Geopoint(new BasicGeoposition { Altitude = 0, Latitude = 55.75, Longitude = 37.62 })};
        private readonly KotElement _phone = new KotElement("PhoneRadioButtonStyle", null){Visibility = Visibility.Collapsed };

        public static KotElement Kot { get { return Instance._kot; } }
        public static KotElement Phone { get { return Instance._phone; } }

        public static string Url = "http://effects.homevideo.pro:52000/files/";

        public static async void GetMapElements()
        {
            ServicePoints.Clear();
            await MapsElementsDataHandler.GetMapElements();
        }

        void UpdatePositionKot()
        {
            MapControl.SetLocation(Kot, Kot.Location);
        }

        async void UpdatePositionPhone()
        {
            var geolocator = new Geolocator();
            var position = await geolocator.GetGeopositionAsync();
            //MapControl.SetLocation(Phone, position.Coordinate.Point);
            MapControl.SetLocation(Phone, Phone.Location);
        }

        internal static async Task<MapRouteFinderResult> UpdatePhone(bool p)
        {
            Phone.Visibility = p ? Visibility.Visible : Visibility.Collapsed;
            if (p)
            {
                return await MapRouteFinder.GetWalkingRouteAsync(Phone.Location, Kot.Location);
            }
            return null;
        }
    }
}
