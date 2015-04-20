using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using DTCDev.Client.Cars.Engine.DisplayModels;
using DTCDev.Client.Controls.Map;
using DTCDev.Models.CarsSending;
using Newtonsoft.Json;

namespace DTCDev.Client.Cars.Engine.Handlers.Cars
{
    public class ZonesHandler
    {
        private static ZonesHandler _instance;

        public static ZonesHandler Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ZonesHandler();
                return _instance;
            }
        }

        public ZonesHandler()
        {
            _instance = this;
        }



        public delegate void LoadComplete(string ResultOperation);
        public event LoadComplete LoadCompleted;



        ObservableCollection<VmPolyline> _zones = new ObservableCollection<VmPolyline>();
        public ObservableCollection<VmPolyline> Zones
        {
            get { return _zones; }
            set { _zones = value; }
        }

        private List<ZoneDataModel> _data = new List<ZoneDataModel>();

        public void Init()
        {
            
        }


        #region NETWORK

        public void Split(char fx, string row)
        {
            switch (fx)
            {
                case 'a':
                case 'A':
                    FillZones(row);
                    break;
                case 'b':
                case 'B':
                    Update();
                    break;
                case 'c':
                case 'C':
                    Update();
                    break;
                    
            }
        }


        private void FillZones(string row)
        {
            try
            {
                List<ZoneDataModel> zones = JsonConvert.DeserializeObject<List<ZoneDataModel>>(row);
                if (Application.Current != null)
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            if (zones == null)
                                return;
                            _data = zones;
                            SaveToDisk();
                            Zones.Clear();
                            foreach (var item in zones)
                            {
                                VmPolyline vmp = new VmPolyline();
                                vmp.ID = item.ID;
                                vmp.Name = item.Name;
                                vmp.IsPublic = item.IsPublic == 1;
                                //List<Location> locations = new List<Location>();
                                for (int i = 0; i < item.Points.Count(); i++)
                                {
                                    //locations.Add(new Location
                                    //{
                                    //    Latitude = item.Points[i].Latitude / 10000.0,
                                    //    Longitude = item.Points[i].Longitude / 10000.0
                                    //});
                                    vmp.AddLocation(new Location
                                    {
                                        Latitude = item.Points[i].Latitude / 10000.0,
                                        Longitude = item.Points[i].Longitude / 10000.0
                                    }, false);
                                }
                                //vmp.Locations = new LocationCollection(locations);
                                Zones.Add(vmp);
                            }
                        }));
            }
            catch { }
        }

        #endregion NETWORK

        public void Update()
        {
            try
            {
                TCPConnection.Instance.SendData("FA");
                LoadFromDisk();
            }
            catch { }
        }

        public void EditZone(VmPolyline z)
        {
            try
            {
                TCPConnection.Instance.SendData("FC" + ConvertZone(z));
            }
            catch { }
        }

        public void AddZone(VmPolyline z)
        {
            try
            {
                TCPConnection.Instance.SendData("FB" + ConvertZone(z));
            }
            catch { }
        }

        private string ConvertZone(VmPolyline z)
        {
            ZoneDataModel model = new ZoneDataModel();
            model.ID = z.ID;
            model.Name = z.Name;
            List<ZonePoint> points = new List<ZonePoint>();
            foreach (var item in z.Locations)
            {
                points.Add(new ZonePoint
                {
                    Latitude = (int)(item.Latitude * 10000),
                    Longitude = (int)(item.Longitude * 10000)
                });
            }
            model.IsPublic = z.IsPublic ? 1 : 0;
            model.Points = points;
            string req = JsonConvert.SerializeObject(model);
            return req;
        }

        private void LoadFromDisk()
        {
            
        }

        private void SaveToDisk()
        {

        }

        public void DeleteZone(VmPolyline zone)
        {
            //throw new NotImplementedException();
        }
    }
}
