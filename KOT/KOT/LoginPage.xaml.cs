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

// Документацию по шаблону элемента пустой страницы см. по адресу http://go.microsoft.com/fwlink/?LinkID=390556
using KOT.Common;
using KOT.DataModel.Handlers;
using KOT.DataModel.Network;
using KOT.DataModel.ViewModel;

namespace KOT
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        private readonly NavigationHelper navigationHelper;
        private readonly ObservableDictionary _defaultViewModel = new ObservableDictionary();

        public LoginPage()
        {
            this.InitializeComponent();
            navigationHelper = new NavigationHelper(this);
        }

        public ObservableDictionary DefaultViewModel
        {
            get { return _defaultViewModel; }
        }
        /// <summary>
        /// Вызывается перед отображением этой страницы во фрейме.
        /// </summary>
        /// <param name="e">Данные события, описывающие, каким образом была достигнута эта страница.
        /// Этот параметр обычно используется для настройки страницы.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await TcpConnection.Instance.Connect();
        }

        private async void btLogin_Click(object sender, RoutedEventArgs e)
        {
            if (await LoginHandler.Login(tbName.Text, tbPass.Text))
                Navigate();
        }

        private async void btDemo_Click(object sender, RoutedEventArgs e)
        {
            if (await LoginHandler.Login("test", "test123"))
                Navigate();
        }

        private void Navigate()
        {
            MainMenuViewModel.IsPcCheck = true;
            Frame.Navigate(typeof(EVMPage));
        }

    }
}
