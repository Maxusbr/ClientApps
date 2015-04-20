using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;
using System.ComponentModel;
using System.Windows.Media;
using System.Linq;

namespace DTCDev.Client.Controls.Map
{
    /// <summary>
    /// A polyline or polygon created from a collection of Locations.
    /// </summary>
    public partial class MapPolyline : MapPath
    {

        protected static readonly Type LocationsPropertyType = typeof(IEnumerable<Location>);

        public static readonly DependencyProperty LocationsProperty = DependencyProperty.Register(
            "Locations", LocationsPropertyType, typeof(MapPolyline),
            new PropertyMetadata(null, LocationsPropertyChanged));

        public static readonly DependencyProperty IsClosedProperty = DependencyProperty.Register(
            "IsClosed", typeof(bool), typeof(MapPolyline),
            new PropertyMetadata(false, (o, e) => ((MapPolyline)o).UpdateData()));

        /// <summary>
        /// Gets or sets the locations that define the polyline points.
        /// </summary>

        [TypeConverter(typeof(LocationCollectionConverter))]
        public IEnumerable<Location> Locations
        {
            get { return (IEnumerable<Location>)GetValue(LocationsProperty); }
            set { SetValue(LocationsProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that indicates if the polyline is closed, i.e. is a polygon.
        /// </summary>
        public bool IsClosed
        {
            get { return (bool)GetValue(IsClosedProperty); }
            set { SetValue(IsClosedProperty, value); }
        }

        public MapPolyline()
        {
            Data = new StreamGeometry();
        }

        protected void LocationCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateData();
        }

        protected override void UpdateData()
        {
            var geometry = (StreamGeometry)Data;
            var locations = Locations;
            Location first;

            if (ParentMap != null && locations != null && (first = locations.FirstOrDefault()) != null)
            {
                using (var context = geometry.Open())
                {
                    var startPoint = ParentMap.MapTransform.Transform(first);
                    var points = locations.Skip(1).Select(l => ParentMap.MapTransform.Transform(l)).ToList();

                    context.BeginFigure(startPoint, IsClosed, IsClosed);
                    context.PolyLineTo(points, true, false);
                }

                geometry.Transform = ParentMap.ViewportTransform;
            }
            else
            {
                geometry.Clear();
                geometry.ClearValue(Geometry.TransformProperty);
            }
        }

        protected static void LocationsPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var mapPolyline = (MapPolyline)obj;
            var oldCollection = e.OldValue as INotifyCollectionChanged;
            var newCollection = e.NewValue as INotifyCollectionChanged;

            if (oldCollection != null)
            {
                oldCollection.CollectionChanged -= mapPolyline.LocationCollectionChanged;
            }

            if (newCollection != null)
            {
                newCollection.CollectionChanged += mapPolyline.LocationCollectionChanged;
            }

            mapPolyline.UpdateData();
        }
    }
}
