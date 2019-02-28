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
    public interface IApplicationUserService
    {
        AppUser GetUserById(string userId);

        AppUser GetUserByName(string userName);

        IEnumerable<AppUser> GetUserListPaging(int page, int pageSize, string filter, out int totalRow);

        void SetDeleted(string id);
    }

    public class ApplicationUserService : IApplicationUserService
    {
        private IApplicationUserRepository _applicationUserRepository;
        private IUnitOfWork _unitOfWork;

        public ApplicationUserService(IApplicationUserRepository applicationUserRepository,
            IUnitOfWork unitOfWork)
        {
            _applicationUserRepository = applicationUserRepository;
            _unitOfWork = unitOfWork;
        }

        public AppUser GetUserById(string userId)
        {
            try
            {
                return _applicationUserRepository.GetSingleById(userId);
            }
            catch
            {
                throw;
            }
        }

        public AppUser GetUserByName(string userName)
        {
            return _applicationUserRepository.GetSingleById(userName);
        }

        public IEnumerable<AppUser> GetUserListPaging(int page, int pageSize, string filter, out int totalRow)
        {
            var query = _applicationUserRepository.GetMulti(x => x.UserName.Contains(filter) || x.FullName.Contains(filter) && x.IsDeleted == false);
            if (string.IsNullOrEmpty(filter))
                query = _applicationUserRepository.GetMulti(x => x.IsDeleted == false);

            totalRow = query.Count();

            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public void SetDeleted(string id)
        {
            var user = _applicationUserRepository.GetSingleById(id);
            user.IsDeleted = true;
            _applicationUserRepository.Update(user);
            _unitOfWork.Commit();

        }
    }
}
