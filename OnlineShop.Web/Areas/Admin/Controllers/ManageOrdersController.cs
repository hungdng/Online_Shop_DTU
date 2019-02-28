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
    public class ManageOrdersController : BaseController
    {
        private IOrderService _orderService;
        private IOrderStatusService _orderStatusService;

        public ManageOrdersController(IOrderService orderService, IOrderStatusService orderStatusService)
        {
            _orderService = orderService;
            _orderStatusService = orderStatusService;
        }

        // GET: Admin/ManageOrders
        public ActionResult Index(string keyword = null, int page = 1, string orderStatusId = null)
        {
            ViewBag.Filter = keyword;
            int pageSize = PaginationHelper.PageSize();
            int totalRow = 0;
            var orders = _orderService.GetIncludeOrderStatusPaging(keyword, page, orderStatusId, pageSize, out totalRow);
            var model = Mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(orders);

            var pagination = new PaginationSet<OrderViewModel>()
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
            var model = new Order_OrderDetailViewModel();
            SetViewBagOrderStatus();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Order_OrderDetailViewModel model)
        {
            var order = new Order();
            order.UpdateOrder(model.Orders);
            order.CreatedBy = User.Identity.Name;
            order.CreatedDate = DateTime.Now;
            _orderService.Add(order);
            _orderService.SaveChanges();
            SetAlert("Added " + model.Orders.BillCode + " into Order List successfully", "success");
            SetViewBagOrderStatus();
            return Redirect("/Admin/ManageOrders/Edit/"+order.ID);
        }

        [HttpGet]
        public ActionResult Edit(int page = 1, string id = null)
        {
            Order_OrderDetailViewModel o = new Order_OrderDetailViewModel();
            var order = _orderService.GetOrderById(new Guid(id));
            o.Orders = Mapper.Map<Order, OrderViewModel>(order);
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
            SetViewBagOrderStatus();
            return View(o);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, Order_OrderDetailViewModel model)
        {
            if (ModelState.IsValid)
            {
                var order = _orderService.GetOrderById(new Guid(id));
                order.UpdateOrder(model.Orders, "update");
                order.UpdatedBy = User.Identity.Name;
                order.UpdatedDate = DateTime.Now;
                order.Amount = _orderService.GetAmountOfOrder(order.ID);
                _orderService.Update(order);
                _orderService.SaveChanges();

                SetAlert("Updated " + model.Orders.BillCode + " successfully", "success");
                return RedirectToAction("Edit");
            }
            SetViewBagOrderStatus();
            return View(model);
        }

        public void SetViewBagOrderStatus(Guid? selectedID = null)
        {
            ViewBag.OrderStatuses = new SelectList(_orderStatusService.GetAll(), "OrderStatusID", "OrderStatusName", selectedID);
        }
    }
}