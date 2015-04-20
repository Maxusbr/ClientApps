using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;


namespace DTCDev.Client.Controls.Map
{
    internal partial class TileContainer : ContainerVisual
    {
        // relative scaled tile size ranges from 0.75 to 1.5 (192 to 384 pixels)
        private static double zoomLevelSwitchDelta = -Math.Log(0.75, 2d);

        internal static TimeSpan UpdateInterval = TimeSpan.FromSeconds(0.5);

        private readonly DispatcherTimer updateTimer;
        private Size viewportSize;
        private Point viewportOrigin;
        private Point tileLayerOffset;
        private double rotation;
        private double zoomLevel;
        private int tileZoomLevel;
        private Int32Rect tileGrid;

        public readonly MatrixTransform ViewportTransform = new MatrixTransform();

        public TileContainer()
        {
            updateTimer = new DispatcherTimer { Interval = UpdateInterval };
            updateTimer.Tick += UpdateTiles;
        }

        public void AddTileLayers(int index, IEnumerable<TileLayer> tileLayers)
        {
            var tileLayerTransform = GetTileLayerTransformMatrix();

            foreach (var tileLayer in tileLayers)
            {
                if (index < Children.Count)
                {
                    Children.Insert(index, tileLayer);
                }
                else
                {
                    Children.Add(tileLayer);
                }

                index++;
                tileLayer.SetTransformMatrix(tileLayerTransform);
                tileLayer.UpdateTiles(tileZoomLevel, tileGrid);
            }
        }

        public void RemoveTileLayers(int index, int count)
        {
            while (count-- > 0)
            {
                ((TileLayer)Children[index]).ClearTiles();
                Children.RemoveAt(index);
            }
        }

        public void ClearTileLayers()
        {
            foreach (TileLayer tileLayer in Children)
            {
                tileLayer.ClearTiles();
            }

            Children.Clear();
        }

        public double SetViewportTransform(double mapZoomLevel, double mapRotation, Point mapOrigin, Point vpOrigin, Size vpSize)
        {
            var scale = Math.Pow(2d, zoomLevel) * TileSource.TileSize / 360d;
            var oldMapOriginX = (viewportOrigin.X - tileLayerOffset.X) / scale - 180d;

            if (zoomLevel != mapZoomLevel)
            {
                zoomLevel = mapZoomLevel;
                scale = Math.Pow(2d, zoomLevel) * TileSource.TileSize / 360d;
            }

            rotation = mapRotation;
            viewportSize = vpSize;
            viewportOrigin = vpOrigin;

            var transformOffsetX = viewportOrigin.X - mapOrigin.X * scale;
            var transformOffsetY = viewportOrigin.Y + mapOrigin.Y * scale;

            ViewportTransform.Matrix = GetViewportTransformMatrix(scale, transformOffsetX, transformOffsetY);

            tileLayerOffset.X = transformOffsetX - 180d * scale;
            tileLayerOffset.Y = transformOffsetY - 180d * scale;

            var tileLayerTransform = GetTileLayerTransformMatrix();

            foreach (TileLayer tileLayer in Children)
            {
                tileLayer.SetTransformMatrix(tileLayerTransform);
            }

            if (Math.Abs(mapOrigin.X - oldMapOriginX) > 180d)
            {
                // immediately handle map origin leap when map center moves across 180° longitude
                UpdateTiles(this, EventArgs.Empty);
            }
            else
            {
                updateTimer.Start();
            }

            return scale;
        }

        private void UpdateTiles(object sender, object e)
        {
            updateTimer.Stop();

            var zoom = (int)Math.Floor(zoomLevel + zoomLevelSwitchDelta);
            var transform = GetTileIndexMatrix(1 << zoom);

            // tile indices of visible rectangle
            var p1 = transform.Transform(new Point(0d, 0d));
            var p2 = transform.Transform(new Point(viewportSize.Width, 0d));
            var p3 = transform.Transform(new Point(0d, viewportSize.Height));
            var p4 = transform.Transform(new Point(viewportSize.Width, viewportSize.Height));

            // index ranges of visible tiles
            var x1 = (int)Math.Floor(Math.Min(p1.X, Math.Min(p2.X, Math.Min(p3.X, p4.X))));
            var y1 = (int)Math.Floor(Math.Min(p1.Y, Math.Min(p2.Y, Math.Min(p3.Y, p4.Y))));
            var x2 = (int)Math.Floor(Math.Max(p1.X, Math.Max(p2.X, Math.Max(p3.X, p4.X))));
            var y2 = (int)Math.Floor(Math.Max(p1.Y, Math.Max(p2.Y, Math.Max(p3.Y, p4.Y))));
            var grid = new Int32Rect(x1, y1, x2 - x1 + 1, y2 - y1 + 1);

            if (tileZoomLevel != zoom || tileGrid != grid)
            {
                tileZoomLevel = zoom;
                tileGrid = grid;
                var tileLayerTransform = GetTileLayerTransformMatrix();

                foreach (TileLayer tileLayer in Children)
                {
                    tileLayer.SetTransformMatrix(tileLayerTransform);
                    tileLayer.UpdateTiles(tileZoomLevel, tileGrid);
                }
            }
        }

        private Matrix GetViewportTransformMatrix(double scale, double offsetX, double offsetY)
        {
            var transform = new Matrix(scale, 0d, 0d, -scale, offsetX, offsetY);

            transform.RotateAt(rotation, viewportOrigin.X, viewportOrigin.Y);

            return transform;
        }

        /// <summary>
        /// Gets a transform matrix with origin at tileGrid.X and tileGrid.Y to minimize rounding errors.
        /// </summary>
        private Matrix GetTileLayerTransformMatrix()
        {
            var scale = Math.Pow(2d, zoomLevel - tileZoomLevel);
            var transform = new Matrix(1d, 0d, 0d, 1d, tileGrid.X * TileSource.TileSize, tileGrid.Y * TileSource.TileSize);

            transform.Scale(scale, scale);
            transform.Translate(tileLayerOffset.X, tileLayerOffset.Y);
            transform.RotateAt(rotation, viewportOrigin.X, viewportOrigin.Y);

            return transform;
        }

        private Matrix GetTileIndexMatrix(int numTiles)
        {
            var scale = (double)numTiles / 360d;
            var transform = ViewportTransform.Matrix;

            transform.Invert(); // view to map coordinates
            transform.Translate(180d, -180d);
            transform.Scale(scale, -scale); // map coordinates to tile indices

            return transform;
        }
    }
}
