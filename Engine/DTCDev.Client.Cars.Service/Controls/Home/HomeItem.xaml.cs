using DTCDev.Client.Cars.Service.Engine.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace DTCDev.Client.Cars.Service.Controls.Home
{
    /// <summary>
    /// Interaction logic for HomeItem.xaml
    /// </summary>
    public partial class HomeItem : UserControl
    {
        public HomeItem()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                HomeDataModel model = (HomeDataModel)this.DataContext;
                if (model.Image != null && model.Image.Length > 2)
                {
                    BitmapImage b = new BitmapImage();
                    b.BeginInit();
                    b.UriSource = new Uri("/DTCDev.Client.Cars.Service;component/Assets/Images/" + model.Image, UriKind.Relative);
                    b.EndInit();
                    img.Source = b;
                }
            }
            catch { }
        }

        int _startAnimate = 0;

        private void ThreadUpdate()
        {
            bool phase1 = true;
            int await = 20 + 5 * _startAnimate;
            while (true)
            {
                Thread.Sleep(await*100);
                if (cnv1 == null || cnv2 == null)
                {
                    break;
                }
                else
                {
                    await = new Random().Next(30, 100);
                    if (phase1)
                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                AnimateUp();
                            }));
                    else
                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            AnimateDown();
                        }));

                    phase1 = !phase1;
                }
            }
        }

        public HomeDataModel Data
        {
            get { return (HomeDataModel)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        private static DependencyProperty DataProperty = DependencyProperty.Register("Data",
            typeof(HomeDataModel),
            typeof(HomeItem), 
            new PropertyMetadata(OnDataChanged));

        private static void OnDataChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            HomeItem control = (HomeItem)sender;
            if (e.NewValue != null)
                control.StartUpdate((HomeDataModel)e.NewValue);
        }

        private void StartUpdate(HomeDataModel data)
        {
            if (data == null)
                return;
            this.DataContext = data;
            brB.Visibility = brG.Visibility = brLB.Visibility = brP.Visibility = brR.Visibility = Visibility.Collapsed;

            switch (data.Color)
            {
                case HomeDataModel.BGColor.Blue:
                    brB.Visibility = Visibility.Visible;
                    break;
                case HomeDataModel.BGColor.Green:
                    brG.Visibility = Visibility.Visible;
                    break;
                case HomeDataModel.BGColor.LightBlue:
                    brLB.Visibility = Visibility.Visible;
                    break;
                case HomeDataModel.BGColor.Purpur:
                    brP.Visibility = Visibility.Visible;
                    break;
                case HomeDataModel.BGColor.Red:
                    brR.Visibility = Visibility.Visible;
                    break;
            }

            _startAnimate = data.AnimatePosition;
            new Thread(ThreadUpdate).Start();
        }

        private void AnimateUp()
        {
            DoubleAnimation da = new DoubleAnimation
            {
                From = 0,
                To = -150,
                Duration = TimeSpan.FromMilliseconds(500)
            };
            Storyboard.SetTarget(da, cnv1);
            Storyboard.SetTargetProperty(da, new PropertyPath(Canvas.TopProperty));
            Storyboard sb = new Storyboard();
            sb.Children.Add(da);
            sb.Begin();

            DoubleAnimation da1 = new DoubleAnimation
            {
                From = 150,
                To = 0,
                Duration = TimeSpan.FromMilliseconds(500)
            };
            Storyboard.SetTarget(da1, cnv2);
            Storyboard.SetTargetProperty(da1, new PropertyPath(Canvas.TopProperty));
            Storyboard sb1 = new Storyboard();
            sb1.Children.Add(da1);
            sb1.Begin();
        }

        private void AnimateDown()
        {
            DoubleAnimation da = new DoubleAnimation
            {
                From = -150,
                To = 0,
                Duration = TimeSpan.FromMilliseconds(500)
            };
            Storyboard.SetTarget(da, cnv1);
            Storyboard.SetTargetProperty(da, new PropertyPath(Canvas.TopProperty));
            Storyboard sb = new Storyboard();
            sb.Children.Add(da);
            sb.Begin();

            DoubleAnimation da1 = new DoubleAnimation
            {
                From = 0,
                To = 150,
                Duration = TimeSpan.FromMilliseconds(500)
            };
            Storyboard.SetTarget(da1, cnv2);
            Storyboard.SetTargetProperty(da1, new PropertyPath(Canvas.TopProperty));
            Storyboard sb1 = new Storyboard();
            sb1.Children.Add(da1);
            sb1.Begin();
        }



        public event EventHandler Click;

        private void UserControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Click != null)
                Click(this, new EventArgs());
        }
    }
}
