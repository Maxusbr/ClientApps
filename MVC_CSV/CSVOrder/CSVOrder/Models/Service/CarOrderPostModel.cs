using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using CSVOrder.Models.User;

namespace CSVOrder.Models.Service
{
    public class CarOrderPostModel : OrderModel
    {
        public CarOrderPostModel()
        {
            Works = new List<WorksInfoDataModel>();
        }

        /// <summary>
        /// перемещение в заказ-наряды
        /// </summary>
        
        public int InUse { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int PostId { get; set; }

        
        public UserLightModel User { get; set; }

        /// <summary>
        /// планируемая дата начала работ
        /// </summary>
        [Display(Name = "Планируемая дата начала работ")]
        public DateTime DateWork { get; set; }

        [Display(Name = "Список работ")]
        public List<WorksInfoDataModel> Works { get; set; }
    }
}