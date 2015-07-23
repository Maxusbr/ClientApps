using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CSVOrder.Models.Service
{
    public class WorksInfoDataModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int IdWork { get; set; }

        [Display(Name = "Название")]
        [Required(ErrorMessage = "Введите название работы")]
        public string Name { get; set; }

        
        public int IdClass { get; set; }

        
        public int Nh { get; set; }

        
        public decimal Nhd { get; set; }

        
        public int Cost { get; set; }

        
        public string Wguid { get; set; }


        public int IsPeriodic { get; set; }
    }
}