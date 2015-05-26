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
    public class HistoryWorksViewModel : INotifyPropertyChanged
    {
        public HistoryWorksViewModel()
        {
            HistoryWorks.CollectionChanged += HistoryWorks_CollectionChanged;
            UpdateHistory();
        }

        private async void UpdateHistory()
        {
            await HistoryWorkHandler.UpdateHistory();
        }

        private void HistoryWorks_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //OnPropertyChanged("DateRange");
            OnPropertyChanged("CostSumm");
        }

        public static ObservableCollection<WorkFromHistoryViewModel> HistoryWorks
        {
            get { return HistoryWorkHandler.HistorySortedWorks; }

        }

        public string DateRange
        {
            get { return string.Format("{0} - {1}", HistoryWorks.Min(o => o.Date).ToString("d"), HistoryWorks.Max(o => o.Date).ToString("d")); }
        }

        public string CostSumm
        {
            get { return string.Format("{0} руб.", HistoryWorks.Sum(o => o.Cost)); }
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
