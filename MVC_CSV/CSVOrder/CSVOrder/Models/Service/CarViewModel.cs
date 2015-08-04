using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CSVOrder.DAL.Abstract;
using CSVOrder.Models.Abstract;

namespace CSVOrder.Models.Service
{
    public class CarViewModel : CarModel
    {
        private readonly List<KVPBase> _marks = new List<KVPBase>();
        private readonly List<KVPBase> _models = new List<KVPBase>();
        private readonly List<KVPBase> _engineTypes = new List<KVPBase>();
        private readonly List<KVPBase> _engineVolumes = new List<KVPBase>();
        private readonly List<KVPBase> _bodyTypes = new List<KVPBase>();
        private readonly List<KVPBase> _transTypes = new List<KVPBase>();

        public CarViewModel(string carNumber)
        {
            Number = carNumber ?? string.Empty;
        }

        [Display(Name = "Марка", Order = 3)]
        public List<KVPBase> Marks { get { return _marks; } }

        [Display(Name = "Модель", Order = 4)]
        public List<KVPBase> Models { get { return _models; } }

        [Display(Name = "Тип кузова", Order = 5)]
        public List<KVPBase> BodyTypes { get { return _bodyTypes; } }

        [Display(Name = "Тип двигателя", Order = 6)]
        public List<KVPBase> EngineTypes { get { return _engineTypes; } }

        [Display(Name = "Объем двигателя", Order = 7)]
        public List<KVPBase> EngineVolumes { get { return _engineVolumes; } }

        [Display(Name = "Тип КПП", Order = 8)]
        public List<KVPBase> TransTypes { get { return _transTypes; } }

        public string Mark { get; set; }

        public string Model { get; set; }

        public string EngineType { get; set; }

        public string EngineVolume { get; set; }

        public string BodyType { get; set; }

        public string TransType { get; set; }

        private void ClearLists()
        {
            Marks.Clear(); Models.Clear();
            EngineTypes.Clear(); EngineVolumes.Clear(); BodyTypes.Clear(); TransTypes.Clear();
        }

        internal void Update(IServiseRepository storage)
        {
            ClearLists();
            Marks.AddRange(storage.Marks);
            Models.AddRange(storage.Models);
            BodyTypes.AddRange(storage.BodyTypes);
            EngineTypes.AddRange(storage.EngineTypes);
            EngineVolumes.AddRange(storage.EngineVolumes);
            TransTypes.AddRange(storage.TransTypes);
        }
    }
}