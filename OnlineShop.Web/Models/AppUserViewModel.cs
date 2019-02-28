using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineShop.Web.Models
{
    public class AppUserViewModel
    {
        public string Id { set; get; }

        [Display(Name = "Fullname")]
        [Required(ErrorMessage = "Fullname is REQUIRED")]
        [StringLength(256, ErrorMessage = "Max length is 256 characters")]
        public string FullName { set; get; }

        [Display(Name = "Date Of Birth")]
        [Required(ErrorMessage = "Birthday is REQUIRED")]
        public DateTime BirthDay { set; get; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is REQUIRED")]
        public string Email { set; get; }

        [Required(ErrorMessage = "Password is REQUIRED")]
        [Display(Name = "Password")]
        public string Password { set; get; }

        [Display(Name = "Username")]
        public string UserName { set; get; }

        [Display(Name = "Gender")]
        public string Gender { set; get; }

        [Display(Name = "Deleted")]
        public bool IsDeleted { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { set; get; }

        public string Address { get; set; }

        public List<string> Roles { get; set; }
    }
}