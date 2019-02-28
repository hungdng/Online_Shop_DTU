using OnlineShop.Data.Infrastructure;
using OnlineShop.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Data.Repositories
{
    public interface IManufactureRepository : IRepository<Manufacture> {
        IQueryable<Manufacture> GetListManufactureByCategoryId(Guid categoryId);
    }
    public class ManufactureRepository : RepositoryBase<Manufacture>, IManufactureRepository
    {
        public ManufactureRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public IQueryable<Manufacture> GetListManufactureByCategoryId(Guid categoryId)
        {
            var query = from m in DbContext.Manufactures
                        join p in DbContext.Products
                        on m.ManufactureID equals p.ManufactureID
                        where p.ProductCategoryID == categoryId
                        select m;
            return query.Distinct();
        }
    }
}
