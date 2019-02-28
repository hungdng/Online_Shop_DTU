using AutoMapper;
using OnlineShop.Common;
using OnlineShop.Data.Models;
using OnlineShop.Service;
using OnlineShop.Web.Controllers;
using OnlineShop.Web.Infrastructure.Core;
using OnlineShop.Web.Infrastructure.Extensions;
using OnlineShop.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Web.Areas.Admin.Controllers
{
    public class ManufactureController : BaseController
    {
        private IManufactureService _manufactureService;

        public ManufactureController(IManufactureService manufactureService)
        {
            _manufactureService = manufactureService;
        }
        // GET: Admin/Manufacture
        public ActionResult Index(string keyword = null, int page = 1)
        {
            ViewBag.Filter = keyword;
            int pageSize = PaginationHelper.PageSize();
            int totalRow = 0;
            var manufacture = _manufactureService.GetAllPaging(keyword, page, pageSize, out totalRow);
            var model = Mapper.Map<IEnumerable<Manufacture>, IEnumerable<ManufactureViewModel>>(manufacture);

            var pagination = new PaginationSet<ManufactureViewModel>()
            {
                TotalCount = totalRow,
                TotalPages = PaginationHelper.TotalPages(totalRow, pageSize),
                Items = model,
                Page = page,
                MaxPage = PaginationHelper.MaxPage()
            };
            return View(pagination);
        }

        // GET: Admin/Manufacture/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Manufacture/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ManufactureViewModel model)
        {
            try
            {
                var manufacture = new Manufacture();                
                manufacture.UpdateManufacture(model);
                manufacture.ManufactureID = Guid.NewGuid();
                _manufactureService.Add(manufacture);
                _manufactureService.SaveChange();

                SetAlert("AAdded " + model.ManufactureName + " into Manufactures List successfully", "success");

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("create-brand-err", ex.Message);
                return View(model);
            }
        }

        // GET: Admin/Manufacture/Edit/5
        public ActionResult Edit(string id)
        {
            var manufacture = _manufactureService.GetById(new Guid(id));
            var model = Mapper.Map<Manufacture, ManufactureViewModel>(manufacture);
            return View(model);
        }

        // POST: Admin/Manufacture/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, ManufactureViewModel model)
        {
            try
            {
                var manufacture = _manufactureService.GetById(new Guid(id));

                manufacture.UpdateManufacture(model);
                manufacture.ManufactureID = new Guid(id);
                _manufactureService.Update(manufacture);
                _manufactureService.SaveChange();
                SetAlert("Updated " + model.ManufactureName + " successfully", "success");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("edit-brand-err", ex.Message);
                return View(model);
            }
        }

        [HttpPost]
        public JsonResult Delete(string id)
        {
            try
            {
                _manufactureService.Delete(new Guid(id));
                _manufactureService.SaveChange();
                SetAlert("Deleted successfully", "success");
                return Json(new
                {
                    status = true
                });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("delete-brand-err", ex.Message);
                return Json(new
                {
                    status = false
                });
            }
        }
    }
}