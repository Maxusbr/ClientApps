using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KOT.DataModel.Model;
using KOT.DataModel.ViewModel;

namespace KOT.DataModel.Handlers
{
    public class WorksDataHandler
    {
        private static WorksDataHandler _instance;
        public static WorksDataHandler Instance
        {
            get { return _instance ?? (_instance = new WorksDataHandler()); }
        }

        public WorksDataHandler()
        {
            _instance = this;
            
        }

        private readonly ObservableCollection<WorkTypeViewModel> _recomendetWorkTypes = new ObservableCollection<WorkTypeViewModel>();
        public static ObservableCollection<WorkTypeViewModel> RecomendetWorkTypes
        {
            get { return Instance._recomendetWorkTypes; }

        }

        public async static Task UpdateWork()
        {
            await Instance.UpdateWorkAsync();
        }

        private async Task UpdateWorkAsync()
        {
            //if (DesignMode.DesignModeEnabled)
            {
                RecomendetWorkTypes.Add(new WorkTypeViewModel(
                    new WorkType
                    {
                        DaysToMake = 35,
                        ID = "0",
                        DistanceToMake = 200,
                        Name = "Чистка форсунок",
                        LastMakeDist = 250,
                        LastMakeTime = new DateDataModel(
                            DateTime.Now.AddDays(-60)),
                        PeriodicDist = 100,
                        PeriodicTime = 5,
                        PresentModel = 1
                    }));
                RecomendetWorkTypes.Add(new WorkTypeViewModel(
                    new WorkType
                    {
                        DaysToMake = 34,
                        ID = "0",
                        DistanceToMake = 200,
                        Name = "Замена масла",
                        LastMakeDist = 250,
                        LastMakeTime = new DateDataModel(
                            DateTime.Now.AddDays(-60)),
                        PeriodicDist = 100,
                        PeriodicTime = 5,
                        PresentModel = 1
                    }));
                RecomendetWorkTypes.Add(new WorkTypeViewModel(
                    new WorkType
                    {
                        DaysToMake = 38,
                        ID = "0",
                        DistanceToMake = 200,
                        Name = "Замена фильтров",
                        LastMakeDist = 250,
                        LastMakeTime = new DateDataModel(
                            DateTime.Now.AddDays(-65)),
                        PeriodicDist = 100,
                        PeriodicTime = 5,
                        PresentModel = 1
                    }));
                RecomendetWorkTypes.Add(new WorkTypeViewModel(
                    new WorkType
                    {
                        DaysToMake = -3,
                        ID = "1",
                        DistanceToMake = -100,
                        Name = "Замена фильтров",
                        LastMakeDist = 250,
                        LastMakeTime = new DateDataModel(
                            DateTime.Now.AddDays(-45)),
                        PeriodicDist = 50,
                        PeriodicTime = 4,
                        PresentModel = 2
                    }));
                RecomendetWorkTypes.Add(new WorkTypeViewModel(
                    new WorkType
                    {
                        DaysToMake = -4,
                        ID = "1",
                        DistanceToMake = -100,
                        Name = "Замена масла",
                        LastMakeDist = 250,
                        LastMakeTime = new DateDataModel(
                            DateTime.Now.AddDays(-42)),
                        PeriodicDist = 50,
                        PeriodicTime = 4,
                        PresentModel = 2
                    }));
                RecomendetWorkTypes.Add(new WorkTypeViewModel(
                    new WorkType
                    {
                        DaysToMake = 3,
                        ID = "1",
                        DistanceToMake = -100,
                        Name = "Чистка форсунок",
                        LastMakeDist = 250,
                        LastMakeTime = new DateDataModel(
                            DateTime.Now.AddDays(-43)),
                        PeriodicDist = 50,
                        PeriodicTime = 4,
                        PresentModel = 2
                    }));
            }
        }
    }
}
