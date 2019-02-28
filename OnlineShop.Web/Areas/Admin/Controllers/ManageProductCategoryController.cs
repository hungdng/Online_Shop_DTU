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
    public class ManageProductCategoryController : BaseController
    {
        private IProductCategoryService _productCategoryService;

        public ManageProductCategoryController(IProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;
        }
        // GET: Admin/ManageProductCategory
        public ActionResult Index(string keyword = null, int page = 1)
        {
            ViewBag.Filter = keyword;
            int pageSize = PaginationHelper.PageSize();
            int totalRow = 0;
            var productCategory = _productCategoryService.GetAllPaging(keyword, page, pageSize, out totalRow);
            var model = Mapper.Map<IEnumerable<ProductCategory>, IEnumerable<ProductCategoryViewModel>>(productCategory);

            var pagination = new PaginationSet<ProductCategoryViewModel>()
            {
                TotalCount = totalRow,
                TotalPages = PaginationHelper.TotalPages(totalRow, pageSize),
                Items = model,
                Page = page,
                MaxPage = PaginationHelper.MaxPage()
            };
            return View(pagination);
        }

        // GET: Admin/ManageProductCategory/Create
        public ActionResult Create()
        {
            var model = new ProductCategoryViewModel();
            SetViewBag();
            return View(model);
        }

        // POST: Admin/ManageProductCategory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductCategoryViewModel model)
        {
            try
            {
                var productCategory = new ProductCategory();
                productCategory.UpdateProductCategory(model);
                productCategory.ID = Guid.NewGuid();
                productCategory.CreatedBy = User.Identity.Name;

                _productCategoryService.Add(productCategory);
                _productCategoryService.SaveChanges();

                SetAlert("Added " + model.Name + " into Product Categories List succcessfully", "success");

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("create-category-err", ex.Message);
                return View(model);
            }
        }

        // GET: Admin/ManageProductCategory/Edit/5
        public ActionResult Edit(string id)
        {
            var productCategory = _productCategoryService.GetByID(new Guid(id));
            var model = Mapper.Map<ProductCategory, ProductCategoryViewModel>(productCategory);
            SetViewBag();
            return View(model);
        }

        // POST: Admin/ManageProductCategory/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, ProductCategoryViewModel model)
        {
            try
            {
                var productCategory = _productCategoryService.GetByID(new Guid(id));

                productCategory.UpdateProductCategory(model);
                productCategory.UpdatedBy = User.Identity.Name;

                _productCategoryService.Update(productCategory);
                _productCategoryService.SaveChanges();
                SetAlert("Updated " + model.Name + " successfully", "success");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("edit-category-err", ex.Message);
                return View(model);
            }
        }

        [HttpPost]
        public JsonResult Delete(string id)
        {
            try
            {
                _productCategoryService.Delete(new Guid(id));
                _productCategoryService.SaveChanges();
                SetAlert("Delete successfully", "success");
                return Json(new
                {
                    status = true
                });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("delete-category-err", ex.Message);
                return Json(new
                {
                    status = false
                });
            }
        }

        #region Helpers

        [NonAction]
        protected virtual void PrepareProductCategoriesModel(ProductCategoryViewModel model)
        {
            if (model == null)
                throw new ArgumentNullException("model");
            if (model.ParentList == null)
                model.ParentList = new List<SelectListItem>();
            model.ParentList.Add(new SelectListItem
            {
                Text = "Chưa chọn danh mục",
                Value = ""
            });
            var categories = SelectListHelper.GetCategoryList(_productCategoryService);
            foreach (var c in categories)
                model.ParentList.Add(c);
        }

        public void SetViewBag(Guid? selectedID = null)
        {
            ViewBag.ParentID = new SelectList(_productCategoryService.GetParentByDisplayOrder(), "ID", "Name", selectedID);
        }

        #endregion Helpers
    }
}