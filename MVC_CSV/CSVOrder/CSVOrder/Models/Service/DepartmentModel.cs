using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CSVOrder.Models.Service
{
    public class DepartmentModel
    {
        public DepartmentModel()
        {
            Posts = new List<PostModel>();
        }

        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name = "Название")]
        [Required(ErrorMessage = "Введите название департамента")]
        public string Name { get; set; }

        [Display(Name = "Телефон")]
        public string Phone { get; set; }

        [Display(Name = "Время начала работы")]
        public TimeSpan From { get; set; }

        [Display(Name = "Время окончания работы")]
        public TimeSpan To { get; set; }

        [Display(Name = "Стоимость нормо/часа (руб)")]
        public int NhCost { get; set; }

        [Display(Name = "Адрес")]
        public string Adress { get; set; }

        [Display(Name = "Посты")]
        public List<PostModel> Posts { get; set; }
    }
}