using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Linq;

namespace DTCDev.Client.Controls.Map
{
    /// <summary>
    /// Draws a graticule overlay.
    /// </summary>
    public partial class MapGraticule : MapOverlay
    {
        static MapGraticule()
        {
            UIElement.IsHitTestVisibleProperty.OverrideMetadata(
                typeof(MapGraticule), new FrameworkPropertyMetadata(false));

            MapOverlay.StrokeThicknessProperty.OverrideMetadata(
                typeof(MapGraticule), new FrameworkPropertyMetadata(0.5, (o, e) => ((MapGraticule)o).glyphRuns.Clear()));
        }

        /// <summary>
        /// Graticule line spacings in degrees.
        /// </summary>
        public static double[] LineSpacings =
            new double[] { 1d / 60d, 1d / 30d, 1d / 12d, 1d / 6d, 1d / 4d, 1d / 3d, 1d / 2d, 1d, 2d, 5d, 10d, 15d, 20d, 30d, 45d };

        public static readonly DependencyProperty MinLineSpacingProperty = DependencyProperty.Register(
            "MinLineSpacing", typeof(double), typeof(MapGraticule), new PropertyMetadata(150d));

        /// <summary>
        /// Minimum spacing in pixels between adjacent graticule lines.
        /// </summary>
        public double MinLineSpacing
        {
            get { return (double)GetValue(MinLineSpacingProperty); }
            set { SetValue(MinLineSpacingProperty, value); }
        }

        private class Label
        {
            public readonly double Position;
            public readonly string Text;

            public Label(double position, string text)
            {
                Position = position;
                Text = text;
            }
        }

        private Dictionary<string, GlyphRun> glyphRuns = new Dictionary<string, GlyphRun>();

        private static string CoordinateString(double value, string format, string hemispheres)
        {
            var hemisphere = hemispheres[0];

            if (value < -1e-8) // ~1mm
            {
                value = -value;
                hemisphere = hemispheres[1];
            }

            var minutes = (int)(value * 60d + 0.5);

            return string.Format(format, hemisphere, minutes / 60, (double)(minutes % 60));
        }

        protected override void OnViewportChanged()
        {
            InvalidateVisual();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (ParentMap != null)
            {
                var bounds = ParentMap.ViewportTransform.Inverse.TransformBounds(new Rect(ParentMap.RenderSize));
                var start = ParentMap.MapTransform.Transform(new Point(bounds.X, bounds.Y));
                var end = ParentMap.MapTransform.Transform(new Point(bounds.X + bounds.Width, bounds.Y + bounds.Height));
                var minSpacing = MinLineSpacing * 360d / (Math.Pow(2d, ParentMap.ZoomLevel) * TileSource.TileSize);
                var spacing = LineSpacings[LineSpacings.Length - 1];

                if (spacing >= minSpacing)
                {
                    spacing = LineSpacings.FirstOrDefault(s => s >= minSpacing);
                }

                var labelFormat = spacing < 1d ? "{0} {1}°{2:00}'" : "{0} {1}°";
                var labelStart = new Location(
                    Math.Ceiling(start.Latitude / spacing) * spacing,
                    Math.Ceiling(start.Longitude / spacing) * spacing);

                var latLabels = new List<Label>((int)((end.Latitude - labelStart.Latitude) / spacing) + 1);
                var lonLabels = new List<Label>((int)((end.Longitude - labelStart.Longitude) / spacing) + 1);

                for (var lat = labelStart.Latitude; lat <= end.Latitude; lat += spacing)
                {
                    latLabels.Add(new Label(lat, CoordinateString(lat, labelFormat, "NS")));

                    drawingContext.DrawLine(Pen,
                        ParentMap.LocationToViewportPoint(new Location(lat, start.Longitude)),
                        ParentMap.LocationToViewportPoint(new Location(lat, end.Longitude)));
                }

                for (var lon = labelStart.Longitude; lon <= end.Longitude; lon += spacing)
                {
                    lonLabels.Add(new Label(lon, CoordinateString(Location.NormalizeLongitude(lon), labelFormat, "EW")));

                    drawingContext.DrawLine(Pen,
                        ParentMap.LocationToViewportPoint(new Location(start.Latitude, lon)),
                        ParentMap.LocationToViewportPoint(new Location(end.Latitude, lon)));
                }

                if (Foreground != null && Foreground != Brushes.Transparent && latLabels.Count > 0 && lonLabels.Count > 0)
                {
                    var latLabelOrigin = new Point(StrokeThickness / 2d + 2d, -StrokeThickness / 2d - FontSize / 4d);
                    var lonLabelOrigin = new Point(StrokeThickness / 2d + 2d, StrokeThickness / 2d + FontSize);
                    var transform = Matrix.Identity;
                    transform.Rotate(ParentMap.Heading);

                    foreach (var latLabel in latLabels)
                    {
                        foreach (var lonLabel in lonLabels)
                        {
                            GlyphRun latGlyphRun;
                            GlyphRun lonGlyphRun;

                            if (!glyphRuns.TryGetValue(latLabel.Text, out latGlyphRun))
                            {
                                latGlyphRun = GlyphRunText.Create(latLabel.Text, Typeface, FontSize, latLabelOrigin);
                                glyphRuns.Add(latLabel.Text, latGlyphRun);
                            }

                            if (!glyphRuns.TryGetValue(lonLabel.Text, out lonGlyphRun))
                            {
                                lonGlyphRun = GlyphRunText.Create(lonLabel.Text, Typeface, FontSize, lonLabelOrigin);
                                glyphRuns.Add(lonLabel.Text, lonGlyphRun);
                            }

                            var position = ParentMap.LocationToViewportPoint(new Location(latLabel.Position, lonLabel.Position));

                            drawingContext.PushTransform(new MatrixTransform(
                                transform.M11, transform.M12, transform.M21, transform.M22, position.X, position.Y));

                            drawingContext.DrawGlyphRun(Foreground, latGlyphRun);
                            drawingContext.DrawGlyphRun(Foreground, lonGlyphRun);
                            drawingContext.Pop();
                        }
                    }

                    var removeKeys = glyphRuns.Keys.Where(k => !latLabels.Any(l => l.Text == k) && !lonLabels.Any(l => l.Text == k));

                    foreach (var key in removeKeys.ToList())
                    {
                        glyphRuns.Remove(key);
                    }
                }
                else
                {
                    glyphRuns.Clear();
                }
            }
        }
    }
}
