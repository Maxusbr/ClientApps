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
using KOT.DataModel.Handlers;

namespace KOT.DataModel.ViewModel
{
    public class HistoryAlarmsViewModel: INotifyPropertyChanged
    {
        private AlarmViewModel _selectedAlarm;

        public HistoryAlarmsViewModel()
        {
            Init();
            if (DesignMode.DesignModeEnabled)
            {
                HistoryAlarmList.Add(new AlarmViewModel("AlarmLevel", DateTime.Now));
                HistoryAlarmList.Add(new AlarmViewModel("IsDoorClosed", DateTime.Now));
            }
        }

        private async void Init()
        {
            await AlarmHandler.UpdateSource();
        }

        public ObservableCollection<AlarmViewModel> HistoryAlarmList { get { return AlarmHandler.Instance.HistoryAlarmList; } }

        public AlarmViewModel SelectedAlarm
        {
            get { return _selectedAlarm; }
            set
            {
                UpdateVisableDetails(_selectedAlarm, false);
                if (Equals(value, _selectedAlarm)) return;
                _selectedAlarm = value;
                UpdateVisableDetails(_selectedAlarm, true);
                OnPropertyChanged();
            }
        }

        

        private void UpdateVisableDetails(AlarmViewModel alarm, bool visable)
        {
            if(alarm == null) return;
            alarm.ViewDetails = visable;
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
