using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShop.Web.Areas.Admin.Models
{
    public class AppUserViewModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string BirthDay { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public bool Status { get; set; }

        public string Gender { get; set; }

        public string UserTypeID { get; set; }

        public ICollection<string> Roles { get; set; }
    }
}