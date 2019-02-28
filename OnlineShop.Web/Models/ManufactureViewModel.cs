using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineShop.Web.Models
{
    public class ManufactureViewModel
    {
        public Guid ManufactureID { get; set; }

        [Required(ErrorMessage = "Name is REQUIRED")]
        [Display(Name = "Manufacture Name")]
        [StringLength(150, ErrorMessage = "Max length is 150 characters")]
        public string ManufactureName { get; set; }

        [Display(Name = "Logo")]
        [StringLength(256, ErrorMessage = "Max length is 256 characters")]
        public string Logo { get; set; }

        [Display(Name = "Description")]
        [StringLength(750, ErrorMessage = "Max length is 750 characters")]
        public string Description { get; set; }

        [Url]
        [Display(Name = "Link")]
        [StringLength(100, ErrorMessage = "Max length is 100 characters")]
        public string Url { get; set; }
    }
}