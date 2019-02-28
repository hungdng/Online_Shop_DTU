using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Data.Models
{
    [Table("Products")]
    public class Product
    {
        public Guid ProductID { get; set; }

        [Required]
        [MaxLength(256)]
        public string ProductName { get; set; }

        [Required]
        [MaxLength(256)]
        public string Alias { get; set; }

        [Required]
        public Guid ProductCategoryID { get; set; }

        [Required]
        public Guid ManufactureID { get; set; }

        [MaxLength(256)]
        public string ThumbnailImage { get; set; }

        [StringLength(50)]
        public string CalculationUnit { get; set; }

        public decimal Price { get; set; }

        public decimal OriginalPrice { get; set; }

        public decimal? PromotionPrice { get; set; }

        public bool IncludedVAT { get; set; }

        public int? Warranty { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public string FullDescription { get; set; }
        
        public int? ViewCount { get; set; }

        public string Tags { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsBuy { get; set; }

        public bool HotFlag { get; set; }

        public DateTime? CreatedDate { get; set; }

        [MaxLength(256)]
        public string CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [MaxLength(256)]
        public string UpdatedBy { get; set; }

        public int Quantity { get; set; }


        [ForeignKey("ProductCategoryID")]
        public virtual ProductCategory ProductCategory { get; set; }

        [ForeignKey("ManufactureID")]
        public virtual Manufacture Manufactures { get; set; }

        public virtual IEnumerable<OrderDetail> OrderDetails { get; set; }

        public virtual ICollection<ProductTag> ProductTags { get; set; }
    }
}
