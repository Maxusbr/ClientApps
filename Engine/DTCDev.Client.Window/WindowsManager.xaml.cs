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
    /// Interaction logic for WindowsManager.xaml
    /// </summary>
    public partial class WindowsManager : UserControl
    {
        private int _tempLeft = 0;

        private int _tempTop = 0;

        private const int HORIZONTAL_STEP = 20;

        private const int VERTICAL_STEP = 20;

        private int currentZIndex = 1;

        public string SettingsPath = string.Empty;


        public static DependencyProperty backgroundProperty = DependencyProperty.Register("Background", typeof(Brush), typeof(WindowsManager), new PropertyMetadata(null, OnBackgroundUpdate));

        public Brush Background
        {
            get { return (Brush)GetValue(backgroundProperty); }
            set { SetValue(backgroundProperty, value); }
        }

        private static void OnBackgroundUpdate(object sender, DependencyPropertyChangedEventArgs e)
        {
            WindowsManager win = sender as WindowsManager;
            if (win.Background != null)
                win.LayoutRoot.Background = win.Background;
        }




        /// <summary>
        /// Коллекция открытых окон
        /// </summary>
        private List<WindowExemplar> _openedWins = new List<WindowExemplar>();
        public List<WindowExemplar> OpenedWins
        {
            get { return _openedWins; }
            set { _openedWins = value; }
        }

        public WindowsManager()
        {
            InitializeComponent();

            OpenedWins.Clear();
        }

        //маршрутизируемые события
        public event EventHandler MouseEnterWindows;

        public event EventHandler MouseLeaveWindows;

        public event EventHandler CloseWindows;


        private void MouseEnterWindow(object sender, EventArgs args)
        {
            if (MouseEnterWindows != null)
                MouseEnterWindows(sender, args);
        }

        private void MouseLeaveWindow(object sender, EventArgs args)
        {
            if (MouseLeaveWindows != null)
                MouseLeaveWindows(sender, args);
        }

        /// <summary>
        /// Добавление нового окна
        /// </summary>
        /// <param name="opened">экземпляр нового окна</param>
        /// <param name="info">дополнительная информация об окне</param>
        public void OpenWindow(WindowExemplar opened, string info="", bool saveToConfig=true)
        {
            WindowExemplar we = OpenedWins.Where(p => p.Title == opened.Title).FirstOrDefault();
            if (we != null)
                if (we.AllowDuplicate == false)
                {
                    if (we.Visibility == Visibility.Collapsed)
                    {
                        we.Visibility = Visibility.Visible;
                        return;
                    }
                    else
                        return;
                }
            //подключаем обработчики событий изменения окна
            opened.Moved += new EventHandler(MoveWindow);
            opened.Resized += new EventHandler(ResizeWindow);
            opened.Minimized += new EventHandler(MinimizeWindow);
            opened.Maximized += new EventHandler(MaximizeWindow);
            opened.Closed += new EventHandler(CloseWindow);
            opened.MouseEnter += new EventHandler(MouseEnterWindow);
            opened.MouseLeave += new EventHandler(MouseLeaveWindow);
            opened.Clicked += new EventHandler(opened_Clicked);

            //добавление в драйвер и коллекцию менеджера
            AddWindow(opened, -1, -1, saveToConfig, info);
        }

        /// <summary>
        /// Добавление нового окна
        /// </summary>
        /// <param name="opened">экземпляр нового окна</param>
        /// <param name="left">left-position</param>
        /// <param name="top">top-position</param>
        /// <param name="info">дополнительная информация об окне</param>
        public void OpenWindow(WindowExemplar opened, double left, double top, string info="", bool saveToConfig=true)
        {
            WindowExemplar we = OpenedWins.Where(p => p.Title == opened.Title).FirstOrDefault();
            if (we != null)
                if (we.AllowDuplicate == false)
                {
                    if (we.Visibility == Visibility.Collapsed)
                    {
                        we.Visibility = Visibility.Visible;
                        return;
                    }
                    else
                        return;
                }
            //подключаем обработчики событий изменения окна
            opened.Moved += new EventHandler(MoveWindow);
            opened.Resized += new EventHandler(ResizeWindow);
            opened.Minimized += new EventHandler(MinimizeWindow);
            opened.Maximized += new EventHandler(MaximizeWindow);
            opened.Closed += new EventHandler(CloseWindow);
            opened.MouseEnter += new EventHandler(MouseEnterWindow);
            opened.MouseLeave += new EventHandler(MouseLeaveWindow);
            opened.Clicked += new EventHandler(opened_Clicked);

            //добавление в драйвер и коллекцию менеджера
            AddWindow(opened, left, top, saveToConfig, info);
        }

        void opened_Clicked(object sender, EventArgs e)
        {
            PlaceWinToTop(sender as WindowExemplar);
        }

        /// <summary>
        /// Добавление в драйвер и коллекцию окон менеджера
        /// </summary>
        /// <param name="window">Экземпляр нового окна</param>
        private void AddWindow(WindowExemplar window, double left, double top, bool saveSettings, string info = "")
        {
            window.Index = window.Index;
            window.SM.LinkId = info;
            OpenedWins.Add(window);
            LayoutRoot.Children.Add(window);
            if (left < 0 || top < 0)
            {

                Canvas.SetLeft(window, this._tempLeft);
                Canvas.SetTop(window, this._tempTop);

                this._tempLeft += HORIZONTAL_STEP;
                this._tempTop += VERTICAL_STEP;

                window.CurrentPosition = new Point(this._tempLeft, this._tempTop);

                if (this._tempLeft > this.Width - 400)
                    this._tempLeft = 0;
                if (this._tempTop > this.Height - 300)
                    this._tempTop = 0;
            }
            else
            {
                Canvas.SetLeft(window, left);
                Canvas.SetTop(window, top);
                window.CurrentPosition = new Point(left, top);
            }
            Canvas.SetZIndex(window, this.currentZIndex);
            this.currentZIndex++;
            if (saveSettings)
                SaveWindowsSettings();
        }

        /// <summary>
        /// Обработчик события закрытия окна
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void CloseWindow(object sender, EventArgs args)
        {
            WindowExemplar window = sender as WindowExemplar;
            OpenedWins.Remove(window);
            LayoutRoot.Children.Remove(window);
            if (CloseWindows != null)
                CloseWindows(window, new EventArgs());
            SaveWindowsSettings();
        }

        /// <summary>
        /// Обработчик события изменения размера окна
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void ResizeWindow(object sender, EventArgs args)
        {
            WindowExemplar window = sender as WindowExemplar;
            PlaceWinToTop(window);
            OpenedWins[OpenedWins.IndexOf(window)] = window;
            SaveWindowsSettings();
        }

        /// <summary>
        /// Обработчик события перемещения окна
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void MoveWindow(object sender, EventArgs args)
        {
            WindowExemplar window = sender as WindowExemplar;
            PlaceWinToTop(window);
            OpenedWins[OpenedWins.IndexOf(window)] = window;
            SaveWindowsSettings();
        }

        /// <summary>
        /// Обработчик события сворачивания окна
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void MinimizeWindow(object sender, EventArgs args)
        {
            WindowExemplar window = sender as WindowExemplar;
            RenderTargetBitmap rtb = new RenderTargetBitmap(
                (int)window.WindowWidth,
                (int)window.WindowHeight,
                96,
                96,
                PixelFormats.Pbgra32);
            rtb.Render(window.LayoutRoot);
            Grid g = new Grid();
            g.Tag = window;
            g.MouseLeftButtonUp += g_MouseLeftButtonUp;
            g.Width = g.Height = 100;
            Border b = new Border();
            b.Background = new SolidColorBrush(Colors.Black);
            b.Opacity = 0.3;
            g.Children.Add(b);
            Image img = new Image { Source = rtb, Width = 100, Height = 100, Stretch = System.Windows.Media.Stretch.UniformToFill };
            g.Children.Add(img);
            stkOpened.Children.Add(g);
            window.Visibility = Visibility.Collapsed;
            AddWindowToMinList(window);
            MaximazeMinimazeWin(window, true);
        }

        void g_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Grid g = (Grid)sender;
            WindowExemplar window = (WindowExemplar)g.Tag;
            window.Visibility = Visibility.Visible;
            stkOpened.Children.Remove(g);
        }

        private List<string> minimizedImagesList = new List<string>();

        private void AddWindowToMinList(WindowExemplar win)
        {
            minimizedImagesList.Add(win.Title);
        }



        /// <summary>
        /// Передвинуть окно на передний план
        /// </summary>
        public void ShowWindow(int ind)
        {
            foreach (var item in LayoutRoot.Children)
            {
                if (item is WindowExemplar)
                {
                    if ((item as WindowExemplar).Index == ind)
                    {
                        (item as WindowExemplar).Visibility = Visibility.Visible;
                        //GetDriver().MinimizeWindow(ind, true);
                        PlaceWinToTop(item as WindowExemplar);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Обработчик события разворачивания окна на весь экран
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void MaximizeWindow(object sender, EventArgs args)
        {
            WindowExemplar window = sender as WindowExemplar;

            window.grdContent.Height = LayoutRoot.ActualHeight - 30;
            window.grdContent.Width = LayoutRoot.ActualWidth - 5;
            window.SetValue(Canvas.LeftProperty, (double)2);
            window.SetValue(Canvas.TopProperty, (double)2);

            //вызываем события перемещения и растяжения
            MoveWindow(sender, args);
            ResizeWindow(sender, args);
            MaximazeMinimazeWin(window, false);
        }

        private void PlaceWinToTop(WindowExemplar win)
        {
            foreach (var item in LayoutRoot.Children)
            {
                Canvas.SetZIndex((UIElement)item, 0);
            }
            Canvas.SetZIndex(win, 1);
        }

        UIElement _displayedList;

        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_displayedList == null)
            {
                int col = minimizedImagesList.Count();
                if (col < 1)
                    return;

                Canvas displayer = new Canvas();

                if (col > 10)
                {
                    ScrollViewer sv = new ScrollViewer();
                    sv.Height = 400;
                    sv.Width = 200;
                    sv.Content = displayer;
                    _displayedList = sv;
                }
                else
                    _displayedList = displayer;

                displayer.Height = col * 40;
                displayer.Width = 200;
                displayer.Background = new SolidColorBrush(Colors.Black);
                for (int i = 0; i < col; i++)
                {
                    TextBlock txt = new TextBlock();
                    txt.Text = minimizedImagesList[i];
                    txt.FontSize = 14;
                    //txt.
                }
            }
            else
            {

            }
        }

        private void MaximazeMinimazeWin(WindowExemplar window, bool maximazed)
        {
            OpenedWins[OpenedWins.IndexOf(window)].SM.Minimized = !maximazed;
            OpenedWins[OpenedWins.IndexOf(window)].SM.Maximize = maximazed;
            SaveWindowsSettings();
        }

        private void SaveWindowsSettings()
        {
            if(string.IsNullOrEmpty(SettingsPath)) return;
            SettingsLoader.Instance.SaveSettings(SettingsPath, OpenedWins.Select(o => o.SM));
        }

        public IEnumerable<SettingsModel> GetSettingsWindows()
        {
            return SettingsLoader.Instance.GetSettings(SettingsPath);
        }
    }
}
