using System.Windows;
using System.Windows.Media;


namespace DTCDev.Client.Controls.Map
{
    /// <summary>
    /// Fills a rectangular area with an ImageBrush from the Source property.
    /// </summary>
    public partial class MapImage : MapRectangle
    {
        static MapImage()
        {
            imageTransform.Freeze();
        }

        private static readonly MatrixTransform imageTransform = new MatrixTransform
        {
            Matrix = new Matrix(1d, 0d, 0d, -1d, 0d, 1d)
        };

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            "Source", typeof(ImageSource), typeof(MapImage),
            new PropertyMetadata(null, (o, e) => ((MapImage)o).SourceChanged((ImageSource)e.NewValue)));

        public ImageSource Source
        {
            get { return (ImageSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        private void SourceChanged(ImageSource image)
        {
            var imageBrush = new ImageBrush
            {
                ImageSource = image,
                RelativeTransform = imageTransform
            };

            Fill = imageBrush;
        }
    }
}
