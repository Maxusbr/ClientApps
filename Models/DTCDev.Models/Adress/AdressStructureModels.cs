using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.Adress
{
    public class CountryStructModel : AdressBase
    {
        private List<RegionStructModel> _regions = new List<RegionStructModel>();
        public List<RegionStructModel> Regions
        {
            get { return _regions; }
            set { _regions = value; }
        }
    }

    public class RegionStructModel : AdressBase
    {
        private List<CityStructModel> _cities = new List<CityStructModel>();
        public List<CityStructModel> Cities
        {
            get { return _cities; }
            set { _cities = value; }
        }
    }

    public class CityStructModel : AdressBase
    {
        private List<StreetStructModel> _streets = new List<StreetStructModel>();
        public List<StreetStructModel> Streets
        {
            get { return _streets; }
            set { _streets = value; }
        }
    }

    public class StreetStructModel : AdressBase
    {
        private List<PlaceStructModel> _places = new List<PlaceStructModel>();
        public List<PlaceStructModel> Places
        {
            get { return _places; }
            set { _places = value; }
        }
    }

    public class PlaceStructModel : AdressBase
    {
    }
}
