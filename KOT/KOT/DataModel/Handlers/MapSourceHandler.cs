using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Services.Maps;
using Windows.System.Threading;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Maps;
using KOT.Common.Controls;
using KOT.DataModel.Model;
using KOT.DataModel.Network;
using KOT.DataModel.ViewModel;
using Newtonsoft.Json;

namespace KOT.DataModel.Handlers
{
    public sealed class MapSourceHandler
    {
        private static MapSourceHandler _instance;
        public static MapSourceHandler Instance
        {
            get { return _instance ?? (_instance = new MapSourceHandler()); }
        }

        public MapSourceHandler()
        {
            _instance = this;
            
            UpdatePositionPhone();
            UpdatePositionKot();
            //CarsHandler.SelectionChanged += CarsHandler_SelectionChanged;
        }

        async void CarsHandler_SelectionChanged(object sender, EventArgs e)
        {
            await UpdatePositionKotAsync();
            OnCenterUpdate(CenterMap = new Geopoint(Kot.Location.Position));
        }

        public delegate void CenterUpdateEvent(Geopoint point);
        public static event CenterUpdateEvent CenterUpdate;
        private void OnCenterUpdate(Geopoint point)
        {
            if (CenterUpdate != null) CenterUpdate(point);
        }

        public static ObservableCollection<ServicePointViewModel> ServicePoints = new ObservableCollection<ServicePointViewModel>();

        private readonly KotElement _kot = new KotElement { Location = new Geopoint(new BasicGeoposition { Altitude = 0, Latitude = 55.75, Longitude = 37.62 }) };
        private readonly KotElement _phone = new KotElement("PhoneRadioButtonStyle", null) { Visibility = Visibility.Collapsed };
        
        public static Geopoint CenterMap { get; set; }
        public static KotElement Kot { get { return Instance._kot; } }
        public static KotElement Phone { get { return Instance._phone; } }
        private string CarId { get { return CarsHandler.SelectedCar.DID; } }

        public static async Task GetMapElements()
        {
            ServicePoints.Clear();
            await Instance.UpdatePositionKotAsync();
            await MapsElementsDataHandler.GetMapElements();
        }

        async void UpdatePositionKot()
        {
            while (true)
            {
                await UpdatePositionKotAsync();
                await Task.Delay(new TimeSpan(0, 0, 10));
            }
        }

        private async Task UpdatePositionKotAsync()
        {
            var res = await TcpConnection.Send("BB" + CarId);
            if (!string.IsNullOrEmpty(res.Msg))
                await Split(res.Msg);
            if (CenterMap == null) OnCenterUpdate(CenterMap = new Geopoint(Kot.Location.Position));
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
            if (!p) return null;
            var res = await MapRouteFinder.GetWalkingRouteAsync(Phone.Location, Kot.Location);
            if (res == null || res.Status != MapRouteFinderStatus.Success) 
                res= await MapRouteFinder.GetDrivingRouteAsync(Phone.Location, Kot.Location);
            return res;
        }

        internal async static Task SendComment(double userRate, string userComment)
        {
            await Instance.SendCommentAsync(userRate, userComment);
        }

        private async Task SendCommentAsync(double userRate, string userComment)
        {
            try
            {


            }
            catch
            {

            }
        }

        async void Loop(object state)
        {
            while (true)
            {
                UpdatePositionPhone();
                await Task.Delay(new TimeSpan(0, 0, 30));
            }
        }

        private async Task Split(string msg)
        {
            try
            {
                var res = JsonConvert.DeserializeObject<CarStateFullModel[]>(msg);
                if (res == null) return;
                var kot = res.FirstOrDefault(o => o.State.ID.Equals(CarId));
                if (kot == null) return;
                Kot.Location = new Geopoint(new BasicGeoposition
                {
                    Altitude = 0,
                    Latitude = kot.State.Navigation.Latitude / 10000.0,
                    Longitude = kot.State.Navigation.Longitude / 10000.0,
                });

            }
            catch (Exception)
            {

            }
        }

    }
}
