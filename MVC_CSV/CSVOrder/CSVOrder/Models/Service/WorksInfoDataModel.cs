using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CSVOrder.Models.Abstract;

namespace CSVOrder.Models.Service
{
    public class WorksInfoDataModel : ITreeViewModel
    {
        public WorksInfoDataModel()
        {
            Id = 0;
            Name = "";
        }

        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int IdParent { get; set; }

        [Display(Name = "Название")]
        [Required(ErrorMessage = "Введите название работы")]
        public string Name { get; set; }

        
        public int IdClass { get; set; }

        
        public int Nh { get; set; }

        
        public decimal Nhd { get; set; }

        
        public int Cost { get; set; }

        
        public string Wguid { get; set; }


        public int IsPeriodic { get; set; }

        /// <summary>
        /// Значение = Id для работы и пустое для подгруппы
        /// </summary>
        public string NavUrl { get; set; }
    }
}