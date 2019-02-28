using OnlineShop.Data.Infrastructure;
using OnlineShop.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Data.Repositories
{
    public interface IApplicationUserRepository : IRepository<AppUser>
    {

    }

    public class ApplicationUserRepository : RepositoryBase<AppUser>, IApplicationUserRepository
    {
        public ApplicationUserRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

    }
}
