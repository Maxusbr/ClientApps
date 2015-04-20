using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCDev.Client.ViewModel;

namespace DTCDev.Client.Cars.Controls.ViewModels.Reports
{
    class ReportsViewModel : ViewModelBase
    {
        public ReportsViewModel()
        {

        }

        private bool _oneWindow = true;
        public bool OneWindow
        {
            get { return _oneWindow; }
            set
            {
                _oneWindow = value;
                this.OnPropertyChanged("OneWindow");
            }
        }
    }
}
