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
    public interface ICommonService
    {
       
        IEnumerable<AppUser> GetUsers(string filter);
    }

    public class CommonService : ICommonService
    {
      
        private IUnitOfWork _unitOfWork;
        private IApplicationUserRepository _applicationUserRepository;

        
        public IEnumerable<AppUser> GetUsers(string filter)
        {
            if (!string.IsNullOrEmpty(filter))
            {
                return _applicationUserRepository.GetMulti(x => x.FullName.Contains(filter));
            }
            else
            {
                return _applicationUserRepository.GetAll();
            }
        }
    }
}
