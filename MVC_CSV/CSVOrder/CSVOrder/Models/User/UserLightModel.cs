using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CSVOrder.Models.User
{
    public class UserLightModel
    {
        public UserLightModel()
        {
            Id = 0;
            Tp = 2;
        }

        [HiddenInput(DisplayValue = false)]
        [Required]
        public int Id { get; set; }

        [Display(Name = "Имя пользователя")]
        [Required(ErrorMessage = "Введите имя пользователя")]
        public string Nm { get; set; }

        [Display(Name = "Телефон")]
        [Required(ErrorMessage = "Укажите телефон")]
        public string Ph { get; set; }

        [Display(Name = "Email")]
        public string Em { get; set; }

        /// <summary>
        /// тип того, кто записывается на ТО
        /// </summary>

        public int Tp { get; set; }
    }
}