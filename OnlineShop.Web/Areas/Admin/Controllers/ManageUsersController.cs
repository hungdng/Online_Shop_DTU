using AutoMapper;
using OnlineShop.Common;
using OnlineShop.Common.Exceptions;
using OnlineShop.Data.Models;
using OnlineShop.Service;
using OnlineShop.Web.App_Start;
using OnlineShop.Web.Controllers;
using OnlineShop.Web.Infrastructure.Core;
using OnlineShop.Web.Infrastructure.Extensions;
using OnlineShop.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Web.Areas.Admin.Controllers
{
    public class ManageUsersController : BaseController
    {
        #region ctor

        private ICommonService _commonService;
        private IApplicationUserService _appUser;
        private ApplicationUserManager _userManager;
        private IApplicationRoleService _appRoleService;

        public ManageUsersController(
            IApplicationUserService appUser,
            IApplicationRoleService appRoleService,
            ApplicationUserManager userManager,
            ICommonService commonService)
        {
            _appRoleService = appRoleService;
            _appUser = appUser;
            _userManager = userManager;
            _commonService = commonService;
        }

        #endregion ctor

        // GET: Admine/AppUser
        public ActionResult Index(string filter = null, int page = 1)
        {
            ViewBag.Filter = filter;
            int pageSize = PaginationHelper.PageSize();
            int totalRow = 0;
            var users = _appUser.GetUserListPaging(page, pageSize, filter, out totalRow);
            var model = Mapper.Map<IEnumerable<AppUser>, IEnumerable<AppUserViewModel>>(users);

            var pagination = new PaginationSet<AppUserViewModel>()
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
        //[Authorize(Roles = CommonConstants.ADMIN)]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        //[Authorize(Roles = CommonConstants.ADMIN)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AppUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.UserName = model.Email;
                var userByEmail = await _userManager.FindByEmailAsync(model.Email);
                if (userByEmail != null)
                {
                    ModelState.AddModelError("email", "Email already exist");
                    return View(model);
                }
                var userByUserName = await _userManager.FindByNameAsync(model.UserName);
                if (userByUserName != null)
                {
                    ModelState.AddModelError("username", "Username already exist");
                    return View(model);
                }
                var user = new AppUser();

                user.UpdateUser(model);
                user.IsDeleted = false;
                user.CreatedDate = DateTime.Now;
                user.EmailConfirmed = true;
                user.PhoneNumberConfirmed = true;
                user.TwoFactorEnabled = false;
                user.LockoutEnabled = false;
                user.AccessFailedCount = 0;
                try
                {
                    user.Id = Guid.NewGuid().ToString();
                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        var userModel = await _userManager.FindByEmailAsync(model.Email);
                        if (userModel != null)
                        {
                            await _userManager.AddToRolesAsync(userModel.Id, new string[] { CommonConstants.MEMBER });
                        }
                        SetAlert("Added " + model.FullName + " succcessfully", "success");
                        Thread.Sleep(2000);
                        return RedirectToAction("Index");
                    }
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    return View(model);
                }

            }
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = CommonConstants.ADMIN)]
        public async Task<ActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var userModel = Mapper.Map<AppUser, AppUserViewModel>(user);
            return View(userModel);
        }
        [HttpPost]
        [Authorize(Roles = CommonConstants.ADMIN)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(AppUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var appUser = await _userManager.FindByIdAsync(model.Id);
                try
                {
                    appUser.UpdateUser(model);
                    var result = await _userManager.UpdateAsync(appUser);
                    if (result.Succeeded)
                    {
                        SetAlert("Updated " + model.FullName + " successfully", "success");
                        Thread.Sleep(2000);
                        return RedirectToAction("Index");
                    }
                    else
                        return View(model);
                }
                catch (NameDuplicatedException dex)
                {
                    ModelState.AddModelError("", dex.Message);
                    return View(model);
                }
            }
            ModelState.AddModelError("", "Updated unsuccessfully. Please try again!");
            return View(model);
        }
        [Authorize(Roles = CommonConstants.ADMIN)]
        public async Task<JsonResult> Delete(string id)
        {
            if (ModelState.IsValid)
            {
                var appUser = await _userManager.FindByIdAsync(id);
                appUser.IsDeleted = true;
                var result = await _userManager.UpdateAsync(appUser);
                if (result.Succeeded)
                {
                    SetAlert("Deleted " + appUser.FullName + " successfully", "success");
                    Thread.Sleep(2000);
                    return Json(new
                    {
                        status = true
                    });
                }
            }
            return Json(new
            {
                status = false
            });
        }
    }
}