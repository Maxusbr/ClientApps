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
            //if (DesignMode.DesignModeEnabled)
            {
                RecomendetWorkTypes.Add(new WorkTypeViewModel(
                    new WorkType
                    {
                        DaysToMake = 2,
                        ID = "0",
                        DistanceToMake = 200,
                        Name = "Work 1",
                        LastMakeDist = 250,
                        LastMakeTime = new DateDataModel(
                            DateTime.Now.AddDays(-5)),
                        PeriodicDist = 100,
                        PeriodicTime = 5,
                        PresentModel = 1
                    }));
                RecomendetWorkTypes.Add(new WorkTypeViewModel(
                    new WorkType
                    {
                        DaysToMake = -1,
                        ID = "1",
                        DistanceToMake = -100,
                        Name = "Work 2",
                        LastMakeDist = 250,
                        LastMakeTime = new DateDataModel(
                            DateTime.Now.AddDays(-15)),
                        PeriodicDist = 50,
                        PeriodicTime = 4,
                        PresentModel = 2
                    }));
            }
        }

        private readonly ObservableCollection<WorkTypeViewModel> _recomendetWorkTypes = new ObservableCollection<WorkTypeViewModel>();
        public static ObservableCollection<WorkTypeViewModel> RecomendetWorkTypes
        {
            get { return Instance._recomendetWorkTypes; }

        }


    }
}
