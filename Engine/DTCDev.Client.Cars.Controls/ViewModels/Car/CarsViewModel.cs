using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using DTCDev.Client.Cars.Engine.AppLogic;
using DTCDev.Client.Cars.Engine.DisplayModels;
using DTCDev.Client.Cars.Engine.Handlers.Cars;
using DTCDev.Client.ViewModel;

namespace DTCDev.Client.Cars.Controls.ViewModels.Car
{
    class CarsViewModel:ViewModelBase
    {
        public CarsViewModel()
        {
            if (CarsList == null)
                CarsList = new ObservableCollection<DISP_Car>();
            FillCarsList();
            CarsHandler.Instance.CarsRefreshed -= Instance_CarsRefreshed;
            CarsHandler.Instance.CarsRefreshed += Instance_CarsRefreshed;
            CarSelector.OnCarChanged -= CarSelector_OnCarChanged;
            CarSelector.OnCarChanged += CarSelector_OnCarChanged;
        }

        void CarSelector_OnCarChanged(DISP_Car car)
        {
            if (SelectedCar != CarSelector.SelectedCar)
            {
                SelectedCar = CarSelector.SelectedCar;
            }
        }


        public ObservableCollection<DISP_Car> CarsList { get; set; }

        private DISP_Car _selectedCar;
        public DISP_Car SelectedCar
        {
            get { return _selectedCar; }
            set
            {
                _selectedCar = value;
                this.OnPropertyChanged("SelectedCar");
                if (CarSelector.SelectedCar != value)
                    CarSelector.SelectedCar = value;
            }
        }




        void Instance_CarsRefreshed(object sender, EventArgs e)
        {
            FillCarsList();
        }



        private void FillCarsList()
        {
            CarsList.Clear();
            foreach (var item in CarsHandler.Instance.Cars)
            {
                CarsList.Add(item);
            }
        }
    }
}
