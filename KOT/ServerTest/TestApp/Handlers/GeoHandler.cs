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





        public void Split(char fx, string row)
        {
            switch (fx)
            {
                case 'C':
                    SplitGeoServiceTypes(row);
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
    }
}
