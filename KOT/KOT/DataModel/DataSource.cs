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

        private const string _url = "http://195.208.184.73/";
        public static string Url
        {
            get { return _url; }
        }

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
            //TODO Раскоментировать перед релизом
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

        internal async static Task SendComment(double userRate, string userComment)
        {
            await Instance.SendCommentAsync(userRate,  userComment);
        }

        private async Task SendCommentAsync(double userRate, string userComment)
        {
            var dataUri = new Uri(DataSource.Url + "GetTemplates");
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(dataUri);
                request.Method = "GET";
                //request.ContentType = "text/plain";
                using (var response = await request.GetResponseAsync())
                {
                    using (var stream = response.GetResponseStream())
                    {

                    }
                }

            }
            catch
            {

            }
        }
    }
}
