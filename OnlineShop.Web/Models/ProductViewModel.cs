using MvcPaging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;

namespace OnlineShop.Web.Models
{
    [Serializable]
    public class ProductViewModel
    {
        public Guid ProductID { get; set; }

        [Display(Name = "Product Name")]
        [Required(ErrorMessage = "Please enter Product Name")]
        [StringLength(256, ErrorMessage = "Max length is 256 characters")]
        public string ProductName { get; set; }

        public string Alias { get; set; }

        [Display(Name = "Product Category")]
        [Required(ErrorMessage = "Please choose ONE Category")]
        [BindRequired]
        public Guid ProductCategoryID { get; set; }

        [Display(Name = "Manufacture")]
        [Required(ErrorMessage = "Please choose ONE Manufacture")]
        [BindRequired]
        public Guid ManufactureID { get; set; }

        [Display(Name = "Thumbnail Image")]
        [StringLength(256, ErrorMessage = "Max length is 256 characters")]
        public string ThumbnailImage { get; set; }

        [Display(Name = "Unit")]
        public string CalculationUnit { get; set; }

        [Display(Name = "Sale Price")]
        [Required(ErrorMessage = "Please enter Sale Price")]
        public decimal Price { get; set; }

        [Display(Name = "Historical Price")]
        [Required(ErrorMessage = "Please enter Historical Price")]
        public decimal OriginalPrice { get; set; }

        [Display(Name = "Promotional Pricing")]
        public decimal? PromotionPrice { get; set; }

        public bool IncludedVAT { get; set; }

        [Display(Name = "Warranty")]
        public int? Warranty { get; set; }

        [Display(Name = "Short Description")]
        [StringLength(700, ErrorMessage = "Max length is 700 characters")]
        public string Description { get; set; }

        [Display(Name = "Full Description")]
        public string FullDescription { get; set; }

        [Display(Name = "Views")]
        public int? ViewCount { get; set; }

        public string Tags { get; set; }

        [Display(Name = "Deleted")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Out Of Stock")]
        public bool IsBuy { get; set; }

        [Display(Name = "Hot Product")]
        public bool HotFlag { get; set; }

        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        public DateTime? CreatedDate { get; set; }
        
        public string CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public virtual ProductCategoryViewModel ProductCategory { get; set; }

        public IPagedList<ProductViewModel> ListProducts { get; set; }

        public virtual ManufactureViewModel Manufactures { get; set; }

        public IList<SelectListItem> CategoryList { get; set; }

    }
}