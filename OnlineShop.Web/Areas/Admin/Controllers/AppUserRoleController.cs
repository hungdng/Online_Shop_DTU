using AutoMapper;
using OnlineShop.Common;
using OnlineShop.Data.Models;
using OnlineShop.Service;
using OnlineShop.Web.App_Start;
using OnlineShop.Web.Controllers;
using OnlineShop.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Web.Areas.Admin.Controllers
{
    public class AppUserRoleController : BaseController
    {
        private ApplicationUserManager _userManager;
        private IApplicationUserService _userService;
        private IApplicationRoleService _roleService;

        public AppUserRoleController(ApplicationUserManager userManager,
            IApplicationUserService userService, IApplicationRoleService roleService)
        {
            _userManager = userManager;
            _userService = userService;
            _roleService = roleService;
        }

        
        public ActionResult Index(string id)
        {
            var user = _userService.GetUserById(id);
            var model = Mapper.Map<AppUser, AppUserViewModel>(user);
            var roles = _roleService.GetAll();
            ViewBag.Roles = roles;

            var userRoles = _userManager.GetRolesAsync(id).Result.ToList();
            model.Roles = userRoles;

            return View(model);
        }

        [HttpPost]
        //[Authorize(Roles = CommonConstants.ADMIN)]
        public async Task<JsonResult> UpdateRole(string role, string id)
        {
            try
            {
                var userRoles = _userManager.GetRolesAsync(id).Result.ToList();

                if (userRoles.Contains(role))
                {
                    var res = await _userManager.RemoveFromRoleAsync(id, role);
                    if (res.Succeeded)
                        SetAlert("Delete Role " + role + " successfully", "warning");
                    else
                        SetAlert("Delete Role " + role + " unsuccessfully", "error");
                }
                else
                {
                    var res = await _userManager.AddToRoleAsync(id, role);
                    if (res.Succeeded)
                        SetAlert("Grant Role " + role + " successfully", "success");
                    else
                        SetAlert("Grant Role " + role + " unsuccessfully", "error");
                }

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
    }
}