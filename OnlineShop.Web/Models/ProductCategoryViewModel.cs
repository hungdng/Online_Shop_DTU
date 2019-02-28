using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Web.Models
{
    public class ProductCategoryViewModel
    {
        public Guid ID { get; set; }

        [Required]
        [StringLength(256, ErrorMessage = "Max length is 256 characters")]
        [Display(Name = "Category Name")]
        public string Name { get; set; }

        [Display(Name = "Alias")]
        [StringLength(256, ErrorMessage = "Max length is 256 characters")]
        public string Alias { get; set; }

        [Display(Name = "Description")]
        [StringLength(700, ErrorMessage = "Max length is 700 characters")]
        public string Description { get; set; }

        [Display(Name = "Created Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yy}")]
        public DateTime? CreatedDate { get; set; }

        [MaxLength(50)]
        [Display(Name = "Generator")]
        public string CreatedBy { get; set; }

        [Display(Name = "Updated Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yy}")]
        public DateTime? UpdatedDate { get; set; }

        [MaxLength(50)]
        [Display(Name = "Updated Person")]
        public string UpdatedBy { get; set; }

        [Display(Name = "Display Order")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Display Order has to be NUMBER")]
        public int? DisplayOrder { get; set; }

        [Required]
        [Display(Name = "Status")]
        public bool Status { get; set; }

        [Display(Name = "Parent List")]
        public Guid? ParentID { get; set; }

        [Display(Name = "Parent List")]
        public IList<SelectListItem> ParentList { get; set; }
    }
}