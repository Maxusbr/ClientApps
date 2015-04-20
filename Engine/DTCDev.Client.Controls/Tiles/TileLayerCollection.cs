using System.Collections.ObjectModel;
using System.Linq;

namespace DTCDev.Client.Controls.Map
{
    public class TileLayerCollection : ObservableCollection<TileLayer>
    {
        public TileLayer this[string sourceName]
        {
            get { return this.FirstOrDefault(t => t.SourceName == sourceName); }
        }
    }
}
