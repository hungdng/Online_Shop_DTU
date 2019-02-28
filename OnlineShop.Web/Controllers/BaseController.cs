using Microsoft.AspNet.Identity.Owin;
using OnlineShop.Common;
using OnlineShop.Web.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Web.Controllers
{
    //[Authorize(Roles = CommonConstants.ADMIN)]
    public class BaseController : Controller
    {
        protected void SetAlert(string message, string type)
        {
            TempData["AlertMessage"] = message;
            if (type == "success")
            {
                TempData["AlertType"] = "alert-success";
            }
            else
                if (type == "warning")
            {
                TempData["AlertType"] = "alert-warning";
            }
            else
                    if (type == "error")
            {
                TempData["AlertType"] = "alert-error";
            }
        }
    }
}