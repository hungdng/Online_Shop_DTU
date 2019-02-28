using AutoMapper;
using OnlineShop.Common;
using OnlineShop.Data.Models;
using OnlineShop.Service;
using OnlineShop.Web.Controllers;
using OnlineShop.Web.Infrastructure.Core;
using OnlineShop.Web.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace OnlineShop.Web.Areas.Admin.Controllers
{
    public class ManageOrderDetailsController : BaseController
    {
        private IProductService _productService;
        private IOrderService _orderService;
        private IOrderDetailService _orderDetailService;

        public ManageOrderDetailsController(IProductService productService,
            IOrderService orderService, IOrderDetailService orderDetailService)
        {
            _productService = productService;
            _orderService = orderService;
            _orderDetailService = orderDetailService;
        }

        // GET: Admin/ManageOrderDetails
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetDetail(string id = null)
        {
            var model = _orderDetailService.GetOrderDetailById(new Guid(id));
            return Json(new
            {
                data = model,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(string id = null)
        {
            var orderid = _orderDetailService.GetOrderDetailById(new Guid(id)).OrderID;
            _orderDetailService.Delete(new Guid(id));
            _orderDetailService.SaveChanges();
            OrderViewModel model = null;
            Order order = _orderService.GetOrderById(orderid);
            order.Amount = _orderService.GetAmountOfOrder(orderid);
            order.TotalCost = order.Amount - order.Discount;
            _orderService.Update(order);
            _orderService.SaveChanges();
            model = Mapper.Map<Order, OrderViewModel>(order);
            try
            {
                return Json(new
                {
                    status = true,
                    data = model
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = false
                });
            }
        }

        [HttpPost]
        public JsonResult SaveData(string strEmployee)
        {
            JavaScriptSerializer seralizer = new JavaScriptSerializer();
            var orderDetail = seralizer.Deserialize<OrderDetailViewModel>(strEmployee);
            //add new if employee id = 0
            var entity = _orderDetailService.GetOrderDetailById(orderDetail.OrderDetailID);
            entity.SaleNumber = orderDetail.SaleNumber;
            entity.SalePrice = orderDetail.SalePrice;
            entity.ProductSpec = orderDetail.ProductSpec;
            entity.TotalCost = orderDetail.SalePrice * orderDetail.SaleNumber;
            _orderDetailService.Update(entity);
            OrderViewModel model = null;
            try
            {
                _orderDetailService.SaveChanges();
                Order order = _orderService.GetOrderById(entity.OrderID);
                order.Amount = _orderService.GetAmountOfOrder(entity.OrderID);
                order.TotalCost = order.Amount - order.Discount;
                _orderService.Update(order);
                _orderService.SaveChanges();
                model = Mapper.Map<Order, OrderViewModel>(order);
            }
            catch (Exception ex)
            {

            }

            return Json(new
            {
                status = true,
                data = model
            });
        }

        [HttpPost]
        public JsonResult CreateData(string strEmployee)
        {
            JavaScriptSerializer seralizer = new JavaScriptSerializer();
            OrderDetail orderDetail = new OrderDetail();
            var entity = seralizer.Deserialize<OrderDetailViewModel>(strEmployee);
            Order order = _orderService.GetOrderById(entity.OrderID);
            if (order != null)
            {
                orderDetail.OrderDetailID = Guid.NewGuid();
                orderDetail.ProductID = entity.ProductID;
                orderDetail.SaleNumber = entity.SaleNumber;
                orderDetail.SalePrice = entity.SalePrice;
                orderDetail.OrderID = entity.OrderID;
                orderDetail.ProductSpec = entity.ProductSpec;
                orderDetail.TotalCost = orderDetail.SalePrice * orderDetail.SaleNumber;
                _orderDetailService.Add(orderDetail);

                OrderViewModel model = null;
                try
                {
                    _orderDetailService.SaveChanges();
                    order.Amount = _orderService.GetAmountOfOrder(orderDetail.OrderID);
                    order.TotalCost = order.Amount - order.Discount;
                    _orderService.Update(order);
                    _orderService.SaveChanges();

                    model = Mapper.Map<Order, OrderViewModel>(order);
                }
                catch (Exception ex)
                {

                }

                return Json(new
                {
                    status = true,
                    data = model
                });
            }
            else
            {
                return Json(new
                {
                    status = false
                });
            }
        }

        [HttpGet]
        public JsonResult LoadData(string id = null)
        {
            int page = 1;
            Order_OrderDetailViewModel o = new Order_OrderDetailViewModel();
            int pageSize = PaginationHelper.PageSize();
            int totalRow = 0;

            var orderdetails = _orderService.GetOrderDetailByOrderID(page, new Guid(id), pageSize, out totalRow);

            var model = Mapper.Map<IEnumerable<OrderDetail>, IEnumerable<OrderDetailViewModel>>(orderdetails);

            var pagination = new PaginationSet<OrderDetailViewModel>()
            {
                TotalCount = totalRow,
                TotalPages = PaginationHelper.TotalPages(totalRow, pageSize),
                Items = model,
                Page = page,
                MaxPage = PaginationHelper.MaxPage()
            };
            o.OrderDetails = pagination;
            return Json(new
            {
                data = pagination,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProductJson(string productName)
        {
            var product = _productService.GetListProductJsonByName(productName);
            return Json(new
            {
                data = product
            }, JsonRequestBehavior.AllowGet);
        }
    }
}