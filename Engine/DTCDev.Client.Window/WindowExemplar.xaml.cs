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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DTCDev.Client.Window
{
    /// <summary>
    /// Interaction logic for WindowExemplar.xaml
    /// </summary>
    public partial class WindowExemplar : UserControl
    {
        public WindowExemplar()
        {
            InitializeComponent();
        }

        public WindowExemplar(bool canUserResize)
        {
            InitializeComponent();
            _canUserResize = canUserResize;
            if (_canUserResize == false)
                btnMaximize.Visibility = Visibility.Collapsed;
        }

        public WindowExemplar(bool canUserResize, bool allowDuplicate)
        {
            InitializeComponent();
            _canUserResize = canUserResize;
            if (_canUserResize == false)
                btnMaximize.Visibility = Visibility.Collapsed;
        }

        public WindowExemplar(bool canUserResize, bool allowDuplicate, bool canUserClose)
        {
            InitializeComponent();
            _allowDuplicate = allowDuplicate;
            _canUserResize = canUserResize;
            _canUserClose = canUserClose;
            if (_canUserResize == false)
                btnMaximize.Visibility = Visibility.Collapsed;
            if (_canUserClose == false)
                btnClose.Visibility = Visibility.Collapsed;
        }
        public SettingsModel SM = new SettingsModel(){Left = 0, Top = 0, Width = 0, Height = 0, Maximize = false, Minimized = false};
        private double width = 0;

        private double height = 0;

        bool _canUserResize = true;

        bool _canUserClose = true;

        bool _allowDuplicate = false;

        public bool AllowDuplicate
        {
            get { return _allowDuplicate; }
            set { _allowDuplicate = value; }
        }

        public int Index { get; set; }

        public event EventHandler Clicked;

        public event EventHandler Moved;

        public event EventHandler Resized;

        public event EventHandler Closed;

        public event EventHandler Minimized;

        public event EventHandler Maximized;

        public event EventHandler MouseEnter;

        public event EventHandler MouseLeave;

        public UIElement Context
        {
            get
            {
                if (grdContent.Children.Count > 0)
                    return grdContent.Children[0];
                else
                    return null;
            }
            set
            {
                grdContent.Children.Clear();
                grdContent.Children.Add(value);
            }
        }
        private string _title = "";

        public string Title
        {
            set { SM.Name = _title = value; }
            get { return _title; }
        }

        public double WindowWidth
        {
            get { return (int)this.ActualWidth; }
            set
            {
                this.grdContent.Width = value;
                SM.Width = width = value;
            }
        }

        public double WindowHeight
        {
            get { return (int)this.ActualHeight; }
            set
            {
                this.grdContent.Height = value;
                SM.Height = height = value;
            }
        }

        private Point _currentPosition = new Point(0, 0);

        public Point CurrentPosition
        {
            get { return _currentPosition; }
            set
            {
                _currentPosition = value;
                SM.Left = value.X;
                SM.Top = value.Y;
            }
        }

        private int _userControlID = -1;

        public int UserControlID
        {
            get { return _userControlID; }
            set
            {
                _userControlID = value;
            }
        }




        #region move window
        //******************************************
        //           перемещение окна
        //******************************************

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Clicked != null)
                Clicked(this, new EventArgs());
            brdrRoot.MouseLeftButtonUp += new MouseButtonEventHandler(brdrRoot_MouseLeftButtonUp);
            brdrRoot.MouseMove += new MouseEventHandler(brdrRoot_MouseMove);
            brdrRoot.CaptureMouse();
            pPrevios.X = e.GetPosition(this).X;
            pPrevios.Y = e.GetPosition(this).Y;
        }

        Point pPrevios;

        void brdrRoot_MouseMove(object sender, MouseEventArgs e)
        {
            Point p = e.GetPosition(brdrRoot);
            if (p != pPrevios)
            {
                double left = Canvas.GetLeft(this);
                double top = Canvas.GetTop(this);
                left += p.X - pPrevios.X;
                top += p.Y - pPrevios.Y;
                if (left < 0)
                    left = 0;
                if (top < 0)
                    top = 0;
                Canvas.SetLeft(this, left);
                Canvas.SetTop(this, top);
                CurrentPosition = new Point(left, top);
            }
        }

        void brdrRoot_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Moved != null)
                Moved(this, new EventArgs());
            brdrRoot.MouseMove -= brdrRoot_MouseMove;
            brdrRoot.ReleaseMouseCapture();
        }


        #endregion





        #region change window size

        //******************************************
        //           растяжение окна
        //******************************************

        //горизонтальное
        private void brdrStretchRight_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_canUserResize)
            {
                brdrStretchRight.MouseLeftButtonUp += new MouseButtonEventHandler(brdrStretchRight_MouseLeftButtonUp);
                brdrStretchRight.MouseMove += new MouseEventHandler(brdrStretchRight_MouseMove);
                brdrStretchRight.CaptureMouse();
            }
        }

        void brdrStretchRight_MouseMove(object sender, MouseEventArgs e)
        {
            Point p = e.GetPosition(brdrStretchRight);
            double wd = grdContent.Width + p.X;
            if (wd > 1)
            {
                WindowWidth = wd;
            }
        }

        void brdrStretchRight_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Resized != null)
                Resized(this, new EventArgs());
            brdrStretchRight.MouseLeftButtonUp -= brdrStretchRight_MouseLeftButtonUp;
            brdrStretchRight.MouseMove -= brdrStretchRight_MouseMove;
            brdrStretchRight.ReleaseMouseCapture();
        }


        //вертикальное
        private void brdrStretchBottom_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_canUserResize)
            {
                brdrStretchBottom.MouseLeftButtonUp += new MouseButtonEventHandler(brdrStretchBottom_MouseLeftButtonUp);
                brdrStretchBottom.MouseMove += new MouseEventHandler(brdrStretchBottom_MouseMove);
                brdrStretchBottom.CaptureMouse();
            }
        }

        void brdrStretchBottom_MouseMove(object sender, MouseEventArgs e)
        {
            Point p = e.GetPosition(brdrStretchBottom);
            double hg = grdContent.Height + p.Y;
            if (hg > 1)
            {
                WindowHeight = hg;
            }
        }

        void brdrStretchBottom_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Resized != null)
                Resized(this, new EventArgs());
            brdrStretchBottom.MouseLeftButtonUp -= brdrStretchBottom_MouseLeftButtonUp;
            brdrStretchBottom.MouseMove -= brdrStretchBottom_MouseMove;
            brdrStretchBottom.ReleaseMouseCapture();
        }


        //горизонтальное + вертикальное
        private void brdrStretchBR_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            brdrStretchBR.MouseLeftButtonUp += new MouseButtonEventHandler(brdrStretchBR_MouseLeftButtonUp);
            brdrStretchBR.MouseMove += new MouseEventHandler(brdrStretchBR_MouseMove);
            brdrStretchBR.CaptureMouse();
        }

        void brdrStretchBR_MouseMove(object sender, MouseEventArgs e)
        {
            Point p = e.GetPosition(brdrStretchBR);
            double wd = grdContent.Width + p.X;
            double hg = grdContent.Height + p.Y;
            if (wd > 1 && hg > 1)
            {
                WindowHeight = hg;
                WindowWidth = wd;
                //grdContent.Height = hg;
                //grdContent.Width = wd;
            }
        }

        void brdrStretchBR_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Resized != null)
                Resized(this, new EventArgs());
            brdrStretchBR.MouseLeftButtonUp -= brdrStretchBR_MouseLeftButtonUp;
            brdrStretchBR.MouseMove -= brdrStretchBR_MouseMove;
            brdrStretchBR.ReleaseMouseCapture();
        }

        #endregion





        #region Buttons

        //******************************************
        //           обработка кнопок
        //******************************************

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            if (Closed != null)
                Closed(this, new EventArgs());
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            if (Minimized != null)
                Minimized(this, new EventArgs());
        }

        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (Maximized != null)
                Maximized(this, new EventArgs());
        }

        private void LayoutRoot_MouseEnter(object sender, MouseEventArgs e)
        {
            if (MouseEnter != null)
                MouseEnter(this, new EventArgs());
            //((Storyboard)LayoutRoot.Resources["Up"]).Begin();
        }

        private void LayoutRoot_MouseLeave(object sender, MouseEventArgs e)
        {
            if (MouseLeave != null)
                MouseLeave(this, new EventArgs());
            //((Storyboard)LayoutRoot.Resources["Down"]).Begin();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            txtblTitle.Text = _title;
        }

        #endregion
    }
}
