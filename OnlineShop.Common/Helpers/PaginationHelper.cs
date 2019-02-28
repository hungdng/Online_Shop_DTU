using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Common
{
    public static class PaginationHelper
    {
        public static int TotalPages(int totalRow, int pageSize)
        {
            return (int)Math.Ceiling((double)totalRow / pageSize);
        }
        public static int MaxPage()
        {
            return int.Parse(ConfigHelper.GetByKey("maxPage"));
        }
        public static int PageSize()
        {
            return int.Parse(ConfigHelper.GetByKey("pageSize"));
        }
    }
}
