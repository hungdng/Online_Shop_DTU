using OnlineShop.Data.Infrastructure;
using OnlineShop.Data.Models;
using OnlineShop.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Service
{
    public interface IOrderService
    {
        Order Create(Order order, List<OrderDetail> orderDetails);

        Order Add(Order order);

        Order GetOrderById(Guid id);

        IEnumerable<Order> GetAll();

        IEnumerable<Order> GetAll(string keyword);

        IEnumerable<Order> GetIncludeOrderStatusPaging(string keyword, int page, string orderStatusId, int pageSize, out int totalRow);

        IEnumerable<OrderDetail> GetOrderDetailByOrderID(int page, Guid id, int pageSize, out int totalRow);

        void Update(Order order);

        Order Delete(Guid id);

        void SaveChanges();

        decimal GetAmountOfOrder(Guid id);
    }
    public class OrderService : IOrderService
    {
        IOrderRepository _orderRepository;
        IOrderDetailRepository _orderDetailRepository;
        IUnitOfWork _unitOfWork;

        public OrderService(IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository, IUnitOfWork unitOfWork)
        {
            this._orderRepository = orderRepository;
            this._orderDetailRepository = orderDetailRepository;
            this._unitOfWork = unitOfWork;
        }

        public Order Add(Order order)
        {
            return _orderRepository.Add(order);
        }

        public Order Create(Order order, List<OrderDetail> orderDetails)
        {
            try
            {
                var orderReturn =  _orderRepository.Add(order);
                _unitOfWork.Commit();

                foreach (var orderDetail in orderDetails)
                {
                    orderDetail.OrderID = order.ID;
                    _orderDetailRepository.Add(orderDetail);
                }
                _unitOfWork.Commit();
                return orderReturn;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Order Delete(Guid id)
        {
            return _orderRepository.Delete(id);
        }

        public IEnumerable<Order> GetAll()
        {
            return _orderRepository.GetAll();
        }

        public IEnumerable<Order> GetAll(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
                return _orderRepository.GetAll();
            else
                return _orderRepository.GetMulti(x => x.CustomerName.Contains(keyword));
        }

        public Order GetOrderById(Guid id)
        {
            return _orderRepository.GetSingleById(id);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void Update(Order order)
        {
            _orderRepository.Update(order);
        }

        public IEnumerable<Order> GetIncludeOrderStatusPaging(string keyword, int page, string orderStatusId, int pageSize, out int totalRow)
        {
            var query = _orderRepository.GetAll();
            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.ToLower();
                query = _orderRepository.GetMulti(x => x.CustomerName.ToLower().Contains(keyword)
                    || x.OrderStatusID.ToLower().Contains(keyword) || x.BillCode.ToLower().Contains(keyword));
            }
            if (orderStatusId != null)
            {
                query = _orderRepository.GetMulti(x => x.OrderStatusID == orderStatusId);
            }
            totalRow = query.Count();
            return query.OrderByDescending(x => x.CreatedDate).Skip((page - 1) * pageSize).Take(pageSize);
        }

        public IEnumerable<OrderDetail> GetOrderDetailByOrderID(int page, Guid id, int pageSize, out int totalRow)
        {
            var query = _orderDetailRepository.GetMulti(x => x.OrderID == id, new string[] { "Product" });
            totalRow = query.Count();
            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public decimal GetAmountOfOrder(Guid id)
        {
            return _orderDetailRepository.GetMulti(x => x.OrderID == id).Sum(s => s.TotalCost);
        }
    }
}
