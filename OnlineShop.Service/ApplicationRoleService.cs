using OnlineShop.Common.Exceptions;
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
    public interface IApplicationRoleService
    {
        IEnumerable<AppRole> GetAll(string filter = null);

        AppRole Add(AppRole appRole);

        void Update(AppRole AppRole);

        void Delete(string id);

        AppRole FindById(string id);

        void Save();
    }

    public class ApplicationRoleService : IApplicationRoleService
    {
        private IApplicationRoleRepository _appRole;
        private IUnitOfWork _unitOfWork;

        public ApplicationRoleService(IUnitOfWork unitOfWork,
            IApplicationRoleRepository appRole)
        {
            this._appRole = appRole;
            this._unitOfWork = unitOfWork;
        }

        public AppRole Add(AppRole appRole)
        {
            appRole.IsDeleted = false;
            if (_appRole.CheckContains(x => x.Description == appRole.Description))
                throw new NameDuplicatedException("Tên không được trùng");
            return _appRole.Add(appRole);
        }

        public void Delete(string id)
        {
            var role = _appRole.GetSingleById(id);
            role.IsDeleted = true;
            Save();
        }

        public AppRole FindById(string id)
        {
            return _appRole.GetSingleById(id);
        }

        public IEnumerable<AppRole> GetAll(string filter = null)
        {
            if (!string.IsNullOrEmpty(filter))
                return _appRole.GetMulti(x => x.Name.Contains(filter) || x.Description.Contains(filter) && x.IsDeleted == false);
            return _appRole.GetMulti(x => x.IsDeleted == false || x.IsDeleted == null);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(AppRole AppRole)
        {
            if (_appRole.CheckContains(x => x.Description == AppRole.Description && x.Id != AppRole.Id))
                throw new NameDuplicatedException("Tên không được trùng");
            _appRole.Update(AppRole);
        }
    }
}
