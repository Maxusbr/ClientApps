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
    public sealed partial class RecomendationListControl : UserControl
    {
        private WorksViewModel _vm;
        private WorkTypeViewModel _selectedVM;
        public RecomendationListControl()
        {
            this.InitializeComponent();
        }

        private void WorkControl_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var cntrl = sender as FrameworkElement;
            if(cntrl == null) return;
            _selectedVM = cntrl.DataContext as WorkTypeViewModel;
            if (_selectedVM == null) return;
            _selectedVM.ShowDetail();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _vm = DataContext as WorksViewModel;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            var cntrl = sender as Button;
            if (cntrl == null) return;
            var vm = cntrl.DataContext as WorkTypeViewModel;
            if (vm == null) return;
            vm.HideDetail(false);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var cntrl = sender as Button;
            if (cntrl == null) return;
            var vm = cntrl.DataContext as WorkTypeViewModel;
            if (vm == null) return;
            vm.HideDetail(true);
        }


        private void DateSelect_OnClose(object sender)
        {
            var dt = sender as DateWeekSelectControl;
            if (dt == null || _selectedVM == null) return;
            _selectedVM.Date = dt.Date.ToString("d");
        }
    }
}
