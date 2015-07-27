using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
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

        [Required]
        [Display(Name = "Марка", Order = 3)]
        public List<KVPBase> Marks { get { return _marks; } }
        [Required]
        [Display(Name = "Модель", Order = 4)]
        public List<KVPBase> Models { get { return _models; } }
        [Required]
        [Display(Name = "Тип кузова", Order = 5)]
        public List<KVPBase> BodyTypes { get { return _bodyTypes; } }
        [Required]
        [Display(Name = "Тип двигателя", Order = 6)]
        public List<KVPBase> EngineTypes { get { return _engineTypes; } }
        [Required]
        [Display(Name = "Объем двигателя", Order = 7)]
        public List<KVPBase> EngineVolumes { get { return _engineVolumes; } }
        [Required]
        [Display(Name = "Тип КПП", Order = 8)]
        public List<KVPBase> TransTypes { get { return _transTypes; } }

        public KVPBase Mark { get; set; }

        public KVPBase Model { get; set; }

        public KVPBase EngineType { get; set; }

        public KVPBase EngineVolume { get; set; }

        public KVPBase BodyType { get; set; }

        public KVPBase TransType { get; set; }
    }
}