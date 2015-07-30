using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CSVOrder.Models.Service
{
    public class CarModel
    {
        [Display(Name = "VIN", Order = 1)]
        public string VIN { get; set; }
        [Required]
        [Display(Name = "Гос. номер", Order = 2)]
        public string Number { get; set; }

        [HiddenInput(DisplayValue = false)]
        [Required]
        public int IdUser { get; set; }

        [HiddenInput(DisplayValue = false)]
        [Required]
        public int IdMark { get; set; }

        [HiddenInput(DisplayValue = false)]
        [Required]
        public int IdModel { get; set; }

        [HiddenInput(DisplayValue = false)]
        [Required]
        public int IdEngine { get; set; }

        [HiddenInput(DisplayValue = false)]
        [Required]
        public int IdBody { get; set; }

        [HiddenInput(DisplayValue = false)]
        [Required]
        public int IdEngineType { get; set; }

        [HiddenInput(DisplayValue = false)]
        [Required]
        public int IdTransmission { get; set; }

        [Required]
        [Display(Name = "Пробег", Order = 10)]
        public int Mileage { get; set; }

        [Required]
        [Display(Name = "Дата выпуска", Order = 10)]
        public DateTime Date { get; set; }
    }
}