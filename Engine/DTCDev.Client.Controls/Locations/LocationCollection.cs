using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DTCDev.Client.Controls.Map
{
    /// <summary>
    /// A collection of geographic locations.
    /// </summary>
    public partial class LocationCollection : ObservableCollection<Location>
    {
        public LocationCollection()
        {
        }

        public LocationCollection(IEnumerable<Location> locations)
            : base(locations)
        {
        }

        public LocationCollection(List<Location> locations)
            : base(locations)
        {
        }

        public static LocationCollection Parse(string s)
        {
            var strings = s.Split(new char[] { ' ', ';' }, StringSplitOptions.RemoveEmptyEntries);

            return new LocationCollection(strings.Select(l => Location.Parse(l)));
        }
    }
}
