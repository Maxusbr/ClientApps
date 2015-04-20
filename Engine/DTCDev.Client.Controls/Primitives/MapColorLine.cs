using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace DTCDev.Client.Controls.Map
{
    /// <summary>
    /// A line or polygon created from a collection of Locations.
    /// </summary>
    public class MapColorLine : MapPolyline
    {

        public MapColorLine()
        {
            Data = new StreamGeometry();
        }

        protected override void UpdateData()
        {
            var geometry = (StreamGeometry)Data;
            var locations = Locations;

            if (ParentMap != null && locations!= null)
            {
                using (var context = geometry.Open())
                {
                    foreach (var el in locations)
                    {
                        var point = ParentMap.MapTransform.Transform(el);
                        if(el.FirstPoint)
                            context.BeginFigure(point, IsClosed, IsClosed);
                        else
                            context.LineTo(point, true, true);
                    }
                }
                geometry.Transform = ParentMap.ViewportTransform;
            }
            else
            {
                geometry.Clear();
                geometry.ClearValue(Geometry.TransformProperty);
            }
        }
    }
}
