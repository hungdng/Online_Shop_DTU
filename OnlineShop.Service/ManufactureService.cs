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
    public interface IManufactureService
    {
        Manufacture Add(Manufacture manufacture);

        void Update(Manufacture manufacture);

        Manufacture Delete(Guid Id);

        IEnumerable<Manufacture> GetAll();

        IEnumerable<Manufacture> GetAllPaging(string keyword, int page, int pageSize, out int totalRow);

        Manufacture GetById(Guid Id);

        void SaveChange();

        IEnumerable<Manufacture> GetAllInCategoryID(Guid Id);
    }
    public class ManufactureService : IManufactureService
    {
        private IManufactureRepository _manufactureRepository;
        private IUnitOfWork _unitOfWork;

        public ManufactureService(IManufactureRepository manufactureRepository, IUnitOfWork unitOfWork)
        {
            _manufactureRepository = manufactureRepository;
            _unitOfWork = unitOfWork;
        }

        public Manufacture Add(Manufacture manufacture)
        {
            return _manufactureRepository.Add(manufacture);
        }

        public Manufacture Delete(Guid Id)
        {
            return _manufactureRepository.Delete(Id);
        }

        public IEnumerable<Manufacture> GetAll()
        {
            return _manufactureRepository.GetAll();
        }

        public IEnumerable<Manufacture> GetAllPaging(string keyword, int page, int pageSize, out int totalRow)
        {
            var query = _manufactureRepository.GetAll();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.ManufactureName.Contains(keyword) || x.Description.Contains(keyword));
            }
            totalRow = query.Count();
            return query.Skip((page - 1) * pageSize).Take(pageSize);            
        }

        public IEnumerable<Manufacture> GetAllInCategoryID(Guid Id)
        {
            return _manufactureRepository.GetListManufactureByCategoryId(Id);
        }

        public Manufacture GetById(Guid Id)
        {
            return _manufactureRepository.GetSingleById(Id);
        }

        public void SaveChange()
        {
            _unitOfWork.Commit();
        }

        public void Update(Manufacture manufacture)
        {
            _manufactureRepository.Update(manufacture);
        }
    }
}
