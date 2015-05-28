using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using KOT.Annotations;
using KOT.DataModel.Handlers;
using KOT.DataModel.Model;

namespace KOT.DataModel.ViewModel
{
    public class BudgetViewModel: INotifyPropertyChanged
    {
        public BudgetViewModel()
        {
            Init();
        }
        private DateTime _current = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

        private async void Init()
        {
            await Update(1);
        }

        public async Task Update(int count)
        {
            if (BudgetHandler.Model == null)
                await BudgetHandler.UpdateSource();
            OnPropertyChanged("TotalCost");
            PivotItems.Clear();
            for (var i = count-1; i >= 0; i--)
            {
                PivotItems.Add(new BudgetItemViewModel(_current.AddMonths(-i), await GetModel(-i)));
            }
            SelectedPivot =PivotItems[PivotItems.Count - 1];
        }

        private async Task<SpendingModel> GetModel(int indx)
        {
            var res = new SpendingModel();
            var st = _current.AddMonths(indx); 
            var end = _current.AddMonths(indx+1);
            foreach (var el in BudgetHandler.Model.Spends.Where(o => o.Date.ToDate >= st && o.Date.ToDate < end))
            {
                res.Spends.Add(el);
            }
            res.TotalCost = res.Spends.Sum(o => o.Sum);
            return res;
        }

        public string TotalCost { get { return string.Format("{0} руб.", BudgetHandler.Model.TotalCost) ; } }
        private readonly ObservableCollection<BudgetItemViewModel> _pivotItems = new ObservableCollection<BudgetItemViewModel>();
        private BudgetItemViewModel _selectedPivot;
        private bool _visableDetails;
        public ObservableCollection<BudgetItemViewModel> PivotItems { get { return _pivotItems; } }

        public BudgetItemViewModel SelectedPivot
        {
            get { return _selectedPivot; }
            set
            {
                if (Equals(value, _selectedPivot)) return;
                _selectedPivot = value;
                OnPropertyChanged("SelectedPivot");
            }
        }

        public bool VisableDetails
        {
            get { return _visableDetails; }
            set
            {
                if (value.Equals(_visableDetails)) return;
                _visableDetails = value;
                OnPropertyChanged("VisableDetails");
            }
        }

        internal void UpdateSelected(int id)
        {
            SelectedPivot.UpdateList(id);
            VisableDetails = true;
        }

        internal void CloseDetail()
        {
            VisableDetails = false;
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
