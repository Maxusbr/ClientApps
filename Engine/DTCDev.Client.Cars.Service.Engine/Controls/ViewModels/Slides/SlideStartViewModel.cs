using DTCDev.Client.Cars.Service.Engine.Handlers;
using DTCDev.Client.Cars.Service.Engine.Storage;
using DTCDev.Client.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace DTCDev.Client.Cars.Service.Engine.Controls.ViewModels.Slides
{
    public class SlideStartViewModel : ViewModelBase
    {

        public ObservableCollection<ChartDataModel> Items { get; private set; }

        public SlideStartViewModel()
        {
            Items = new ObservableCollection<ChartDataModel>();
            CarStorage.Instance.LoadComplete += Instance_LoadComplete;
        }

        void Instance_LoadComplete(object sender, EventArgs e)
        {
            Items.Clear();
            int cComing = CarStorage.Instance.Cars.Where(p => p.CarModel.DaysToService <= 7).Count();
            int cNotSoon = CarStorage.Instance.Cars.Where(p => p.CarModel.DaysToService > 7).Count();
            int total = cComing + cNotSoon;
            if (total < 1)
                total = 1;
            int percentsComing = (cComing * 100) / total;
            Items.Add(new ChartDataModel { Count = percentsComing, Name = "На этой неделе: "+cComing.ToString()});
            Items.Add(new ChartDataModel { Count = 100-percentsComing, Name = "Позже: " + cNotSoon.ToString()});
        }

        public class ChartDataModel
        {
            public string Name { get; set; }

            public int Count { get; set; }
        }
    }
}
