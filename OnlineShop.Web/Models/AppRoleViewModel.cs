using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineShop.Web.Models
{
    public class AppRoleViewModel
    {
        public string Id { set; get; }
        [DisplayName("Role Name")]
        [Required(ErrorMessage = "Confirm Role Name")]
        [StringLength(128, ErrorMessage = "Max length is 128 characters")]
        public string Name { set; get; }
        public bool? IsDeleted { get; set; }

        [DisplayName("Role Description")]
        [Required(ErrorMessage = "Confirm Role Description")]
        [StringLength(500, ErrorMessage = "Max length is 500 characters")]
        public string Description { set; get; }
    }
}