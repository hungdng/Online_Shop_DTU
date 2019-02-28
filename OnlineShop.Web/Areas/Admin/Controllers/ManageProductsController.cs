using AutoMapper;
using OnlineShop.Common;
using OnlineShop.Data.Models;
using OnlineShop.Service;
using OnlineShop.Web.Controllers;
using OnlineShop.Web.Infrastructure.Core;
using OnlineShop.Web.Infrastructure.Extensions;
using OnlineShop.Web.Infrastructure.Helpers;
using OnlineShop.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Web.Areas.Admin.Controllers
{
    public class ManageProductsController : BaseController
    {
        private IProductService _productService;
        private IProductCategoryService _productCategoryService;
        private IManufactureService _manufactureService;
        private IProductCategoryService _categoryService;

        public ManageProductsController(IProductService productService,
            IProductCategoryService categoryService, IManufactureService manufactureService,
            IProductCategoryService productCategoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _manufactureService = manufactureService;
            _productCategoryService = productCategoryService;
        }

        // GET: Admin/ManageProducts
        public ActionResult Index(string keyword = null, int page = 1, string manufactureId = null)
        {
            ViewBag.Filter = keyword;
            int pageSize = PaginationHelper.PageSize();
            int totalRow = 0;
            Guid? newManufactureId = (Guid?)null;
            if (!string.IsNullOrEmpty(manufactureId))
            {
                newManufactureId = new Guid(manufactureId);
            }
            var products = _productService.GetIncludeManufacturePaging(keyword, page, newManufactureId, pageSize, out totalRow);
            var model = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(products);

            var pagination = new PaginationSet<ProductViewModel>()
            {
                TotalCount = totalRow,
                TotalPages = PaginationHelper.TotalPages(totalRow, pageSize),
                Items = model,
                Page = page,
                MaxPage = PaginationHelper.MaxPage()
            };
            return View(pagination);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var model = new ProductViewModel();
            PrepareProductCategoriesModel(model);
            SetViewBagManufacture();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var product = new Product();
                product.UpdateProduct(model);

                product.CreatedBy = User.Identity.Name;
                _productService.Add(product);
                _productService.Save();

                SetAlert("Added " + model.ProductName + " into Products List successfully", "success");
                return RedirectToAction("Index");
            }
            PrepareProductCategoriesModel(model);
            SetViewBagManufacture();
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            var product = _productService.GetById(new Guid(id));
            var model = Mapper.Map<Product, ProductViewModel>(product);
            PrepareProductCategoriesModel(model);
            SetViewBagManufacture();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var product = _productService.GetById(new Guid(id));
                product.UpdateProduct(model, "update");
               
                product.UpdatedBy = User.Identity.Name;
                product.UpdatedDate = DateTime.Now;
                _productService.Update(product);
                _productService.Save();

                SetAlert("Updated " + model.ProductName + " successfully", "success");
                return RedirectToAction("Index");
            }
            PrepareProductCategoriesModel(model);
            SetViewBagManufacture();
            return View(model);
        }

        public JsonResult Delete(string id)
        {
            try
            {
                _productService.Delete(new Guid(id));
                _productService.Save();
                SetAlert("Delete product successfully", "success");
                return Json(new
                {
                    status = true
                });
            }
            catch
            {
                return Json(new
                {
                    status = false
                });
            }
        }

        public void SetViewBagProductCategory(Guid? selectedID = null)
        {
            ViewBag.Categories = new SelectList(_productCategoryService.GetChildByDisplayOrder(), "ID", "Name", selectedID);
        }

        [NonAction]
        protected virtual void PrepareProductCategoriesModel(ProductViewModel model)
        {
            if (model == null)
                throw new ArgumentNullException("model");
            if (model.CategoryList == null)
                model.CategoryList = new List<SelectListItem>();
            model.CategoryList.Add(new SelectListItem
            {
                Text = "Please choose a category",
                Value = ""
            });
            var categories = SelectListHelper.GetCategoryList(_productCategoryService);
            foreach (var c in categories)
                model.CategoryList.Add(c);
        }

        public void SetViewBagManufacture(Guid? selectedID = null)
        {
            ViewBag.Manufactures = new SelectList(_manufactureService.GetAll(), "ManufactureID", "ManufactureName", selectedID);
        }

    }
}