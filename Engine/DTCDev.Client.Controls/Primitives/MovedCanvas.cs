using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DTCDev.Client.Controls.Map
{
    public class MovedCanvas : Canvas, IMapElement
    {
        private MapBase parentMap;
        private System.Windows.Point? mousePosition;

        public MapBase ParentMap
        {
            get { return parentMap; }
            set
            {
                parentMap = value;
            }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            if (CaptureMouse())
            {
                mousePosition = e.GetPosition(ParentMap);
                ParentMap.MouseMove += ParentMap_MouseMove;
            }
        }

        private void ParentMap_MouseMove(object sender, MouseEventArgs e)
        {
            OnMouseMove(e);
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);

            if (mousePosition.HasValue)
            {
                ParentMap.MouseMove -= ParentMap_MouseMove;
                mousePosition = null;
                ReleaseMouseCapture();
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (mousePosition.HasValue)
            {
                var position = e.GetPosition(ParentMap);
                MovedLocation ml = this.DataContext as MovedLocation;
                if (ml != null)
                {
                    Location CurrentLocation = ParentMap.ViewportPointToLocation(position);
                    TemplatedParent.SetValue(MapPanel.LocationProperty, CurrentLocation);
                    ml.Latitude = CurrentLocation.Latitude;
                    ml.Longitude = CurrentLocation.Longitude;
                    ml.Update();
                }
                else
                {
                    if(ParentMap != null)
                        ParentMap.TranslateMap((Point)(position - mousePosition));
                }
                mousePosition = position;
            }
        }

        protected override void OnMouseRightButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseRightButtonDown(e);
            MovedLocation ml = this.DataContext as MovedLocation;
            if (ml != null)
                ml.Remove();
        }
    }
}
