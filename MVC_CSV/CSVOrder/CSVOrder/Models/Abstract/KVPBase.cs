using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CSVOrder.Models.Abstract
{
    public class KVPBase
    {
        [HiddenInput(DisplayValue = false)]
        [Required]
        public int id { get; set; }

        [Display(Name = "Название")]
        [Required]
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}