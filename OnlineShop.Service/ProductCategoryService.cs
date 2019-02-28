using OnlineShop.Data.Infrastructure;
using OnlineShop.Data.Models;
using OnlineShop.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Service
{
    public interface IProductCategoryService
    {
        ProductCategory Add(ProductCategory ProductCategory);

        void Update(ProductCategory ProductCategory);

        ProductCategory Delete(Guid id);

        IEnumerable<ProductCategory> GetAll();

        IEnumerable<ProductCategory> GetByDisplayOrder();

        IEnumerable<ProductCategory> GetAllPaging(string q, int page, int pageSize, out int totalRow, bool isDisplayOrder = false);

        IEnumerable<ProductCategory> GetAll(string keyword);

        IEnumerable<ProductCategory> GetParentByDisplayOrder();

        IEnumerable<ProductCategory> GetChildByDisplayOrder();

        IEnumerable<ProductCategory> GetProductCategoryByParentID(Guid parentID);

        ProductCategory GetByID(Guid id);

        void SaveChanges();
    }

    public class ProductCategoryService : IProductCategoryService
    {
        private IProductCategoryRepository _productCategoryRepository;
        private IUnitOfWork _unitOfWork;

        public ProductCategoryService(IProductCategoryRepository productCategoryRepository, IUnitOfWork unitOfWork)
        {
            this._productCategoryRepository = productCategoryRepository;
            this._unitOfWork = unitOfWork;
        }

        public ProductCategory Add(ProductCategory productCategory)
        {
            productCategory.CreatedDate = DateTime.Now;
            return _productCategoryRepository.Add(productCategory);
        }

        public void Update(ProductCategory productCategory)
        {
            productCategory.UpdatedDate = DateTime.Now;
            _productCategoryRepository.Update(productCategory);
        }

        public ProductCategory Delete(Guid id)
        {
            return _productCategoryRepository.Delete(id);
        }

        public IEnumerable<ProductCategory> GetAll()
        {
            return _productCategoryRepository.GetAll().OrderBy(x=>x.DisplayOrder);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }
       
        public ProductCategory GetByID(Guid id)
        {
            return _productCategoryRepository.GetSingleById(id);
        }


        public IEnumerable<ProductCategory> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _productCategoryRepository.GetMulti(x => x.Name.Contains(keyword) || x.Description.Contains(keyword));
            else
                return _productCategoryRepository.GetAll();
        }

        public IEnumerable<ProductCategory> GetAllPaging(string q, int page, int pageSize, out int totalRow, bool isDisplayOrder = false)
        {
            var query = _productCategoryRepository.GetAll();
            if (isDisplayOrder)
            {
                if (!string.IsNullOrEmpty(q))
                    query = _productCategoryRepository.GetMulti(x => x.Name.ToLower().Contains(q.ToLower())
                    || x.Description.ToLower().Contains(q.ToLower())).OrderByDescending(x => x.CreatedDate);
                totalRow = query.Count();
                return query.OrderBy(x => x.DisplayOrder).Skip((page - 1) * pageSize).Take(pageSize);
            }
            else
            {
                if (!string.IsNullOrEmpty(q))
                    query = _productCategoryRepository.GetMulti(x => x.Name.ToLower().Contains(q.ToLower())
                    || x.Description.ToLower().Contains(q.ToLower())).OrderByDescending(x => x.CreatedDate);
                totalRow = query.Count();
                return query.OrderByDescending(x => x.CreatedDate).Skip((page - 1) * pageSize).Take(pageSize);
            }
        }

        public IEnumerable<ProductCategory> GetByDisplayOrder()
        {
            var query = _productCategoryRepository.GetMulti(x => x.Status).OrderBy(x => x.DisplayOrder);
            return query;
        }

        public IEnumerable<ProductCategory> GetParentByDisplayOrder()
        {
            var query = _productCategoryRepository.GetMulti(x => x.Status && x.ParentID == null).OrderBy(x => x.DisplayOrder);
            return query;
        }

        public IEnumerable<ProductCategory> GetChildByDisplayOrder()
        {
            var query = _productCategoryRepository.GetMulti(x => x.Status && x.ParentID != null).OrderBy(x => x.DisplayOrder);
            return query;
        }

        public IEnumerable<ProductCategory> GetProductCategoryByParentID(Guid parentID)
        {
            return _productCategoryRepository.GetMulti(x => x.Status && x.ParentID == parentID);
        }
    }
}
