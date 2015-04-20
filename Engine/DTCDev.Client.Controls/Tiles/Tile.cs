using System;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace DTCDev.Client.Controls.Map
{
    public partial class Tile
    {
        public static TimeSpan AnimationDuration = TimeSpan.FromSeconds(0.5);

        public readonly int ZoomLevel;
        public readonly int X;
        public readonly int Y;

        public Tile(int zoomLevel, int x, int y)
        {
            ZoomLevel = zoomLevel;
            X = x;
            Y = y;
        }

        public bool HasImageSource { get; private set; }

        public int XIndex
        {
            get
            {
                var numTiles = 1 << ZoomLevel;
                return ((X % numTiles) + numTiles) % numTiles;
            }
        }

        public readonly ImageBrush Brush = new ImageBrush { Opacity = 0d };

        public ImageSource ImageSource
        {
            get { return Brush.ImageSource; }
        }

        public void SetImageSource(ImageSource image, bool animateOpacity)
        {
            if (image != null && Brush.ImageSource == null)
            {
                if (animateOpacity)
                {
                    var bitmap = image as BitmapSource;

                    if (bitmap != null && !bitmap.IsFrozen && bitmap.IsDownloading)
                    {
                        bitmap.DownloadCompleted += BitmapDownloadCompleted;
                        bitmap.DownloadFailed += BitmapDownloadFailed;
                    }
                    else
                    {
                        Brush.BeginAnimation(ImageBrush.OpacityProperty, new DoubleAnimation(1d, AnimationDuration));
                    }
                }
                else
                {
                    Brush.Opacity = 1d;
                }
            }

            Brush.ImageSource = image;
            HasImageSource = true;
        }

        private void BitmapDownloadCompleted(object sender, EventArgs e)
        {
            var bitmap = (BitmapSource)sender;

            bitmap.DownloadCompleted -= BitmapDownloadCompleted;
            bitmap.DownloadFailed -= BitmapDownloadFailed;

            Brush.BeginAnimation(ImageBrush.OpacityProperty, new DoubleAnimation(1d, AnimationDuration));
        }

        private void BitmapDownloadFailed(object sender, ExceptionEventArgs e)
        {
            var bitmap = (BitmapSource)sender;

            bitmap.DownloadCompleted -= BitmapDownloadCompleted;
            bitmap.DownloadFailed -= BitmapDownloadFailed;

            Brush.ImageSource = null;
        }
    }
}
