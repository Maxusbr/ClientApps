using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using DTCDev.Client.Controls.Map;

namespace DTCDev.Client.Cars.Controls.Helpers
{
    public class CalcLeavingZone
    {
        private static CalcLeavingZone _instance;
        public static CalcLeavingZone Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new CalcLeavingZone();
                return _instance;
            }
        }

        public CalcLeavingZone()
        {
            _instance = this;
        }

        public bool FillContains(Location point, MovedLocationCollection geometry)
        {
            Point Position = Transform(point);

            var Points = geometry.Select(l => new
            {
                X = Transform(l).X - Position.X,
                Y = Position.Y - Transform(l).Y,
                atan = Math.Atan2(Position.Y - Transform(l).Y, Transform(l).X - Position.X)
            }).ToList();
            Points.Add(Points[0]);

            double result = 0;
            for (int i = 0; i < Points.Count() - 1; i++)
            {
                double atan = Points[i].atan - Points[i + 1].atan;
                if (Math.Abs(atan) > Math.PI)
                    atan -= Math.PI * Math.Sign(atan);

                result += atan;
            }
            return Math.Abs(result) > 1;
        }

        private Point Transform(Location location)
        {
            double latitude;

            if (location.Latitude <= -90d)
            {
                latitude = double.NegativeInfinity;
            }
            else if (location.Latitude >= 90d)
            {
                latitude = double.PositiveInfinity;
            }
            else
            {
                latitude = location.Latitude * Math.PI / 180d;
                latitude = Math.Log(Math.Tan(latitude) + 1d / Math.Cos(latitude)) / Math.PI * 180d;
            }

            return new Point(location.Longitude, latitude);
        }
    }
}
