using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTCDev.Client.ViewModel;
using DTCDev.Models.CarBase.CarStatData;
using Newtonsoft.Json;

namespace DTCDev.Client.Cars.Service.Engine.Controls.ViewModels.Settings
{
    [JsonObject]
    public class WorksInfoDataCostViewModel : ViewModelBase
    {
        private string _mark;
        private string _model;
        private int _costWork;
        private int _costParts;

        [JsonProperty(PropertyName = "A")]
        public int ID { get; set; }

        [JsonProperty(PropertyName = "B")]
        public int IdWork { get; set; }

        [JsonProperty(PropertyName = "C")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "D")]
        public string Mark
        {
            get { return _mark; }
            set
            {
                _mark = value;
                OnPropertyChanged("Mark");
            }
        }

        [JsonProperty(PropertyName = "E")]
        public string Model
        {
            get { return _model; }
            set
            {
                _model = value;
                OnPropertyChanged("Model");
            }
        }

        [JsonProperty(PropertyName = "F")]
        public int CostWork
        {
            get { return _costWork; }
            set
            {
                _costWork = value;
                OnPropertyChanged("CostWork");
            }
        }

        [JsonProperty(PropertyName = "G")]
        public int CostParts
        {
            get { return _costParts; }
            set
            {
                _costParts = value;
                OnPropertyChanged("CostParts");
            }
        }
        [JsonIgnore]
        public bool IsChanged { get; set; }

        [JsonIgnore]
        public override string DisplayName { get; set; }

        [JsonIgnore]
        public bool IsRoot { get; set; }

        public override bool Equals(object obj)
        {
            var model = obj as WorksInfoDataCostViewModel;
            if(model == null) return false;
            return model.Mark != null && model.Model != null  && 
                Mark.Equals(model.Mark) && Model.Equals(model.Model) && Name.Equals(model.Name);
        }
    }
}
