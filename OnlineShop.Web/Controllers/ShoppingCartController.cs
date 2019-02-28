using AutoMapper;
using Microsoft.AspNet.Identity;
using OnlineShop.Common;
using OnlineShop.Data.Models;
using OnlineShop.Service;
using OnlineShop.Web.App_Start;
using OnlineShop.Web.Infrastructure.Extensions;
using OnlineShop.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace OnlineShop.Web.Controllers
{
    public class ShoppingCartController : Controller
    {
        IProductService _productService;
        private ApplicationUserManager _userManager;
        IOrderService _orderService;
        public ShoppingCartController(IOrderService orderService, IProductService productService, ApplicationUserManager userManager)
        {
            this._productService = productService;
            this._userManager = userManager;
            this._orderService = orderService;
        }

        // GET: ShoppingCart
        public ActionResult Index()
        {
            if (Session[CommonConstants.SessionCart] == null)
                Session[CommonConstants.SessionCart] = new List<ShoppingCartViewModel>();
            return View();
        }

        public JsonResult GetAll()
        {
            if (Session[CommonConstants.SessionCart] == null)
                Session[CommonConstants.SessionCart] = new List<ShoppingCartViewModel>();

            var cart = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            return Json(new
            {
                status = true,
                data = cart
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CreateOrder(string orderViewModel)
        {
            var order = new JavaScriptSerializer().Deserialize<OrderViewModel>(orderViewModel);
            var orderNew = new Order();
            orderNew.UpdateOrder(order, "add");                     
            if (Request.IsAuthenticated)
            {
                orderNew.CustomerId = User.Identity.GetUserId();
                orderNew.CreatedBy = User.Identity.GetUserName();
            }

            var cart = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            List<OrderDetail> orderDetails = new List<OrderDetail>();
            foreach (var item in cart)
            {
                var detail = new OrderDetail();
                detail.OrderDetailID = Guid.NewGuid();
                detail.OrderID = orderNew.ID;
                detail.ProductID = item.ProductID;
                detail.SaleNumber = item.Quantity;
                detail.SalePrice = item.Price;
                detail.TotalCost = item.Quantity * item.Price;
                orderDetails.Add(detail);
            }

            var orderResult = _orderService.Create(orderNew, orderDetails);
            var model = Mapper.Map<Order, OrderViewModel>(orderResult);

            return Json(new
            {
                status = true,
                data = model
            });
        }

        [HttpPost]
        public JsonResult Add(string productId)
        {
            var cart = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            var product = _productService.GetById(new Guid(productId));

            if (product.Price > 0)
            {
                if (cart == null)
                {
                    cart = new List<ShoppingCartViewModel>();
                }

                if (cart.Any(x => x.ProductID == new Guid(productId)))
                {
                    foreach (var item in cart)
                    {
                        if (item.ProductID == new Guid(productId))
                        {
                            item.Quantity += 1;
                        }
                    }
                }
                else
                {
                    ShoppingCartViewModel newItem = new ShoppingCartViewModel();
                    newItem.ProductID = new Guid(productId);
                    newItem.Product = Mapper.Map<Product, ProductViewModel>(product);
                    newItem.Quantity = 1;
                    newItem.Price = (int) (product.PromotionPrice > 0 ? product.PromotionPrice : product.Price);
                    cart.Add(newItem);
                }
            }
            
            Session[CommonConstants.SessionCart] = cart;
            return Json(new
            {
                status = true
            });

        }

        [HttpPost]
        public JsonResult Update(string cartData)
        {
            var cartViewModel = new JavaScriptSerializer().Deserialize<List<ShoppingCartViewModel>>(cartData);

            var cartSession = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];

            foreach (var item in cartSession)
            {
                foreach (var jitem in cartViewModel)
                {
                    if (item.ProductID == jitem.ProductID)
                    {
                        item.Quantity = jitem.Quantity;
                        item.Price = jitem.Price;
                    }
                }
            }

            Session[CommonConstants.SessionCart] = cartSession;

            return Json(new
            {
                status = true
            });
        }

        [HttpPost]
        public JsonResult DeleteItem(string productId)
        {
            var cartSession = (List<ShoppingCartViewModel>)Session[CommonConstants.SessionCart];
            if(cartSession != null)
            {
                cartSession.RemoveAll(x => x.ProductID == new Guid(productId));
                Session[CommonConstants.SessionCart] = cartSession;
                return Json(new
                {
                    status = true
                });
            }

            return Json(new
            {
                status = false
            });
        }

        public JsonResult DeleteAll()
        {
            Session[CommonConstants.SessionCart] = new List<ShoppingCartViewModel>();
            return Json(new
            {
                status = true
            });
        }

        public JsonResult GetUser()
        {
            if (Request.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var user = _userManager.FindById(userId);
                return Json(new
                {
                    data = user,
                    status = true
                });
            }
            return Json(new
            {
                status = false
            });
        }
    }
}