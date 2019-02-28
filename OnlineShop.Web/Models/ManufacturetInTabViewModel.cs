using OnlineShop.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShop.Web.Models
{
    public class ManufacturetInTabViewModel
    {
        public Guid Id { get; set; }

        public string CategoryName { get; set; }

        public IEnumerable<ManufactureViewModel> ListManufacture { get; set; }
    }
}