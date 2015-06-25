using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using DTCDev.Client.Cars.Service.Engine.Network;
using DTCDev.Models.CarBase.CarStatData;
using DTCDev.Models.CarBase.Shops;
using Newtonsoft.Json;

namespace DTCDev.Client.Cars.Service.Engine.Handlers
{
    public class SkladHandler
    {
        private static SkladHandler _instance;
        public static SkladHandler Instance
        {
            get { return _instance ?? (_instance = new SkladHandler()); }
        }

        public SkladHandler()
        {
            _instance = this;
            Init();
            LoadCache();
        }

        private void Init()
        {
            Units.Add(new KVPBase { id = 0, Name = "мл" });
            Units.Add(new KVPBase { id = 1, Name = "л" });
            Units.Add(new KVPBase { id = 2, Name = "г" });
            Units.Add(new KVPBase { id = 3, Name = "кг" });
            Units.Add(new KVPBase { id = 4, Name = "шт." });
            Uses.Add(new KVPBase{id = 0, Name = "Для всех"});
        }

        public void LoadCache()
        {
            var data = LoadStateSklad("statesklad");
            if (!string.IsNullOrEmpty(data))
                try
                {
                    var temp = JsonConvert.DeserializeObject<ICollection<ItemSkladDataModel>>(data);
                    SkladState.Clear();
                    foreach (var el in temp)
                        SkladState.Add(el);
                }
                catch { }
            data = LoadStateSklad("vendors");
            LoadListKvpBase(Vendors, data);
            data = LoadStateSklad("types");
            LoadListKvpBase(Types, data);
            if (Types.Count == 0)
            {
                Types.Add(new KVPBase { id = 0, Name = "запчасти" });
                Types.Add(new KVPBase { id = 1, Name = "доп. оборудование" });
            }
            data = LoadStateSklad("divisions");
            LoadListKvpBase(Divisions, data);
            OnLoadSkladStateComplete();
        }

        private void LoadListKvpBase(ObservableCollection<KVPBase> list, string data)
        {
            try
            {
                if (string.IsNullOrEmpty(data)) return;

                var temp = JsonConvert.DeserializeObject<ICollection<KVPBase>>(data);
                list.Clear();
                foreach (var el in temp)
                    list.Add(el);
            }
            catch { }
        }

        public event EventHandler LoadSkladStateComplete;
        protected virtual void OnLoadSkladStateComplete()
        {
            if (LoadSkladStateComplete != null) LoadSkladStateComplete(this, EventArgs.Empty);
        }

        public event EventHandler LoadVendorsComplete;
        protected virtual void OnLoadVendorsComplete()
        {
            if (LoadVendorsComplete != null) LoadVendorsComplete(this, EventArgs.Empty);
        }

        public event EventHandler LoadDivisionsComplete;
        protected virtual void OnLoadDivisionsComplete()
        {
            if (LoadDivisionsComplete != null) LoadDivisionsComplete(this, EventArgs.Empty);
        }

        readonly ObservableCollection<ItemSkladDataModel> _skladState = new ObservableCollection<ItemSkladDataModel>();
        public ObservableCollection<ItemSkladDataModel> SkladState { get { return _skladState; } }

        readonly ObservableCollection<KVPBase> _vendors = new ObservableCollection<KVPBase>();
        public ObservableCollection<KVPBase> Vendors { get { return _vendors; } }

        readonly ObservableCollection<KVPBase> _types = new ObservableCollection<KVPBase>();
        public ObservableCollection<KVPBase> Types { get { return _types; } }

        readonly ObservableCollection<KVPBase> _units = new ObservableCollection<KVPBase>();
        public ObservableCollection<KVPBase> Units { get { return _units; } }

        readonly ObservableCollection<KVPBase> _uses = new ObservableCollection<KVPBase>();
        public ObservableCollection<KVPBase> Uses { get { return _uses; } }

        readonly ObservableCollection<KVPBase> _divisions = new ObservableCollection<KVPBase>();
        public ObservableCollection<KVPBase> Divisions { get { return _divisions; } }

        internal void UpdateSkladState()
        {
            //TODO Get State from server
            for (int i = 1; i < 5; i++)
                SkladState.Add(new ItemSkladDataModel
                {
                    id = i,
                    ArtNo = "asd",
                    BaseValue = 500,
                    Division = new KVPBase { id = 0, Name = "Склад №" + i },
                    Info = "Info" + i,
                    LinkPhoto = "",
                    Name = "Item " + i,
                    Price = 50,
                    Purchase = 40,
                    Quantity = 20,
                    Type = Types[0],
                    Unit = Units[1],
                    Uses = new KVPBase { id = 0, Name = "Для всех" },
                    Vendor = new KVPBase { id = 0, Name = "Vendor " + i }
                });
            OnLoadSkladStateComplete();
        }

        internal KVPBase GetVendor(string value)
        {
            return GetKvpBase(Vendors, value, "vendors");
        }


        internal KVPBase GetTypes(string value)
        {
            return GetKvpBase(Types, value, "types");
        }


        internal KVPBase GetUnit(string value)
        {
            return GetKvpBase(Units, value, "units");
        }

        internal KVPBase GetUses(string value)
        {
            return GetKvpBase(Uses, value, "uses");
        }

        internal KVPBase GetDivision(string value)
        {
            return GetKvpBase(Divisions, value, "divisions");
        }

        private KVPBase GetKvpBase(ICollection<KVPBase> list, string value, string property)
        {
            if (string.IsNullOrEmpty(value)) return new KVPBase { id = -1, Name = "" };
            var res = list.FirstOrDefault(o => o.Name.Equals(value));
            if (res != null) return res;
            res = new KVPBase { Name = value, id = list.Count + 1 };
            list.Add(res);
            UpdateCache(list, property);
            return res;
        }

        private void UpdateCache(ICollection<KVPBase> list, string property)
        {
            try
            {
                var data = JsonConvert.SerializeObject(list);
                Save(data, property);
            }
            catch { }
        }

        #region Request handlers

        private void SendRequest(string req)
        {
            try
            {
                TCPConnection.Instance.SendData(req);
            }
            catch
            {
            }
        }

        #endregion

        #region TCP

        public void Split(char fx, string row)
        {
            switch (fx)
            {
                case 'A':
                case 'a':
                    FillSkladState(row);
                    break;
                case 'C':
                case 'c':
                    AddPosition(row);
                    break;
            }
        }

        private void AddPosition(string row)
        {
            try
            {
                var item = JsonConvert.DeserializeObject<ItemSkladDataModel>(row);
                var itemSklad = SkladState.FirstOrDefault(o => o.id == item.id);
                if (itemSklad != null) return;
                if (Application.Current != null)
                    Application.Current.Dispatcher.BeginInvoke(new Action(() => SkladState.Add(item)));

                SaveStateSklad();
                OnLoadSkladStateComplete();
            }
            catch (Exception e)
            {

            }

        }

        private void FillSkladState(string row)
        {
            try
            {
                var temp = JsonConvert.DeserializeObject<List<ItemSkladDataModel>>(row);
                if (temp == null) return;
                if (Application.Current != null)
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {

                    }));
            }
            catch { }
        }
        #endregion


        public void SaveStateSklad()
        {
            try
            {
                var data = JsonConvert.SerializeObject(SkladState);
                Save(data, "statesklad");
            }
            catch { }
        }


        private void Save(string data, string filename)
        {
            try
            {
                var docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var way = docPath + "\\M2B\\ServiceViewer\\Cache\\";
                if (!Directory.Exists(way)) { Directory.CreateDirectory(way); }
                var file = way + string.Format("{0}.m2bs", filename);
                if (File.Exists(file)) File.Delete(file);

                var sw = File.CreateText(file);
                sw.WriteLine(data);
                sw.Close();
            }
            catch { }
        }

        public string LoadStateSklad(string filename)
        {
            try
            {
                var docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var way = docPath + "\\M2B\\ServiceViewer\\";
                if (!Directory.Exists(way)) return "";
                var file = way + string.Format("{0}.m2bs", filename);
                if (!File.Exists(file)) return "";
                var sr = File.OpenText(file);
                var data = sr.ReadToEnd();
                return data;

            }
            catch { }
            return "";
        }

        internal void AddPosition(ItemSkladDataModel item)
        {
            var itemSklad = SkladState.FirstOrDefault(o => o.id == item.id);
            if (itemSklad == null)
            {
                //TODO Send Model from TCP
                item.id = SkladState.Count;
                var data = JsonConvert.SerializeObject(item);
                Split('C', data);
                //SendRequest("XC;" + data);
            }
            else
            {
                itemSklad.Division = item.Division;
                itemSklad.Info = item.Info;
                itemSklad.LinkPhoto = item.LinkPhoto;
                itemSklad.Name = item.Name;
                itemSklad.Price = item.Price;
                itemSklad.Purchase = itemSklad.Purchase;
                itemSklad.Quantity = item.Quantity;
                itemSklad.Type = item.Type;
                itemSklad.Unit = item.Unit;
                itemSklad.Uses = item.Uses;
                itemSklad.Vendor = item.Vendor;
                itemSklad.ArtNo = item.ArtNo;
                SendUpdateItem(itemSklad);
                SaveStateSklad();
            }
        }

        internal void AddSkladPosition(ItemSkladDataModel item)
        {
            var itemSklad = SkladState.FirstOrDefault(o => o.id == item.id);
            if (itemSklad == null)
            {
                //TODO Send Model from TCP
                item.id = SkladState.Count;
                var data = JsonConvert.SerializeObject(item);
                Split('C', data);
                //SendRequest("XC;" + data);
            }
            else
            {
                SendUpdateItem(itemSklad);
                SaveStateSklad();
                OnLoadSkladStateComplete();
            }
        }

        private void SendUpdateItem(ItemSkladDataModel item)
        {
            var data = JsonConvert.SerializeObject(item);
            //SendRequest("XB;" + data);
        }



    }
}
