﻿using System;
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
            control._isPlayed = (bool)e.NewValue;
            if (control._isPlayed) control.PlayLoop();
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
            typeof(DateTime), typeof(PlayerControl), new PropertyMetadata(OnChangeCurentTime));

        private static void OnChangeCurentTime(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var control = (PlayerControl)sender;
            if (control == null) return;
            control._curentTime = (DateTime)e.NewValue;
        }

        #endregion


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        #region Fields

        int _speedValue = 5;
        bool _isPlayed;
        DateTime _startTime;
        DateTime _endTime;
        DateTime _curentTime;
        #endregion

        #region Properties


        public int SpeedValue
        {
            get { return _speedValue; }
            set { _speedValue = value; }
        }

        public bool IsPlayed
        {
            get { return (bool)GetValue(IsPlayedProperty); }
            set
            {
                SetValue(IsPlayedProperty, value);
                ToPlay.Visibility = value ? Visibility.Collapsed : Visibility.Visible;
                ToStop.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
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
            set{SetValue(CurentTimeProperty, value);}
        }

        #endregion

        #region Commands

        private void ToStart_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            IsPlayed = false;
            CurentTime = StartTime;
        }

        private void ToBack_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            IsPlayed = false;
            var time = CurentTime - new TimeSpan(SpeedValue * 1000000000);
            if (time < StartTime) time = StartTime;
            CurentTime = time;
        }

        private void ToStop_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            IsPlayed = false;
        }

        private void ToPlay_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            IsPlayed = true;
        }

        private void ToForward_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            IsPlayed = false;
            var time = CurentTime + new TimeSpan(SpeedValue * 1000000000);
            if (time > EndTime) time = EndTime;
            CurentTime = time;
        }

        private void ToEnd_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
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
                while (_isPlayed)
                {
                    var time = _curentTime + new TimeSpan(SpeedValue * 1000000);
                    if (time >= _endTime)
                    {
                        time = _endTime;
                        _isPlayed = false;
                    }
                    if (Application.Current != null)
                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            if (_isPlayed)
                                CurentTime = time;

                        }));
                    Thread.Sleep(10);
                }
            });
            slowTask.ContinueWith(o =>
            {
                if (Application.Current != null)
                    Application.Current.Dispatcher.BeginInvoke(new Action(() => IsPlayed = false));
            });
            slowTask.Start();

        }

        #endregion
    }
}
