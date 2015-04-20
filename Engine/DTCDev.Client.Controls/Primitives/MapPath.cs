using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;


namespace DTCDev.Client.Controls.Map
{
    /// <summary>
    /// Base class for map shapes.
    /// </summary>
    public partial class MapPath : Shape, IMapElement
    {
        public static readonly DependencyProperty DataProperty = DependencyProperty.Register(
            "Data", typeof(Geometry), typeof(MapPath),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public Geometry Data
        {
            get { return (Geometry)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        protected override Geometry DefiningGeometry
        {
            get { return Data; }
        }

        private MapBase parentMap;

        public MapBase ParentMap
        {
            get { return parentMap; }
            set
            {
                parentMap = value;
                UpdateData();
            }
        }

        protected virtual void UpdateData()
        {
        }

        protected override Size MeasureOverride(Size constraint)
        {
            // base.MeasureOverride in WPF and WinRT sometimes return a Size with zero
            // width or height, whereas base.MeasureOverride in Silverlight occasionally
            // throws an ArgumentException, as it tries to create a Size from a negative
            // width or height, apparently resulting from a transformed Geometry.
            // In either case it seems to be sufficient to simply return a non-zero size.
            return new Size(1, 1);
        }
    }
}
