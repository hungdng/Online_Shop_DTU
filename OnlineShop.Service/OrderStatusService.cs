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
    public interface IOrderStatusService
    {
        IEnumerable<OrderStatus> GetAll();

        IEnumerable<OrderStatus> GetAllPaging(string keyword, int page, int pageSize, out int totalRow);

        OrderStatus GetById(Guid Id);
    }
    public class OrderStatusService : IOrderStatusService
    {
        private IOrderStatusRepository _orderStatusRepository;
        private IUnitOfWork _unitOfWork;

        public OrderStatusService(IOrderStatusRepository orderStatusRepository, IUnitOfWork unitOfWork)
        {
            _orderStatusRepository = orderStatusRepository;
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<OrderStatus> GetAll()
        {
            return _orderStatusRepository.GetAll();
        }

        public IEnumerable<OrderStatus> GetAllPaging(string keyword, int page, int pageSize, out int totalRow)
        {
            var query = _orderStatusRepository.GetAll();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.OrderStatusName.Contains(keyword) || x.Description.Contains(keyword));
            }
            totalRow = query.Count();
            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public OrderStatus GetById(Guid Id)
        {
            return _orderStatusRepository.GetSingleById(Id);
        }
    }
}
