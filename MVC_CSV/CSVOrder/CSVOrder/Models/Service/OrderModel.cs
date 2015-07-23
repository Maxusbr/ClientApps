using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CSVOrder.Models.Service
{
    public class OrderModel
    {
        [HiddenInput(DisplayValue = false)]
        public int OrderNumer { get; set; }

        [Display(Name = "Номер автомобиля")]
        public string CarNumber { get; set; }

        [Display(Name = "Время создания заявки")]
        public DateTime DtCreate { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Комментарий пользователя")]
        public string UserComment { get; set; }

        [Display(Name = "Стоимость (руб)")]
        public int Cost { get; set; }
    }
}