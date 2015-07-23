using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CSVOrder.Models.Service
{
    public class PostModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int IdPostType { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int IdDep { get; set; }

        [Display(Name = "Название")]
        [Required(ErrorMessage = "Введите название поста")]
        public string Name { get; set; }

        [Display(Name = "Время начала работы поста")]
        public TimeSpan TimeFrom { get; set; }

        [Display(Name = "Время окончания работы поста")]
        public TimeSpan TimeTo { get; set; }
    }
}