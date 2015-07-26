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
        [Required]
        public int OrderNumer { get; set; }

        [HiddenInput(DisplayValue = false)]
        [Required]
        public int UserId { get; set; }

        [HiddenInput(DisplayValue = false)]
        [Required]
        public int PostId { get; set; }

        [Display(Name = "Время создания заявки", Order = 1)]
        [Required]
        public DateTime DtCreate { get; set; }

        /// <summary>
        /// планируемая дата начала работ
        /// </summary>
        [Display(Name = "Планируемая дата начала работ", Order = 2)]
        public DateTime DateWork { get; set; }

        [Display(Name = "Номер автомобиля", Order = 3)]
        [Required(ErrorMessage = "Введите номер автомобиля")]
        public string CarNumber { get; set; }

        [Display(Name = "Стоимость (руб)", Order = 5)]
        public int Cost { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Комментарий пользователя", Order = 6)]
        public string UserComment { get; set; }
    }
}