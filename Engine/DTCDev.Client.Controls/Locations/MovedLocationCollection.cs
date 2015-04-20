using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTCDev.Client.Controls.Map
{
    public partial class MovedLocationCollection : ObservableCollection<MovedLocation>
    {
        public MovedLocationCollection()
        {
        }

        public MovedLocationCollection(IEnumerable<MovedLocation> locations)
            : base(locations)
        {
        }

        public MovedLocationCollection(List<MovedLocation> locations)
            : base(locations)
        {
        }

        public static MovedLocationCollection Parse(string s)
        {
            var strings = s.Split(new char[] { ' ', ';' }, StringSplitOptions.RemoveEmptyEntries);

            return new MovedLocationCollection(strings.Select(l => MovedLocation.Parse(l)));
        }

        public override string ToString()
        {
            string res = string.Join(" ", this.Select(o => o.ToString()));
            return res;
        }

        public Location GetCenter()
        {
            if (!this.Any()) return new Location(0, 0);
            double latitude = this.Average(o => o.Latitude);
            double longitude = this.Average(o => o.Longitude);
            return new Location(latitude, longitude);
        }
    }
}
