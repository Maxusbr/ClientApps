using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using KOT.DataModel.Handlers;
using KOT.DataModel.Model;

namespace KOT.DataModel.ViewModel
{
    public class WorksViewModel
    {
        public WorksViewModel()
        {
            
        }

        public ObservableCollection<WorkTypeViewModel> WorkTypes { get { return WorksDataHandler.RecomendetWorkTypes; } }

        public string DateTxt { get { return DateTime.Now.ToString("d"); } }
    }


}
