using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using DTCDev.Client.Cars.Controls.ViewModels.History;
using System.Windows.Media.Animation;
using System.Windows.Controls.Primitives;

namespace DTCDev.Client.Cars.Controls.Controls.History
{
    /// <summary>
    /// Interaction logic for HistoryControl.xaml
    /// </summary>
    public partial class HistoryControl : UserControl
    {
        readonly HistoryViewModel _hvm;

        public HistoryControl()
        {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this))
                return;
             _hvm = this.DataContext as HistoryViewModel;
        }

        public HistoryControl(DateTime date)
        {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this))
                return;
            this.DataContext = _hvm;
            _hvm.StartDate = date;
            _hvm.StopDate = date + new TimeSpan(1, 0, 0, 0);
        }

        void hvm_HideSelectDate(object sender, EventArgs e)
        {
            //stbHideDate.Begin();
        }

        void hvm_DisplaySelectDate(object sender, EventArgs e)
        {
            //stbShowDate.Begin();
        }


        private void Image_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {

        }

        private bool _playerVisible = false;

        private void Image_MouseLeftButtonUp_2(object sender, MouseButtonEventArgs e)
        {
            //if (_playerVisible)
            //{
            //    grdMove.Visibility = Visibility.Visible;
            //    grdPlayer.Visibility = System.Windows.Visibility.Collapsed;
            //}
            //else
            //{
            //    grdMove.Visibility = Visibility.Collapsed;
            //    grdPlayer.Visibility = System.Windows.Visibility.Visible;
            //}
            //_playerVisible = !_playerVisible;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
        }

        bool _displayHistory=false;

        public void HistoryButton_Click()
        {
            if (_displayHistory == false)
            {
                CarPoints.Opacity = .5;
                _hvm.EnableHistory = true;
                _hvm.LoadData();
                grdHistoryWork.Visibility = Visibility.Visible;
            }
            else
            {
                CarPoints.Opacity = 1;
                _hvm.EnableHistory = false;
                grdHistoryWork.Visibility = Visibility.Collapsed;
            }
            _displayHistory = !_displayHistory;
            CarPin.Visibility = ParkingsPin.Visibility =
            RouteLine.Visibility = WarningLine.Visibility = ErrorLine.Visibility = OfflineLine.Visibility =
            _carZonesError.Visibility = grdHistoryWork.Visibility;
        }

        private void grdHistoryWork_MouseEnter(object sender, MouseEventArgs e)
        {
            grdHistoryWork.Opacity = 1;
        }

        private void grdHistoryWork_MouseLeave(object sender, MouseEventArgs e)
        {
            grdHistoryWork.Opacity = 0.4f;
        }

        private bool _displayedDistance = false;
        private void btnDistance_Click(object sender, RoutedEventArgs e)
        {
            if (_displayedDistance)
                grdDistance.Visibility = Visibility.Collapsed;
            else
                grdDistance.Visibility = Visibility.Visible;
            _displayedDistance = !_displayedDistance;
        }

    }
}
