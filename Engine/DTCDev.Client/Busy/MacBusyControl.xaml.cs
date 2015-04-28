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

namespace DTCDev.Client.Busy
{
    /// <summary>
    /// Interaction logic for MacBusyControl.xaml
    /// </summary>
    public partial class MacBusyControl : UserControl
    {
        public MacBusyControl()
        {
            InitializeComponent();

            this.DefaultStyleKey = typeof(MacBusyControl);
            this.isInitialized = false;
        }

        #region IsWaiting  (DependencyProperty)

        /// <summary>
        /// IsWaiting Dependency Property
        /// </summary>
        public static readonly DependencyProperty IsWaitingProperty =
                DependencyProperty.Register("IsWaiting", typeof(bool), typeof(MacBusyControl),
                        new PropertyMetadata((bool)false,
                                new PropertyChangedCallback(OnIsWaitingChanged)));

        /// <summary>
        /// Gets or sets the IsWaiting property.  This dependency property 
        /// indicates is control value total waiting.
        /// </summary>
        public bool IsWaiting
        {
            get { return (bool)GetValue(IsWaitingProperty); }
            set { SetValue(IsWaitingProperty, value); }
        }

        /// <summary>
        /// Handles changes to the IsWaiting property.
        /// </summary>
        private static void OnIsWaitingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MacBusyControl source = (MacBusyControl)d;
            if (source != null)
            {
                source.UpdateUI();
            }
        }

        #endregion

        #region ForegroundAnimation (DependencyProperty)

        /// <summary>    
        /// Calabonga: Свойство  
        /// </summary>    
        public Brush ForegroundAnimation
        {
            get { return (Brush)GetValue(ForegroundAnimationProperty); }
            set { SetValue(ForegroundAnimationProperty, value); }
        }

        public static readonly DependencyProperty ForegroundAnimationProperty =
                DependencyProperty.Register(
                "ForegroundAnimation",
                typeof(Brush),
                typeof(MacBusyControl),
                new PropertyMetadata(new SolidColorBrush(Colors.Orange)));

        #endregion  ForegroundAnimation (DependencyProperty)

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            root = FindName(ElementRoot) as Grid;

            busyIndicator = FindName(ElementBusyIndicator) as Grid;
            WaitAnimation = FindResource("PATH_WaitStoryboard") as Storyboard;
            FrameworkContent = FindName(ElementFramework) as FrameworkElement;
            //if (root != null)
            //{
            //    WaitAnimation = root.Resources["PATH_WaitStoryboard"] as Storyboard;
            //}
            IndicatorAnimation = FindName(ElementCanvas1) as Canvas;
            if (busyIndicator != null)
            {
                busyIndicator.Visibility = System.Windows.Visibility.Visible;
            }
            if (root != null && busyIndicator != null &&
                FrameworkContent != null && WaitAnimation != null
                && IndicatorAnimation != null)
            {
                this.isInitialized = true;
                UpdateUI();
            }
        }

        private void UpdateUI()
        {
            if (isInitialized)
            {
                if (!this.IsWaiting)
                {
                    FrameworkContent.Opacity = 1;
                    busyIndicator.Visibility = System.Windows.Visibility.Collapsed;
                    WaitAnimation.Stop();
                }
                else
                {
                    FrameworkContent.Opacity = .1;
                    WaitAnimation.Stop();
                    WaitAnimation.Begin();
                    busyIndicator.Visibility = System.Windows.Visibility.Visible;
                }
            }
        }

        #region elements names
        private const string ElementRoot = "PATH_RootLoader";
        private const string ElementBusyIndicator = "PATH_BusyIndicatorLoader";
        private const string ElementFramework = "PATH_Framework";
        private const string ElementCanvas1 = "PATH_CanvasLoader1";
        #endregion elements names

        #region private member fields
        private Grid busyIndicator;
        private Grid root;
        private FrameworkElement FrameworkContent;
        private Storyboard WaitAnimation;
        private Canvas IndicatorAnimation;
        private bool isInitialized;
        #endregion private member fields
    }
}
