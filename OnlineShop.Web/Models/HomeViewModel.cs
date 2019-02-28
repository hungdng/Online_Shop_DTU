using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShop.Web.Models
{
    public class HomeViewModel
    {
        // Sản phâm mới
        public IEnumerable<ProductViewModel> LastestProduct { get; set; }

        // Sản phâm khuyến mãi
        public IEnumerable<ProductViewModel> PromotionProduct { get; set; }

        // Sản phâm Hot
        public IEnumerable<ProductViewModel> HotProduct { get; set; }

        // Danh sach thuong hieu
        public IEnumerable<ManufactureViewModel> Manufactures { get; set; }

        // Sản phâm theo category
        public IEnumerable<ProductViewModel> ProductByCategory { get; set; }

        // Lấy category cha
        public IEnumerable<ManufacturetInTabViewModel> ManufacturetInTab { get; set; }
    }
}