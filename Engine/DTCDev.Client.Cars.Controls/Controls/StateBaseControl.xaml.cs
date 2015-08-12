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
using DTCDev.Client.Cars.Engine.Handlers;
using DTCDev.Client.Cars.Engine.Handlers.Cars;

namespace DTCDev.Client.Cars.Controls.Controls
{
    /// <summary>
    /// Interaction logic for StateBaseControl.xaml
    /// </summary>
    public partial class StateBaseControl : UserControl
    {
        public StateBaseControl()
        {
            InitializeComponent();
        }

        Storyboard stbUpdate;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
                return;
            stbUpdate = (Storyboard)FindResource("stbUpdate");
            CarsHandler.Instance.StartLoadCarData += Instance_StartLoadCarData;
            try
            {
                Thread tr = new Thread(ThreadRefresh);
                tr.Start();
            }
            catch { }
        }

        void Instance_StartLoadCarData(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
                {
                    stbUpdate.Begin();
                }));
        }

        private void ThreadRefresh()
        {
            while (true)
            {
                try
                {
                    Thread.Sleep(1000);
                    if (Application.Current != null)
                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                DisplayConnection();
                                DisplayCarLoad();
                            }));
                }
                catch { }
            }
        }

        private void DisplayCarLoad()
        {
            if (CarsHandler.Instance.Cars.Count() > 0)
            {
                imgCarOk.Visibility = Visibility.Visible;
                imgCar.Visibility = Visibility.Collapsed;
            }
            else
            {
                imgCarOk.Visibility = Visibility.Collapsed;
                imgCar.Visibility = Visibility.Visible;
            }
        }

        private void DisplayConnection()
        {
            if (TCPConnection.Instance.IsConnected)
            {
                imgOffline.Visibility = Visibility.Collapsed;
                imgOnline.Visibility = Visibility.Visible;
            }
            else
            {
                imgOffline.Visibility = Visibility.Visible;
                imgOnline.Visibility = Visibility.Collapsed;
            }
        }
    }
}
