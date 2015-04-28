using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using DTCDev.Client.Cars.Service.Engine.Handlers;
using DTCDev.Client.ViewModel;
using DTCDev.Models.CarBase.CarList;
using DTCDev.Client.Cars.Service.Engine.Storage;

namespace DTCDev.Client.Cars.Service.Engine.Controls.ViewModels
{
    public class ShowHistoryViewModel : ViewModelBase, ICancelHandler
    {
        private readonly ObservableCollection<ServiceViewModel> _services = new ObservableCollection<ServiceViewModel>();
        private RelayCommand _cancelCommand;
        private CarListDetailsDataModel _model;

        public ShowHistoryViewModel()
        {
            _model = CarStorage.Instance.SelectedCarDetails;
            CarsHandler.Instance.HistoryWorksLoaded += Instance_HistoryWorksLoaded;
            CarsHandler.Instance.GetCarHistory(_model.CarNumber);
        }

        void Instance_HistoryWorksLoaded(List<CarHistoryWorkReport> works)
        {
            Services.Clear();
            foreach (var item in works)
            {
                Services.Add(new ServiceViewModel
                {
                    Comment = item.Comment,
                    CurentDate = item.Date.D.ToString() + "." + item.Date.M.ToString() + "." + item.Date.Y.ToString(),
                    Name = item.WorkName,
                    DistanceToMake = item.Distance,
                    Price = item.Cost,
                    OrderN = item.OrderN
                });
            }
        }

        public event EventHandler CancelHandler;
        protected virtual void OnCancelHandler()
        {
            if (CancelHandler != null) CancelHandler(this, EventArgs.Empty);
        }

        public ObservableCollection<ServiceViewModel> Services
        {
            get { return _services; }
        }

        public RelayCommand CancelCommand
        {
            get { return _cancelCommand ?? (_cancelCommand = new RelayCommand(Cancel)); }
        }

        private void Cancel(object obj)
        {
            OnCancelHandler();
        }
    }
}
