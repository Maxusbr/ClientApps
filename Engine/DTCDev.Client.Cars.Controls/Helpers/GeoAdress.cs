using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using DTCDev.Client.Controls.Map;


namespace DTCDev.Client.Cars.Controls.Helpers
{
    public class GeoAdress
    {
        //static string geourl = "http://nominatim.openstreetmap.org/search/{0} {1}, {2}?format=json&polygon=0&addressdetails=0";
        static string geourl = "http://nominatim.openstreetmap.org/search?street={0} {1}&city={2}&format=json&polygon=0&addressdetails=0";
        static string adressurl = "http://nominatim.openstreetmap.org/reverse?format=json&&lat={0}&lon={1}&zoom=18&addressdetails=0";

        private static GeoAdress _instance;
        public static GeoAdress Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GeoAdress();
                return _instance;
            }
        }

        public GeoAdress()
        {
            _instance = this;
        }

        public string GetAdress(Location coordinates)
        {
            Uri dataUri = new Uri(string.Format(adressurl,
                coordinates.Latitude.ToString("G", CultureInfo.InvariantCulture),
                coordinates.Longitude.ToString("G", CultureInfo.InvariantCulture)));
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(dataUri);
                request.Method = "GET";
                AdressModel Adress;
                using (var response = request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        DataContractJsonSerializer dcs = new DataContractJsonSerializer(typeof(AdressModel));
                        Adress = dcs.ReadObject(stream) as AdressModel;
                    }
                }
                if (Adress == null) return string.Empty;
                return string.Join(", ", Adress.display_name.Split(',').Take(3));
            }
            catch
            {
                return string.Empty;
            }
        }

        public List<Location> GetCoordinat(string house, string street, string city)
        {
            Uri dataUri = new Uri(string.Format(geourl, house, street, city));
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(dataUri);
                request.Method = "GET";
                AdressModel[] Adress;
                using (var response = request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        DataContractJsonSerializer dcs = new DataContractJsonSerializer(typeof(AdressModel[]));
                        Adress = dcs.ReadObject(stream) as AdressModel[];
                    }
                }
                if (Adress == null || Adress.Count() == 0) return null;
                List<Location> lookups = new List<Location>();
                foreach (AdressModel el in Adress)
                {
                    double lon = 0, lat = 0;
                    NumberStyles style = NumberStyles.AllowDecimalPoint;
                    CultureInfo culture = CultureInfo.InvariantCulture;
                    double.TryParse(el.lat, style, culture, out lat);
                    double.TryParse(el.lon, style, culture, out lon);
                    lookups.Add(new Location(lat, lon));
                }
                return lookups;
            }
            catch
            {
                return null;
            }
        }
    }

    public class AdressModel
    {
        public string place_id { get; set; }
        public string lat { get; set; }
        public string lon { get; set; }
        public string display_name { get; set; }
        public override string ToString()
        {
            return display_name;
        }
    }
}
