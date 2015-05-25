using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Шаблон элемента пользовательского элемента управления задокументирован по адресу http://go.microsoft.com/fwlink/?LinkId=234236
using KOT.DataModel.ViewModel;

namespace KOT.Common.Controls
{
    public sealed partial class RecomendationCalendarControl : UserControl
    {
        private WorksViewModel _vm;
        private static bool _isScrolling = false;
        private static bool _isTaped = false;
        public RecomendationCalendarControl()
        {
            this.InitializeComponent();

        }

        private void AppBarButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout(sender as FrameworkElement);
        }

        private void ListBox_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            var fl = FlyoutBase.GetAttachedFlyout(YearButton);
            if (fl == null) return;
            fl.Hide();
        }

        private void ListView_Loaded(object sender, RoutedEventArgs e)
        {
            _vm = DataContext as WorksViewModel;
        }

        private void ListView_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var lw = sender as ListView;
            if (_vm == null || lw == null) return;
            FlyoutBase.ShowAttachedFlyout(this);
            UpdateLayout();
            _vm.SelectedMonth = lw.SelectedItem as WorksInMonthViewModel;
            Month.ScrollIntoView(_vm.SelectedMonth);

        }

        private void Event_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            var scroll = sender as ScrollViewer;
            if (_vm == null || scroll == null || !e.IsIntermediate) return;
            var wd = (int)Math.Round(scroll.HorizontalOffset / scroll.ViewportWidth);
            if (wd >= 0 && wd < _vm.YearWorks.Count)
            {
                _vm.SelectedMonth = _vm.YearWorks[wd];
            }
        }

        public static ScrollViewer GetScrollViewer(DependencyObject depObj)
        {
            if (depObj is ScrollViewer) return depObj as ScrollViewer;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);

                var result = GetScrollViewer(child);
                if (result != null) return result;
            }
            return null;
        }

        private void Month_Loaded(object sender, RoutedEventArgs e)
        {
            GetScrollViewer(Month).ViewChanged += Event_ViewChanged;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var fl = FlyoutBase.GetAttachedFlyout(this);
            if (fl == null) return;
            fl.Hide();
        }

        private void Days_ItemClick(object sender, ItemClickEventArgs e)
        {
            var el = e.ClickedItem as WorksInMonthViewModel;
            if (_vm == null || el == null ) return;
            _vm.SelectedMotnthWorkUpdate(el.Date);
        }
    }
}
