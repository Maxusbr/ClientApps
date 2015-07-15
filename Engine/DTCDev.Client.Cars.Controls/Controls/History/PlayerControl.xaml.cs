using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DTCDev.Client.Cars.Controls.ViewModels.History;

namespace DTCDev.Client.Cars.Controls.Controls.History
{
    /// <summary>
    /// Логика взаимодействия для PlayerControl.xaml
    /// </summary>
    public partial class PlayerControl : UserControl
    {
        public PlayerControl()
        {
            InitializeComponent();
        }

        #region DependencyProperties

        private static readonly DependencyProperty IsPlayedProperty =
            DependencyProperty.Register("IsPlayed", typeof(bool), typeof(PlayerControl),
                new PropertyMetadata(OnChangeIsPlayed));

        private static void OnChangeIsPlayed(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var control = (PlayerControl)sender;
            if (control == null) return;
            control.IsPlayed = (bool)e.NewValue;
        }

        private static readonly DependencyProperty StartTimeProperty = DependencyProperty.Register("StartTime",
            typeof(DateTime), typeof(PlayerControl), new PropertyMetadata(OnChangeStartTime));

        private static void OnChangeStartTime(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var control = (PlayerControl)sender;
            if (control == null) return;
            control.SetStartTime((DateTime)e.NewValue);
        }

        private static readonly DependencyProperty EndTimeProperty = DependencyProperty.Register("EndTime",
            typeof(DateTime), typeof(PlayerControl), new PropertyMetadata(OnChangeEndTime));

        private static void OnChangeEndTime(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var control = (PlayerControl)sender;
            if (control == null) return;
            control.EndTime = (DateTime)e.NewValue;
        }

        private static readonly DependencyProperty CurentTimeProperty = DependencyProperty.Register("CurentTime",
            typeof(DateTime), typeof(PlayerControl), new PropertyMetadata(null));

        #endregion


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        #region Fields

        private int _speedValue = 5;
        private bool _isPlayed;
        private DateTime _startTime;
        private DateTime _endTime;
        private DateTime _curentTime;
        #endregion

        #region Properties


        public int SpeedValue
        {
            get { return _speedValue; }
            set { _speedValue = value; }
        }

        public bool IsPlayed
        {
            get { return _isPlayed; }
            set
            {
                _isPlayed = value;
                ToPlay.Visibility = value ? Visibility.Collapsed : Visibility.Visible;
                ToStop.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
                if (value) PlayLoop();
            }
        }

        public DateTime StartTime
        {
            get { return _startTime; }
            set { _startTime = value; }
        }

        public DateTime EndTime
        {
            get { return _endTime; }
            set { _endTime = value; }
        }

        public DateTime CurentTime
        {
            get { return (DateTime)GetValue(CurentTimeProperty); }
            set
            {
                SetValue(CurentTimeProperty, value);
                _curentTime = value;
            }
        }

        #endregion

        #region Commands

        private void ToStart_Click(object sender, RoutedEventArgs e)
        {
            IsPlayed = false;
            CurentTime = StartTime;
        }

        private void ToBack_Click(object sender, RoutedEventArgs e)
        {
            IsPlayed = false;
            var time = CurentTime - new TimeSpan(SpeedValue * 1000000000);
            if (time < StartTime) time = StartTime;
            CurentTime = time;
        }

        private void ToStop_Click(object sender, RoutedEventArgs e)
        {
            IsPlayed = false;
        }

        private void ToPlay_Click(object sender, RoutedEventArgs e)
        {
            IsPlayed = true;
        }

        private void ToForward_Click(object sender, RoutedEventArgs e)
        {
            IsPlayed = false;
            var time = CurentTime + new TimeSpan(SpeedValue * 1000000000);
            if (time > EndTime) time = EndTime;
            CurentTime = time;
        }

        private void ToEnd_Click(object sender, RoutedEventArgs e)
        {
            IsPlayed = false;
            CurentTime = EndTime;
        }

        #endregion

        #region Metods

        private void SetStartTime(DateTime dateTime)
        {
            CurentTime = StartTime = dateTime;
        }

        private void PlayLoop()
        {
            var slowTask = new Task(delegate
            {
                while (true)
                {
                    if (!_isPlayed) break;
                    var time = _curentTime + new TimeSpan(SpeedValue * 1000000);
                    if (time >= _endTime)
                    {
                        time = _endTime;
                        _isPlayed = false;
                    }
                    if (Application.Current != null)
                        Application.Current.Dispatcher.BeginInvoke(new Action(() => CurentTime = time));

                    Thread.Sleep(10);
                }
                if (Application.Current != null)
                    Application.Current.Dispatcher.BeginInvoke(new Action(() => IsPlayed = false));
            });
            slowTask.Start();
        }

        #endregion

    }
}
