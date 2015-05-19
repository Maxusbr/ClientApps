using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.UI.Xaml;
using KOT.DataModel.Model;

namespace KOT.DataModel.ViewModel
{
    public class ServicePointViewModel
    {
        public ServicePointViewModel() { StylePoint = Application.Current.Resources["ServiceRadioButtonStyle"] as Style; }

        public ServicePointViewModel(PlacesModel model)
        {
            Model = model;
            Location = new Geopoint(new BasicGeoposition
            {
                Altitude = 0,
                Latitude = model.Latitude/10000.0,
                Longitude = model.Longitude/10000.0
            });
            NAP = new Point(0, 1);
            StylePoint = Application.Current.Resources["ServiceRadioButtonStyle"] as Style;
        }

        public ServicePointViewModel(PlacesModel model, string style)
        {
            Model = model;
            Location = new Geopoint(new BasicGeoposition
            {
                Altitude = 0,
                Latitude = model.Latitude/10000.0,
                Longitude = model.Longitude/10000.0
            });
            NAP = new Point(0, 1);
            StylePoint = Application.Current.Resources[style] as Style;
        }

        public PlacesModel Model { get; set; }
        public Geopoint Location { get; set; }
        public Point NAP { get; set; }
        public Style StylePoint { get; set; }
    }
}
