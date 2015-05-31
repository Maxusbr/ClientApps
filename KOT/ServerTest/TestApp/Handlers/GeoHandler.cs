using KOTServerTester.Models;
using KOTServerTester.Network;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KOTServerTester.Handlers
{
    public class GeoHandler
    {
        private static GeoHandler _instance;

        public static GeoHandler Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GeoHandler();
                return _instance;
            }
        }

        public GeoHandler()
        {
            _instance = this;
        }





        public void GetGeoTypes()
        {
            try
            {
                TCPConnection.Instance.SendData("DC");
            }
            catch { }
        }

        public void GetPlaces(int idType, int lat, int lon)
        {
            try
            {
                TCPConnection.Instance.SendData("DD" + idType.ToString() + ";" + lat.ToString() + ";" + lon.ToString());
            }
            catch { }
        }





        public void Split(char fx, string row)
        {
            switch (fx)
            {
                case 'C':
                    SplitGeoServiceTypes(row);
                    break;
                case 'D':
                    SplitPlaces(row);
                    break;
            }
        }


        private void SplitGeoServiceTypes(string row)
        {
            List<PlacesClassesModel> temp = JsonConvert.DeserializeObject<List<PlacesClassesModel>>(row);
            foreach (var item in temp)
            {
                Console.WriteLine("Geo service - " + item.ClassName + ", id " + item.ID);
            }
        }

        private void SplitPlaces(string row)
        {
            List<PlacesModel> places = JsonConvert.DeserializeObject<List<PlacesModel>>(row);
            foreach (var item in places)
            {
                Console.WriteLine(item.Name + ", Lat - " + item.Latitude.ToString() + "; lon - " + item.Longitude.ToString());
            }
        }
    }
}
