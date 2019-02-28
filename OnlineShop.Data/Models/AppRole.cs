using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Data.Models
{
    [Table("AppRoles")]
    public class AppRole : IdentityRole
    {
        public AppRole() : base()
        {}

        public AppRole(string name, string description) : base(name)
        {
            this.Description = description;
        }

        public bool? IsDeleted { get; set; }
        public virtual string Description { get; set; }
    }
}
