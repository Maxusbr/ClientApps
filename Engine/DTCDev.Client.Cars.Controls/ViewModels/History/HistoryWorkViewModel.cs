using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using DTCDev.Client.ViewModel;

namespace DTCDev.Client.Cars.Controls.ViewModels.History
{
    public class HistoryWorkViewModel : ViewModelBase
    {
        private readonly System.Windows.Threading.Dispatcher _dispatcher;

        public HistoryWorkViewModel(System.Windows.Threading.Dispatcher dispatcher)
        {
            // TODO: Complete member initialization
            _dispatcher = dispatcher;
        }

        protected virtual void OnPropertyChanging(string propertyName)
        {
            if (_dispatcher != null)
                _dispatcher.BeginInvoke(new Action(() =>
                {
                    OnPropertyChanged(propertyName);
                }));
        }
    }
}
