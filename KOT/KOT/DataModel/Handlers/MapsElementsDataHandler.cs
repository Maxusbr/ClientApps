using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Maps;
using KOT.Common.Controls;
using KOT.DataModel.Model;

namespace KOT.DataModel.Handlers
{
    public class MapsElementsDataHandler
    {
        private static readonly MapsElementsDataHandler Instance = new MapsElementsDataHandler();
        
        public static async Task GetMapElements()
        {
            await Instance.GetMapElementsAsync();
        }

        private async Task GetMapElementsAsync()
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
            AddServiceElement(new PlacesModel { Latitude = 557500, Longitude = 376300 });
            AddWashElement(new PlacesModel { Latitude = 557400, Longitude = 376300 });
            AddGasElement(new PlacesModel { Latitude = 557500, Longitude = 376100 });
            AddShopElement(new PlacesModel { Latitude = 557600, Longitude = 376100 });       
     
             
        }

        private void AddElements(PlacesModel model)
        {
            switch (model.idCategory)
            {
                case 0:
                    AddGasElement(model);
                    break;
                case 1:
                    AddWashElement(model);
                    break;
                case 2:
                    AddShopElement(model);
                    break;
                case 3:
                    AddServiceElement(model);
                    break;
            }
        }

        private void AddGasElement(PlacesModel model)
        {
            var pin = new ServiceElement("GasRadioButtonStyle", new Geopoint(new BasicGeoposition
            {
                Altitude = 0, Latitude = model.Latitude / 10000.0, Longitude = model.Longitude / 10000.0
            })) { DataContext = model};
            pin.Click += pin_Click;
            MapControl.SetLocation(pin, pin.Location);
            DataSource.Instance.MapElements.Add(pin);
        }

        private void AddWashElement(PlacesModel model)
        {
            var pin = new ServiceElement("WashRadioButtonStyle", new Geopoint(new BasicGeoposition
            {
                Altitude = 0,
                Latitude = model.Latitude / 10000.0,
                Longitude = model.Longitude / 10000.0
            })) { DataContext = model };
            pin.Click += pin_Click;
            MapControl.SetLocation(pin, pin.Location);
            DataSource.Instance.MapElements.Add(pin);
        }

        private void AddShopElement(PlacesModel model)
        {
            var pin = new ServiceElement("ShopRadioButtonStyle", new Geopoint(new BasicGeoposition
            {
                Altitude = 0,
                Latitude = model.Latitude / 10000.0,
                Longitude = model.Longitude / 10000.0
            })) { DataContext = model };
            pin.Click += pin_Click;
            MapControl.SetLocation(pin, pin.Location);
            DataSource.Instance.MapElements.Add(pin);
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
            DataSource.Instance.MapElements.Add(pin);
        }
        void pin_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            
        }
    }
}
