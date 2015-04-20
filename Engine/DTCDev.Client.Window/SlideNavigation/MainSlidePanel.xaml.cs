using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DTCDev.Client.Window.SlideNavigation
{
    /// <summary>
    /// Interaction logic for MainSlidePanel.xaml
    /// </summary>
    public partial class MainSlidePanel : UserControl
    {
        public MainSlidePanel()
        {
            InitializeComponent();
        }

        private double h = 100;
        private double w = 100;
        private bool _loadComplete = false;
        private int _animationTimeMilliseconds = 300;
        private bool _lightTheme = false;

        public bool LightTheme
        {
            get { return _lightTheme; }
            set
            {
                _lightTheme = value;
                if (value)
                {
                    txtHeader.Foreground = new SolidColorBrush(Colors.White);
                }
                else
                {
                    txtHeader.Foreground = new SolidColorBrush(Colors.Black);
                }
            }
        }


        public void AddFrame(SlideFrameControl control)
        {
            if (_loadComplete)
            {
                _slideStack.Add(control);
                AnimateForward(control);
            }
            else
            {
                _preLoadingTempStorage.Add(control);
            }
        }

        private void brdrDimensionDetector_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (brdrDimensionDetector != null)
            {
                if (Double.IsNaN(brdrDimensionDetector.Width)==false)
                    w = brdrDimensionDetector.Width;
                if (Double.IsNaN(brdrDimensionDetector.Height)==false)
                    h = brdrDimensionDetector.Height;

                ResetDimensions();
            }
        }
        
        private List<SlideFrameControl> _slideStack = new List<SlideFrameControl>();

        private List<SlideFrameControl> _preLoadingTempStorage = new List<SlideFrameControl>();

        private List<Grid> _slides = new List<Grid>();

        private void AnimateForward(SlideFrameControl control)
        {
            if (w == double.NaN)
                return;
            Grid g = new Grid();
            g.Width = w;
            g.Height = h;
            g.Children.Add(control);
            if (_slides.Count() > 0)
                g.Background = new SolidColorBrush(Colors.White);
            
            canvas.Children.Add(g);
            Canvas.SetLeft(g, w);
            DoubleAnimation da = new DoubleAnimation
            {
                From = w,
                To = 0,
                Duration = TimeSpan.FromMilliseconds(_animationTimeMilliseconds)
            };
            Storyboard.SetTarget(da, g);
            Storyboard.SetTargetProperty(da, new PropertyPath(Canvas.LeftProperty));
            Storyboard sb = new Storyboard();
            sb.Children.Add(da);
            sb.Begin();
            _slides.Add(g);
            txtHeader.Text = control.Name;
        }

        private void AnimateBackward()
        {
            if (_slides.Count() < 2)
                return;
            else
            {
                Grid g = _slides[_slides.Count() - 1];
                DoubleAnimation da = new DoubleAnimation
                {
                    From = 0,
                    To = w,
                    Duration = TimeSpan.FromMilliseconds(_animationTimeMilliseconds / 2)
                };
                Storyboard.SetTarget(da, g);
                Storyboard.SetTargetProperty(da, new PropertyPath(Canvas.LeftProperty));
                Storyboard sb = new Storyboard();
                sb.Children.Add(da);
                sb.Completed += sb_Completed;
                sb.Begin();
            }
        }

        void sb_Completed(object sender, EventArgs e)
        {
            _slides.RemoveAt(_slides.Count() - 1);
            _slideStack.RemoveAt(_slideStack.Count() - 1);
            txtHeader.Text = _slideStack[_slideStack.Count() - 1].Name;
        }

        private void ResetDimensions()
        {
            foreach (var item in _slideStack)
            {
                item.Width = w;
                item.Height = h;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            w = brdrDimensionDetector.ActualWidth;
            h = brdrDimensionDetector.ActualHeight;
            if (Double.IsNaN(w))
                w = 1280;
            if (Double.IsNaN(h))
                h = 800;
            _loadComplete = true;
            CheckStorage();
        }

        private void CheckStorage()
        {
            while (_preLoadingTempStorage.Count() > 0)
            {
                AddFrame(_preLoadingTempStorage[0]);
                _preLoadingTempStorage.RemoveAt(0);
            }
        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            AnimateBackward();
        }
    }
}
