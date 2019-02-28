using OnlineShop.Data.Infrastructure;
using OnlineShop.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Data.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        IEnumerable<Product> GetListProductByTag(string tagId, int page, int pageSize, out int totalRow);

        IQueryable<Product> GetListProductByTag(Guid categoryId);

    }

    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

       

        public IEnumerable<Product> GetListProductByCategoryID(Guid CategoryID)
        {
            var product = (from p in DbContext.Products where p.ProductCategoryID.Equals(CategoryID) select p);

            return product;
        }

        public IEnumerable<Product> GetListProductByTag(string tagId, int page, int pageSize, out int totalRow)
        {
            var query = from p in DbContext.Products
                        join pt in DbContext.ProductTags
                        on p.ProductID equals pt.ProductID
                        where pt.TagID == tagId
                        select p;
            totalRow = query.Count();

            return query.OrderByDescending(x => x.CreatedDate).Skip((page - 1) * pageSize).Take(pageSize);
        }

        public IQueryable<Product> GetListProductByTag(Guid categoryId)
        {
            var query = from p in DbContext.Products
                        join m in DbContext.Manufactures
                        on p.ManufactureID equals m.ManufactureID
                        where p.ProductCategoryID == categoryId
                        select p;
            return  query.GroupBy(x => x.ManufactureID).Select(m => m.First());
        }
    }
}
