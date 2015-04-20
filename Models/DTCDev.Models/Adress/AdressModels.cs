using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Models.Adress
{
    public class CountryModel : AdressBase
    {

    }

    public class RegionModel : AdressBase
    {
        public int id_country { get; set; }
    }

    public class StateModel : AdressBase
    {
        public int id_Region { get; set; }
    }

    public class CityModel : AdressBase
    {
        public int id_State { get; set; }
    }

    public class StreetModel : AdressBase
    {
        public int id_City { get; set; }
    }

    public class PlaceModel : AdressBase
    {
        public int id_Street { get; set; }

        public int Lat { get; set; }

        public int Lon { get; set; }
    }
}
