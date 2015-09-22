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
        private System.Windows.Threading.Dispatcher _dispatcher;

        public CarsViewModel(System.Windows.Threading.Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
            if (CarsList == null)
                CarsList = new ObservableCollection<DISP_Car>();
            //FillCarsList();
            CarsHandler.Instance.CarsRefreshed -= Instance_CarsRefreshed;
            CarsHandler.Instance.CarsRefreshed += Instance_CarsRefreshed;
            CarSelector.OnCarChanged -= CarSelector_OnCarChanged;
            CarSelector.OnCarChanged += CarSelector_OnCarChanged;
            CarSelector.OnCarSelected -= CarSelector_OnCarSelected;
            CarSelector.OnCarSelected += CarSelector_OnCarSelected;
            CarSelector.ClearSelection -= CarSelector_ClearSelection;
            CarSelector.ClearSelection += CarSelector_ClearSelection;
        }

        private void CarSelector_ClearSelection(object sender, EventArgs e)
        {
            foreach (var el in CarsList)
                el.Selected = false;
        }

        private void CarSelector_OnCarSelected(IEnumerable<string> cars)
        {
            foreach (var el in CarsList)
                el.Selected = cars.FirstOrDefault(o => el.ID.Equals(o)) != null;
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




        void Instance_CarsRefreshed(IEnumerable<DISP_Car> data )
        {
            FillCarsList(data);
        }



        private void FillCarsList(IEnumerable<DISP_Car> data )
        {
            CarsList.Clear();
            foreach (var item in data)
            {
                CarsList.Add(item);
            }
        }
    }
}
