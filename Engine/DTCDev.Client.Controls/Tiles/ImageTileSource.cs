using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DTCDev.Client.Controls.Map
{
    /// <summary>
    /// Provides the image of a map tile. ImageTileSource bypasses downloading and
    /// optional caching in TileImageLoader. By overriding the LoadImage method,
    /// an application can provide tile images from an arbitrary source.
    /// WPF only: If the CanLoadAsync property is true, LoadImage will be called
    /// from a separate, non-UI thread and must hence return a frozen ImageSource.
    /// </summary>
    public partial class ImageTileSource : TileSource
    {
        public virtual ImageSource LoadImage(int x, int y, int zoomLevel)
        {
            var uri = GetUri(x, y, zoomLevel);

            return uri != null ? new BitmapImage(uri) : null;
        }

        public virtual bool CanLoadAsync
        {
            get { return false; }
        }
    }
}
