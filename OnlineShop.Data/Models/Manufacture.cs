using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Data.Models
{
    public class Manufacture
    {
        public Guid ManufactureID { get; set; }

        public string ManufactureName { get; set; }

        public string Logo { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
