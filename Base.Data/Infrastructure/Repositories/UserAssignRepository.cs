using Base.Data.Models;
using Base.Domain.Interfaces;
using Base.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Base.Data.Infrastructure.Repositories
{
    public class UserAssignRepository : GenericRepository<UserAssignVM>,IUserAssignRepository
    {
        public UserAssignRepository(Task01Context context) : base(context)
        {

        }

        public bool IsExistedUserName(string userName)
        {
            var check = _context.UserAssigns.FirstOrDefault(ua=>ua.UserName == userName);
            if(check != null)
            {
                return true;
            }
            return false;
        }

        public void Add(UserAssign entity)
        {           
        }

        public void AddRange(IEnumerable<UserAssign> entities)
        {
        }

        public IEnumerable<UserAssign> Find(Expression<Func<UserAssign, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserAssign> GetAll()
        {
            throw new NotImplementedException();
        }

        public UserAssign GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<UserAssign> entities)
        {
            throw new NotImplementedException();
        }

        public void Update(UserAssign entity, int id)
        {
            throw new NotImplementedException();
        }
    }
}
