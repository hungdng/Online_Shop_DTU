using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Common.Extensions
{
    public static class Extensions
    {
        public static SelectList GenderSelectList()
        {
            var selectList = new SelectList(
                   new List<SelectListItem>
                   {
                        new SelectListItem {Text = "Nam", Value = "Nam"},
                        new SelectListItem {Text = "Nữ", Value = "Nữ"},
                   }, "Value", "Text");
            return selectList;
        }
    }
}
