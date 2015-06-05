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
    public sealed partial class AlarmControl : UserControl
    {
        private readonly string _propertyName = string.Empty;
        public AlarmControl()
        {
            this.InitializeComponent();
        }

        public AlarmControl(string propertyName)
        {
            this.InitializeComponent();
            _propertyName = propertyName;
        }

        public event EventHandler Close;
        private void OnClose()
        {
            if (Close != null) Close(this, null);
        }

        public string PropertyName { get { return _propertyName; } }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as AlarmViewModel;
            if (vm != null)
                vm.Update(_propertyName);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OnClose();
        }
    }
}
