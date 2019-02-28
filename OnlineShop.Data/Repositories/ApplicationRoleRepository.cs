using OnlineShop.Data.Infrastructure;
using OnlineShop.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Data.Repositories
{
    public interface IApplicationRoleRepository : IRepository<AppRole>
    {
    }

    public class ApplicationRoleRepository : RepositoryBase<AppRole>, IApplicationRoleRepository
    {
        public ApplicationRoleRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
