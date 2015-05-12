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

        private void Calendar_SelectedDatesChanged_1(object sender, SelectionChangedEventArgs e)
        {
            var calendar = sender as Calendar;
            if(calendar == null) return;
            _hvm.SetDates(calendar.SelectedDates.First(), calendar.SelectedDates.Last() + new TimeSpan(1, 0, 0, 0));
            //if (calendar.SelectedDates.First() <= calendar.SelectedDates.Last())
            //{
                
            //    hvm.StartDate = ;
            //    hvm.StopDate = ;
            //}
            //else
            //{
            //    hvm.StopDate = calendar.SelectedDates.First();
            //    hvm.StartDate = calendar.SelectedDates.Last() + new TimeSpan(1, 0, 0, 0);
            //}
            _hvm.VisSelectDate = Visibility.Collapsed;
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

        private void rbtnGraphic_Checked(object sender, RoutedEventArgs e)
        {
            if (grdGrapic != null && grdList != null)
            {
                grdGrapic.Visibility = Visibility.Visible;
                grdList.Visibility = Visibility.Collapsed;
            }
        }

        private void rbtnList_Checked(object sender, RoutedEventArgs e)
        {
            if (grdGrapic != null && grdList != null)
            {
                grdGrapic.Visibility = Visibility.Collapsed;
                grdList.Visibility = Visibility.Visible;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            calendar1.DisplayDateEnd = dt;
            if (_hvm != null) _hvm.SetDates(dt, dt + new TimeSpan(1, 0, 0, 0));
        }

        bool _statDisplayed = true;

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            brdrStat.Visibility = _statDisplayed ? Visibility.Collapsed : Visibility.Visible;
            _statDisplayed = !_statDisplayed;
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            var tb = sender as ToggleButton;
            if (tb == null) return;
            if (tb.IsChecked == true)
            {
                grdService.Width = 450;
                grdGrapic.Visibility = Visibility.Visible;
                _hvm.SpanMap = 2;
                tb.Content = "Скрыть детали";
            }
            else
            {
                grdService.Width = 230;
                grdGrapic.Visibility = Visibility.Collapsed;
                _hvm.SpanMap = 3;
                tb.Content = "Детально";
            }

        }

        bool _displayHistory=false;

        public void HistoryButton_Click()
        {
            if (_displayHistory == false)
            {
                //grdService.Visibility = Visibility.Visible;
                CarPoints.Opacity = .5;
                _hvm.EnableHistory = true;
                _hvm.LoadData();
                grdHistoryWork.Visibility = Visibility.Visible;
            }
            else
            {
                //grdService.Visibility = Visibility.Collapsed;
                CarPoints.Opacity = 1;
                _hvm.EnableHistory = false;
                grdHistoryWork.Visibility = Visibility.Collapsed;
            }
            _displayHistory = !_displayHistory;
            CarPin.Visibility = ParkingsPin.Visibility =
            RouteLine.Visibility = WarningLine.Visibility = ErrorLine.Visibility = OfflineLine.Visibility =
            btnMinimize.Visibility = brdrStat.Visibility = _carZonesError.Visibility = grdHistoryWork.Visibility;
        }

        private void grdHistoryWork_MouseEnter(object sender, MouseEventArgs e)
        {
            grdHistoryWork.Opacity = 1;
        }

        private void grdHistoryWork_MouseLeave(object sender, MouseEventArgs e)
        {
            grdHistoryWork.Opacity = 0.4f;
        }
    }
}
