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
    public interface IOrderDetailService
    {

        OrderDetail Add(OrderDetail order);

        OrderDetail GetOrderDetailById(Guid id);

        IEnumerable<OrderDetail> GetAll();

        IEnumerable<OrderDetail> GetAll(string keyword);

        void Update(OrderDetail order);

        OrderDetail Delete(Guid id);

        void SaveChanges();
    }
    public class OrderDetailService : IOrderDetailService
    {
        IOrderRepository _orderRepository;
        IOrderDetailRepository _orderDetailRepository;
        IUnitOfWork _unitOfWork;

        public OrderDetailService(IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository, IUnitOfWork unitOfWork)
        {
            this._orderRepository = orderRepository;
            this._orderDetailRepository = orderDetailRepository;
            this._unitOfWork = unitOfWork;
        }

        public OrderDetail Add(OrderDetail orderDetail)
        {
            return _orderDetailRepository.Add(orderDetail);
        }

        public OrderDetail Delete(Guid id)
        {
            return _orderDetailRepository.Delete(id);
        }

        public IEnumerable<OrderDetail> GetAll()
        {
            return _orderDetailRepository.GetAll();
        }

        public IEnumerable<OrderDetail> GetAll(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
                return _orderDetailRepository.GetAll();
            else
                return _orderDetailRepository.GetMulti(x => x.Product.ProductName.Contains(keyword));
        }

        public OrderDetail GetOrderDetailById(Guid id)
        {
            return _orderDetailRepository.GetSingleById(id);
        }

        public void SaveChanges()
        {
            _unitOfWork.Commit();
        }

        public void Update(OrderDetail orderDetail)
        {
            _orderDetailRepository.Update(orderDetail);
        }

    }
}
