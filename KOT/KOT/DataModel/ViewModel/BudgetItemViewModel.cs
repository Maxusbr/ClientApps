using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using KOT.Annotations;
using KOT.DataModel.Model;

namespace KOT.DataModel.ViewModel
{
    public class BudgetItemViewModel : INotifyPropertyChanged
    {
        private readonly SpendingViewModel _viewModel;
        private readonly SpendingModel _model;
        private readonly ObservableCollection<OneSpendItem> _listItems = new ObservableCollection<OneSpendItem>();

        public BudgetItemViewModel()
        {
            if (DesignMode.DesignModeEnabled)
            {
                ListItem.Add(new OneSpendItem{Date = new DateDataModel(DateTime.Now), Name = "Comment 1", Sum = 4687, idClass = 0});
            }
        }

        public BudgetItemViewModel(DateTime date, SpendingModel model)
        {
            Date = date;
            _model = model;
            _viewModel = new SpendingViewModel(model);
        }

        public string Header { get { return Date.ToString(@"MMMM `yy"); } }

        public SpendingViewModel ViewModel
        {
            get { return _viewModel; }
        }

        public DateTime Date { get; set; }

        public override string ToString()
        {
            return Header;
        }

        public ObservableCollection<OneSpendItem> ListItem { get { return _listItems; } }

        public SpendingModel Model { get { return _model; } }

        public int SelectedCategoryId { get; set; }

        public void UpdateList(int id)
        {
            SelectedCategoryId = id;
            ListItem.Clear();
            foreach (var el in Model.Spends.Where(o => o.idClass == id))
            {
                ListItem.Add(el);
            }
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
