using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KOT.DataModel.ViewModel
{
    public class MainMenuViewModel
    {
        private static MainMenuViewModel _instance;
        public static MainMenuViewModel Instance
        {
            get { return _instance ?? (_instance = new MainMenuViewModel()); }
        }
        public MainMenuViewModel()
        {
            _instance = this;
        }

        public static bool IsMapCheck { get; set; }
        public static bool IsToCheck { get; set; }
        public static bool IsPcCheck { get; set; }
        public static bool IsBudgetCheck { get; set; }
        public static bool IsAlarmCheck { get; set; }
        public static bool IsSettingsCheck { get; set; }
        public static bool IsAboutCheck { get; set; }
    }
}
