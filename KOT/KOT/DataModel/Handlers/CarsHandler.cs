using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using KOT.DataModel.Model;
using KOT.DataModel.Network;
using Newtonsoft.Json;

namespace KOT.DataModel.Handlers
{
    public class CarsHandler
    {
        private static CarsHandler _instance;
        private CarListBaseDataModel _selectedCar;

        public static CarsHandler Instance
        {
            get { return _instance ?? (_instance = new CarsHandler()); }
        }

        public CarsHandler()
        {
            _instance = this;
            if (DesignMode.DesignModeEnabled)
            {
                Cars.Add(new CarListBaseDataModel{CarNumber = "Demo 1"});
                Cars.Add(new CarListBaseDataModel { CarNumber = "Demo 2" });
                Cars.Add(new CarListBaseDataModel { CarNumber = "Demo 3" });
            }
        }

        public static event EventHandler SelectionChanged;
        protected virtual void OnSelectionChanged()
        {
            EventHandler handler = SelectionChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        private readonly ObservableCollection<CarListBaseDataModel> _cars = new ObservableCollection<CarListBaseDataModel>();

        public static ObservableCollection<CarListBaseDataModel> Cars { get { return Instance._cars; } }

        public static CarListBaseDataModel SelectedCar
        {
            get { return Instance._selectedCar; }
            set
            {
                Instance._selectedCar = value;
                Instance.OnSelectionChanged();
            }
        }

        public async static Task Update()
        {
            await Instance.UpdateAsync();
        }

        private async Task UpdateAsync()
        {
            var res = await TcpConnection.Send("BA");
            if (res.Px != 'B' || res.Fx != 'A') return;
            Cars.Clear();
            try
            {
                foreach (var el in JsonConvert.DeserializeObject<List<CarListBaseDataModel>>(res.Msg))
                {
                    Cars.Add(el);
                }
            }
            catch (Exception e)
            {
                
            }
            
        }
    }
}
