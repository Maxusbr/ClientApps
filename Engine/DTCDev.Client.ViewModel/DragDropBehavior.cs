using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace DTCDev.Client.ViewModel
{
    public class DragDropBehavior : Behavior<UIElement>
    {
        #region Private Fields
        private Point _startPosition;
        private Point _mouseStartPosition;
        private UIElement _associatedObject = null;
        private Canvas _parent = null;
        #endregion

        #region Overriden Members
        protected override void OnAttached()
        {
            base.OnAttached();


            _associatedObject = AssociatedObject;
            //TODO: set the parent accordingly
            _parent = FindAnchestor<Canvas>(AssociatedObject);
            //_parent = ((FrameworkElement) AssociatedObject).Parent as Canvas;
            _associatedObject.MouseLeftButtonDown += AssociatedObjectMouseLeftButtonDown;
            _associatedObject.MouseLeftButtonUp += AssociatedObjectMouseLeftButtonUp;
            _associatedObject.MouseMove += AssociatedObjectMouseMove;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            _associatedObject.MouseLeftButtonDown -= AssociatedObjectMouseLeftButtonDown;
            _associatedObject.MouseLeftButtonUp -= AssociatedObjectMouseLeftButtonUp;
            _associatedObject.MouseMove -= AssociatedObjectMouseMove;
        }

        #endregion
        private bool _isMouseCaptured = false;
        #region Event Handlers
        void AssociatedObjectMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _startPosition = _associatedObject.TranslatePoint(new Point(), _parent);
            _mouseStartPosition = e.GetPosition(_parent);
            _isMouseCaptured = true; 
            _associatedObject.CaptureMouse();
            if (_parent != null) _parent.Cursor = System.Windows.Input.Cursors.Hand;
        }

        void AssociatedObjectMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (_parent != null)
            {
                var positionDifference = e.GetPosition(_parent) - _mouseStartPosition;
                if (_isMouseCaptured)
                {
                    _associatedObject.SetValue(Canvas.TopProperty, _startPosition.Y + positionDifference.Y);

                    _associatedObject.SetValue(Canvas.LeftProperty, _startPosition.X + positionDifference.X);
                }
            }
        }

        void AssociatedObjectMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _associatedObject.ReleaseMouseCapture();
            _isMouseCaptured = false;
        }

        #endregion

        private static T FindAnchestor<T>(DependencyObject current)
    where T : DependencyObject
        {
            do
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }
    }
}
