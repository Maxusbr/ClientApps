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
            SelectedWorks = new List<WorksInfoDataModel>();
        }

        public CarOrderPostModel(OrderModel model)
        {
            OrderNumer = model.OrderNumer;
            PostId = model.PostId;
            UserId = model.UserId;
            CarNumber = model.CarNumber;
            DtCreate = model.DtCreate;
            UserComment = model.UserComment;
            Cost = model.Cost;
            DateWork = model.DateWork;
        }
        /// <summary>
        /// перемещение в заказ-наряды
        /// </summary>
        [Display(Name = "В работе?")]
        public bool InUse { get; set; }

        [Required(ErrorMessage = "Укажите пользователя")]
        public UserLightModel User { get; set; }

        [Display(Name = "Список работ")]
        public List<WorksInfoDataModel> SelectedWorks { get; set; }
    }
}