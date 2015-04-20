using DTCDev.Client.Cars.Engine.DisplayModels;
using DTCDev.Client.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Client.Cars.Controls.ViewModels.Car
{
    class CarToolTipViewModel :ViewModelBase
    {
        public CarToolTipViewModel()
        {
        }

        private DISP_Car _car;

        public CarToolTipViewModel(DISP_Car car)
        {
            _car = car;
        }

        private void OnUpdate()
        {
        }
    }
}
