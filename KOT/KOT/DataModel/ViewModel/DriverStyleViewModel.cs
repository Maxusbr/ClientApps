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
        private double _currentDrivingStyle;
        private double _currentEcoStyle;
        private int _totalDrivingScore;
        private int _todayDrivingScore;
        private int _totalEcoScore;
        private int _todayEcoScore;

        public DriverStyleViewModel()
        {
            CarsHandler.SelectionChanged += CarsHandler_SelectionChanged;
            Update();
        }

        private void CarsHandler_SelectionChanged(object sender, EventArgs e)
        {
            Update();
        }

        public async void Update()
        {
            await PCHandler.GetDriverStile();

            CurrentDrivingStyle = PCHandler.DriverStyle.CurrentDrivingStyle/100.0;
            CurrentEcoStyle = PCHandler.DriverStyle.CurrentEcoStyle / 100.0;
            TotalDrivingScore=PCHandler.DriverStyle.TotalDrivingScore;
            TodayDrivingScore = PCHandler.DriverStyle.TodayDrivingScore;
            TotalEcoScore = PCHandler.DriverStyle.TotalEcoScore;
            TodayEcoScore = PCHandler.DriverStyle.TodayEcoScore;
        }

        //Текущий стиль вождения, от 0 до 100% процентов
        public double CurrentDrivingStyle
        {
            get { return _currentDrivingStyle; }
            set
            {
                if (value.Equals(_currentDrivingStyle)) return;
                _currentDrivingStyle = value;
                OnPropertyChanged("CurrentDrivingStyle");
            }
        }

        //Текущая экологичность вождения, от 0 до 100%
        public double CurrentEcoStyle
        {
            get { return _currentEcoStyle; }
            set
            {
                if (value.Equals(_currentEcoStyle)) return;
                _currentEcoStyle = value;
                OnPropertyChanged("CurrentEcoStyle");
            }
        }

        //Общее значение очков вождения
        public int TotalDrivingScore
        {
            get { return _totalDrivingScore; }
            set
            {
                if (value == _totalDrivingScore) return;
                _totalDrivingScore = value;
                OnPropertyChanged("TotalDrivingScore");
            }
        }

        //Сегодняшнее значение очков вождения
        public int TodayDrivingScore
        {
            get { return _todayDrivingScore; }
            set
            {
                if (value == _todayDrivingScore) return;
                _todayDrivingScore = value;
                OnPropertyChanged("TodayDrivingScore");
            }
        }

        //общий показатель экологичности
        public int TotalEcoScore
        {
            get { return _totalEcoScore; }
            set
            {
                if (value == _totalEcoScore) return;
                _totalEcoScore = value;
                OnPropertyChanged("TotalEcoScore");
            }
        }

        //Дневной показатель экологичности
        public int TodayEcoScore
        {
            get { return _todayEcoScore; }
            set
            {
                if (value == _todayEcoScore) return;
                _todayEcoScore = value;
                OnPropertyChanged("TodayEcoScore");
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
