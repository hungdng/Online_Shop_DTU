using OnlineShop.Common;
using OnlineShop.Common.Helpers;
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
    public interface IProductService
    {
        Product Add(Product product);

        void Update(Product product);

        Product Delete(Guid id);

        IEnumerable<Product> GetAll();

        IEnumerable<Product> GetAll(Guid? categoryId);

        // san pham moi nhat
        IEnumerable<Product> GetLastest(int top);

        // san pham hot
        IEnumerable<Product> GetHotProduct(int top);

        // san pham giam gia
        IEnumerable<Product> GetPromotionProduct(int top);


        IEnumerable<Product> GetListProduct(string keyword);

        //san pham lien qua
        IEnumerable<Product> GetReatedProduct(Guid id, int top);

        IEnumerable<string> GetListProductByName(string name);

        IEnumerable<Product> GetListObjProductByName(string name);

        Product GetById(Guid id);

        IEnumerable<Product> GetByCategoryId(Guid id);

        IEnumerable<Product> GetByCategoryId(Guid id, int top);

        IEnumerable<Product> GetListProductByCategoryIdPaging(Guid categoryId, int page, int pageSize, string sort, out int totalRow);

        IEnumerable<Product> GetIncludeManufacturePaging(string keyword, int page, Guid? manufactureId, int pageSize, out int totalRow);

        IEnumerable<Product> Search(string keyword, int page, int pageSize, string sort, out int totalRow);

        IEnumerable<Product> GetByManufacture(Guid id);

        IEnumerable<Product> GetByManufacture(Guid id, int top);

        IEnumerable<Product> GetByManufacture(Guid id, Guid categoryID, int top);

        void Save();

        IEnumerable<Tag> GetListTagByProductId(Guid id);

        Tag GetTag(string tagId);

        void IncreaseView(Guid id);

        IEnumerable<Product> GetListProductByTag(string tagId, int page, int pageSize, out int totalRow);

        bool SellProduct(Guid productId, int quantity);

        IEnumerable<Tag> GetListProductTag(string keyword);

        IEnumerable<Product> GetListProductJsonByName(string name);
    }

    public class ProductService : IProductService
    {
        private IProductRepository _productRepository;
        private ITagRepository _tagRepository;
        private IProductTagRepository _productTagRepository;

        private IUnitOfWork _unitOfWork;

        public ProductService(IProductRepository productRepository,
            ITagRepository tagRepository,
            IProductTagRepository productTagRepository,
            IUnitOfWork unitOfWork)
        {
            this._productRepository = productRepository;
            this._tagRepository = tagRepository;
            this._productTagRepository = productTagRepository;
            this._unitOfWork = unitOfWork;
        }

        public Product Add(Product product)
        {
            var lstStrIDTag = new List<string>();
            var addproduct = _productRepository.Add(product);
            _unitOfWork.Commit();

            if (!string.IsNullOrEmpty(product.Tags))
            {
                string[] tags = product.Tags.Split(',');
                for (int i = 0; i < tags.Length; i++)
                {
                    var tagId = StringHelper.ToUnsignString(tags[i]);
                    var xx = lstStrIDTag.Where(x => x.Equals(tagId));
                    if (xx.Count() == 0)
                    {
                        if (_tagRepository.Count(x => x.ID == tagId) == 0)
                        {
                            Tag tag = new Tag();
                            tag.ID = tagId;
                            tag.Name = tags[i];
                            tag.Type = CommonConstants.ProductTag;
                            _tagRepository.Add(tag);
                            lstStrIDTag.Add(tagId);
                        }

                        ProductTag productTag = new ProductTag();
                        productTag.ProductID = product.ProductID;
                        productTag.TagID = tagId;
                        _productTagRepository.Add(productTag);
                    }
                }
            }
            return addproduct;
        }

        public Product Delete(Guid id)
        {
            return _productRepository.Delete(id);
        }

        public IEnumerable<Product> GetAll()
        {
            return _productRepository.GetAll(new string[] { "ProductCategory", "ProductTags" });

        }

        public IEnumerable<Product> GetAll(Guid? categoryId)
        {
            var query = _productRepository.GetMulti(x => x.IsDeleted == false);
            if (categoryId.HasValue)
                query = query.Where(x => x.ProductCategoryID == categoryId);
            return query;
        }

        public IEnumerable<Product> GetAll(Guid? categoryId, string keyword)
        {
            var query = _productRepository.GetMulti(x => x.IsDeleted == false, new string[] { "ProductCategory", "ProductTags" });
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.ProductName.ToLower().Contains(keyword.ToLower()) || x.Alias.ToLower().Contains(keyword.ToLower()));
            if (categoryId.HasValue)
                query = query.Where(x => x.ProductCategoryID == categoryId);

            return query;
        }



        public IEnumerable<Product> GetByCategoryId(Guid id)
        {
            return _productRepository.GetMulti(x => x.ProductCategoryID.Equals(id));
        }

        public IEnumerable<Product> GetByCategoryId(Guid id, int top)
        {
            return _productRepository.GetMulti(x => x.ProductCategoryID.Equals(id)).OrderBy(x => x.CreatedBy).Take(top);
        }

        public Product GetById(Guid id)
        {
            return _productRepository.GetSingleById(id);
        }

        public IEnumerable<Product> GetByManufacture(Guid id)
        {
            return _productRepository.GetMulti(x => x.ManufactureID.Equals(id));
        }

        public IEnumerable<Product> GetByManufacture(Guid id, int top)
        {
            return _productRepository.GetMulti(x => x.ManufactureID.Equals(id)).OrderBy(x => x.CreatedBy).Take(top);
        }

        public IEnumerable<Product> GetByManufacture(Guid id, Guid categoryID, int top)
        {
            return _productRepository.GetMulti(x => x.ManufactureID.Equals(id) && x.ProductCategoryID.Equals(categoryID)).OrderBy(x => x.CreatedBy).Take(top);
        }

        public IEnumerable<Product> GetHotProduct(int top)
        {
            return _productRepository.GetMulti(x => !x.IsDeleted && x.HotFlag == true).OrderBy(x => x.CreatedDate).Take(top);
        }

        public IEnumerable<Product> GetIncludeManufacturePaging(string keyword, int page, Guid? manufactureId, int pageSize, out int totalRow)
        {
            var query = _productRepository.GetMulti(x => x.IsBuy && x.IsDeleted == false, new string[] { "Manufactures" });

            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.ToLower();
                query = _productRepository.GetMulti(x => x.ProductName.ToLower().Contains(keyword)
                    || x.Manufactures.ManufactureName.ToLower().Contains(keyword));
            }
            if (manufactureId != null)
            {
                query = _productRepository.GetMulti(x => x.ManufactureID == manufactureId);
            }
            totalRow = query.Count();
            return query.OrderByDescending(x => x.CreatedDate).Skip((page - 1) * pageSize).Take(pageSize);
        }

        public IEnumerable<Product> GetLastest(int top)
        {
            return _productRepository.GetMulti(x => !x.IsDeleted).OrderByDescending(x => x.CreatedDate).Take(top);
        }

        public IEnumerable<Product> GetListObjProductByName(string name)
        {
            return _productRepository.GetMulti(x => !x.IsDeleted && x.ProductName.Contains(name));
        }

        public IEnumerable<Product> GetListProduct(string keyword)
        {
            IEnumerable<Product> query;
            if (!string.IsNullOrEmpty(keyword))
                query = _productRepository.GetMulti(x => x.ProductName.Contains(keyword));
            else
                query = _productRepository.GetAll();

            return query;
        }

        public IEnumerable<Product> GetListProductByCategoryIdPaging(Guid categoryId, int page, int pageSize, string sort, out int totalRow)
        {
            var query = _productRepository.GetMulti(x => x.ProductCategoryID == categoryId);

            switch (sort)
            {
                case "popular":
                    query = query.OrderByDescending(x => x.ViewCount);
                    break;
                case "discount":
                    query = query.OrderByDescending(x => x.PromotionPrice > 0);
                    break;
                case "price":
                    query = query.OrderBy(x => x.Price);
                    break;
                default:
                    query = query.OrderByDescending(x => x.CreatedDate);
                    break;
            }

            totalRow = query.Count();

            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public IEnumerable<string> GetListProductByName(string name)
        {
            return _productRepository.GetMulti(x => !x.IsDeleted && x.ProductName.Contains(name)).Select(y => y.ProductName);
        }

        public IEnumerable<Product> GetListProductByTag(string tagId, int page, int pageSize, out int totalRow)
        {
            var model = _productRepository.GetListProductByTag(tagId, page, pageSize, out totalRow);
            return model;
        }

        public IEnumerable<Tag> GetListProductTag(string keyword)
        {
            return _tagRepository.GetMulti(x => x.Type == CommonConstants.ProductTag && x.Name.Contains(keyword));
        }

        public IEnumerable<Tag> GetListTagByProductId(Guid id)
        {
            return _productTagRepository.GetMulti(x => x.ProductID == id, new string[] { "Tag" }).Select(y => y.Tag);
        }

        public IEnumerable<Product> GetPromotionProduct(int top)
        {
            return _productRepository.GetMulti(x => !x.IsDeleted && x.PromotionPrice > 0).OrderBy(x => x.UpdatedDate).Take(top);
        }

        public IEnumerable<Product> GetReatedProduct(Guid id, int top)
        {
            var product = _productRepository.GetSingleById(id);

            return _productRepository.GetMulti(x => !x.IsDeleted && x.ProductID != id && x.ProductCategoryID == product.ProductCategoryID).OrderByDescending(x => x.CreatedDate).Take(top);
        }

        public Tag GetTag(string tagId)
        {
            return _tagRepository.GetSingleByCondition(x => x.ID == tagId);
        }

        public void IncreaseView(Guid id)
        {
            var product = _productRepository.GetSingleById(id);
            if (product.ViewCount.HasValue)
                product.ViewCount += 1;
            else
                product.ViewCount = 1;
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public IEnumerable<Product> Search(string keyword, int page, int pageSize, string sort, out int totalRow)
        {
            var query = _productRepository.GetMulti(x => x.IsBuy && x.ProductName.Contains(keyword));

            switch (sort)
            {
                case "popular":
                    query = query.OrderByDescending(x => x.ViewCount);
                    break;
                case "discount":
                    query = query.OrderByDescending(x => x.PromotionPrice.HasValue);
                    break;
                case "price":
                    query = query.OrderBy(x => x.Price);
                    break;
                default:
                    query = query.OrderByDescending(x => x.CreatedDate);
                    break;
            }

            totalRow = query.Count();

            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public bool SellProduct(Guid productId, int quantity)
        {
            var product = _productRepository.GetSingleById(productId);

            return true;
        }

        public void Update(Product product)
        {
            var lstStrIDTag = new List<string>();
            _productRepository.Update(product);
            if (!string.IsNullOrEmpty(product.Tags))
            {
                string[] tags = product.Tags.Split(',');
                for (int i = 0; i < tags.Length; i++)
                {
                    var tagId = StringHelper.ToUnsignString(tags[i]);
                    var xx = lstStrIDTag.Where(x => x.Equals(tagId));
                    if (xx.Count() == 0)
                    {
                        if (_tagRepository.Count(x => x.ID == tagId) == 0)
                        {
                            Tag tag = new Tag();
                            tag.ID = tagId;
                            tag.Name = tags[i];
                            tag.Type = CommonConstants.ProductTag;
                            _tagRepository.Add(tag);
                            lstStrIDTag.Add(tagId);
                        }

                        _productTagRepository.DeleteMulti(x => x.ProductID == product.ProductID);
                        ProductTag productTag = new ProductTag();
                        productTag.ProductID = product.ProductID;
                        productTag.TagID = tagId;
                        _productTagRepository.Add(productTag);
                    }

                }
            }
        }

        public IEnumerable<Product> GetListProductJsonByName(string name)
        {
            return _productRepository.GetMulti(x => !x.IsDeleted && x.ProductName.Contains(name));
        }
    }
}
