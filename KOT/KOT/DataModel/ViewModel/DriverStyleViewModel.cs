using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using KOT.Annotations;
using KOT.DataModel.Handlers;

namespace KOT.DataModel.ViewModel
{
    public class DriverStyleViewModel : INotifyPropertyChanged
    {
        public DriverStyleViewModel()
        {
            Update();
        }

        public async void Update()
        {
            await PCHandler.GetDriverStile();
            OnPropertyChanged("CurrentDrivingStyle");
            OnPropertyChanged("CurrentEcoStyle");
            OnPropertyChanged("TotalDrivingScore");
            OnPropertyChanged("TodayDrivingScore");
            OnPropertyChanged("TotalEcoScore");
            OnPropertyChanged("TodayEcoScore");
        }
        //Текущий стиль вождения, от 0 до 100% процентов
        public double CurrentDrivingStyle { get { return (double)PCHandler.DriverStyle.CurrentDrivingStyle / 100.0; } }
        //Текущая экологичность вождения, от 0 до 100%
        public double CurrentEcoStyle { get { return (double)PCHandler.DriverStyle.CurrentEcoStyle / 100.0; } }

        //Общее значение очков вождения
        public int TotalDrivingScore { get { return PCHandler.DriverStyle.TotalDrivingScore; } }
        //Сегодняшнее значение очков вождения
        public int TodayDrivingScore { get { return PCHandler.DriverStyle.TodayDrivingScore; } }

        //общий показатель экологичности
        public int TotalEcoScore { get { return PCHandler.DriverStyle.TotalEcoScore; } }
        //Дневной показатель экологичности
        public int TodayEcoScore { get { return PCHandler.DriverStyle.TodayEcoScore; } }

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
