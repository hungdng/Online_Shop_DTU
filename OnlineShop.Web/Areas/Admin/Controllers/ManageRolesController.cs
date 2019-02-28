using AutoMapper;
using OnlineShop.Common;
using OnlineShop.Data.Models;
using OnlineShop.Service;
using OnlineShop.Web.Controllers;
using OnlineShop.Web.Infrastructure.Extensions;
using OnlineShop.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Web.Areas.Admin.Controllers
{
    public class ManageRolesController : BaseController
    {
        private IApplicationRoleService _roleService;

        public ManageRolesController(IApplicationRoleService roleService)
        {
            _roleService = roleService;
        }

        // GET: Admin/ManageRoles
        public ActionResult Index(string filter = null)
        {
            ViewBag.Filter = filter;

            var roles = _roleService.GetAll(filter);

            var model = Mapper.Map<IEnumerable<AppRole>, IEnumerable<AppRoleViewModel>>(roles);

            return View(model);
        }

        [HttpGet]
        //[Authorize(Roles = CommonConstants.ADMIN)]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        //[Authorize(Roles = CommonConstants.ADMIN)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AppRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = new AppRole();
                role.UpdateAppRole(model);
                _roleService.Add(role);
                _roleService.Save();
                SetAlert("Added successfully", "success");

                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        //[Authorize(Roles = CommonConstants.ADMIN)]
        public ActionResult Edit(string id)
        {
            if (ModelState.IsValid)
            {
                var role = _roleService.FindById(id);
                var model = Mapper.Map<AppRole, AppRoleViewModel>(role);
                return View(model);
            }
            return View();
        }

        [HttpPost]
        //[Authorize(Roles = CommonConstants.ADMIN)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AppRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = _roleService.FindById(model.Id);
                role.UpdateAppRole(model, "update");
                _roleService.Update(role);
                _roleService.Save();

                SetAlert("Updated succeessfully", "success");
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Updated unsuccessfully. Please try again!");
            return View(model);
        }

        //[Authorize(Roles = CommonConstants.ADMIN)]
        public JsonResult Delete(string id)
        {
            if (ModelState.IsValid)
            {
                _roleService.Delete(id);
                SetAlert("Delete successfully", "success");
                return Json(new
                {
                    status = true
                });
            }
            SetAlert("Delete unsuccessfully", "error");
            return Json(new
            {
                status = false
            });
        }
    }
}