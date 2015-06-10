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
        private CarViewModel _selectedCar;
        private readonly ObservableCollection<CarViewModel> _cars = new ObservableCollection<CarViewModel>();
        public static MainMenuViewModel Instance
        {
            get { return _instance ?? (_instance = new MainMenuViewModel()); }
        }
        public MainMenuViewModel()
        {
            _instance = this;
            //if (!DesignMode.DesignModeEnabled)
                UpdateSource();
        }

        private async void UpdateSource()
        {
            if (CarsHandler.Cars.Count == 0) await CarsHandler.Update();
            //if (SelectedCar == null)
            //    SelectedCar = CarsHandler.Cars.Count == 0 ? new CarListBaseDataModel() : CarsHandler.Cars[0];
            Cars.Clear();
            foreach (var el in CarsHandler.Cars)
                Cars.Add(new CarViewModel(el));
            _selectedCar = Cars.FirstOrDefault(o => o.DID.Equals(CarsHandler.SelectedCar.DID));
        }

        public static bool IsMapCheck { get; set; }
        public static bool IsToCheck { get; set; }
        public static bool IsPcCheck { get; set; }
        public static bool IsBudgetCheck { get; set; }
        public static bool IsAlarmCheck { get; set; }
        public static bool IsSettingsCheck { get; set; }
        public static bool IsAboutCheck { get; set; }

        public ObservableCollection<CarViewModel> Cars { get { return _cars; } }

        public CarViewModel SelectedCar
        {
            get
            {
                return _selectedCar;
            }
            set
            {
                if (Equals(value, _selectedCar)) return;
                _selectedCar = value;
                CarsHandler.SelectedCar = CarsHandler.Cars.FirstOrDefault(o => o.DID.Equals(value.DID));
                OnPropertyChanged();
            }
        }

        public string DateEndService
        {
            get { return SettingsHandler.Instance.DateEndService.ToString("d"); }
        }

        public string Phone
        {
            get { return SettingsHandler.Instance.Phone; }
        }

        public string Mail
        {
            get { return SettingsHandler.Instance.Mail; }
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

    public class CarViewModel
    {
        private readonly CarListBaseDataModel _model;
        public CarViewModel() { }
        public CarViewModel(CarListBaseDataModel model)
        {
            _model = model;
        }
        public string DID { get { return _model.DID; } }
        public string NameDrive
        {
            get { return _model.CarNumber; }
        }

        public string MarkModel
        {
            get { return string.Format("{0} {1}", _model.Mark, _model.Model); }
        }
    }
}
