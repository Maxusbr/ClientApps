using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.UI.Xaml.Data;
using KOT.Annotations;
using KOT.DataModel.Handlers;
using KOT.DataModel.Model;

namespace KOT.DataModel.ViewModel
{
    public class MainMenuViewModel : INotifyPropertyChanged
    {
        private static MainMenuViewModel _instance;
        private string _nameDrive = string.Empty;
        private string _markModel = string.Empty;
        private CarListBaseDataModel _selectedCar;

        public static MainMenuViewModel Instance
        {
            get { return _instance ?? (_instance = new MainMenuViewModel()); }
        }
        public MainMenuViewModel()
        {
            _instance = this;
            if (!DesignMode.DesignModeEnabled)
                UpdateSource();
        }

        private async void UpdateSource()
        {
            if (CarsHandler.Cars.Count == 0) await CarsHandler.Update();
            //if (SelectedCar == null)
            //    SelectedCar = CarsHandler.Cars.Count == 0 ? new CarListBaseDataModel() : CarsHandler.Cars[0];
        }

        public static bool IsMapCheck { get; set; }
        public static bool IsToCheck { get; set; }
        public static bool IsPcCheck { get; set; }
        public static bool IsBudgetCheck { get; set; }
        public static bool IsAlarmCheck { get; set; }
        public static bool IsSettingsCheck { get; set; }
        public static bool IsAboutCheck { get; set; }

        public ObservableCollection<CarListBaseDataModel> Cars { get { return CarsHandler.Cars; } }

        public CarListBaseDataModel SelectedCar
        {
            get
            {
                return CarsHandler.SelectedCar;
            }
            set
            {
                if (Equals(value, CarsHandler.SelectedCar)) return;
                CarsHandler.SelectedCar = value;
                OnPropertyChanged("SelectedCar");
                OnPropertyChanged("MarkModel");
                OnPropertyChanged("NameDrive");
            }
        }

        public string NameDrive
        {
            get { return SelectedCar.CarNumber; }
        }

        public string MarkModel
        {
            get { return string.Format("{0} {1}", SelectedCar.Mark, SelectedCar.Model); }
        }

        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

    }
}
