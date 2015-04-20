using System;
using System.Globalization;

namespace DTCDev.Client.Controls.Map
{
    /// <summary>
    /// A geographic location with latitude and longitude values in degrees.
    /// </summary>
    [Serializable]
    public partial class Location
    {
        protected double latitude;
        protected double longitude;
        private bool firstPoint = false;

        public Location Current { get; set; }

        public Location()
        {
            Current = this;
        }

        public Location(double latitude, double longitude, bool first = false)
        {
            Latitude = latitude;
            Longitude = longitude;
            firstPoint = first;
            Current = this;
        }

        public Location(Location el, bool first)
        {
            Latitude = el.Latitude;
            Longitude = el.Longitude;
            firstPoint = first;
            Current = this;
        }

        public double Latitude
        {
            get { return latitude; }
            set { latitude = Math.Min(Math.Max(value, -90d), 90d); }
        }

        public double Longitude
        {
            get { return longitude; }
            set { longitude = value; }
        }

        public bool FirstPoint
        {
            get { return firstPoint; }
            set { firstPoint = value; }
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0:F5},{1:F5}", latitude, longitude);
        }

        public static Location Parse(string s)
        {
            var tokens = s.Split(new char[] { ',' });
            if (tokens.Length != 2)
            {
                throw new FormatException("Location string must be a comma-separated pair of double values");
            }

            return new Location(
                double.Parse(tokens[0], NumberStyles.Float, CultureInfo.InvariantCulture),
                double.Parse(tokens[1], NumberStyles.Float, CultureInfo.InvariantCulture));
        }

        public static double NormalizeLongitude(double longitude)
        {
            if (longitude > 180)
            {
                longitude = ((longitude - 180d) % 360d) - 180d;
            }
            else if (longitude < -180d)
            {
                longitude = ((longitude + 180d) % 360d) + 180d;
            }

            return longitude;
        }

        public bool Aprox(Location loc, double delta)
        {
            return Math.Abs(Latitude - loc.Latitude) + Math.Abs(Longitude - loc.Longitude) > delta;
        }
    }
}
