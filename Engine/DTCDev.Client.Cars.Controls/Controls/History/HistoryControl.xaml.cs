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
        
        HistoryViewModel hvm;

        public HistoryControl()
        {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this))
                return;
             hvm = this.DataContext as HistoryViewModel;
        }

        public HistoryControl(DateTime date)
        {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this))
                return;
            this.DataContext = hvm;
            hvm.StartDate = date;
            hvm.StopDate = date + new TimeSpan(1, 0, 0, 0);
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
            hvm.SetDates(calendar.SelectedDates.First(), calendar.SelectedDates.Last() + new TimeSpan(1, 0, 0, 0));
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
            hvm.VisSelectDate = Visibility.Collapsed;
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
            calendar1.DisplayDateEnd = DateTime.Now;
        }

        bool _statDisplayed = true;

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            if (_statDisplayed)
                brdrStat.Visibility = Visibility.Collapsed;
            else
                brdrStat.Visibility = Visibility.Visible;
            _statDisplayed = !_statDisplayed;
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton tb = sender as ToggleButton;
            if (tb.IsChecked == true)
            {
                grdService.Width = 450;
                //Storyboard sb = this.FindResource("stbShowDetails") as Storyboard;
                //sb.Begin();
            }
            else
            {
                grdService.Width = 0;
                //Storyboard sb = this.FindResource("stbHideDetails") as Storyboard;
                //sb.Begin();
            }
            
        }
    }
}
