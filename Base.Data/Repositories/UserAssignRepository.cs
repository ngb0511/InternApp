using Base.Data.Models;
using Base.Domain.Interfaces;
using Base.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Base.Data.Repositories
{
    public interface IUserAssignRepository
    {
        public bool IsExistedUserName(string userName);
        public UserAssign GetUserById(int id);
    }
    public class UserAssignRepository : GenericRepository<UserAssignVM>, IUserAssignRepository
    {
        public UserAssignRepository(Task01Context context) : base(context)
        {

        }

        public bool IsExistedUserName(string userName)
        {
            var check = _context.UserAssigns.FirstOrDefault(ua => ua.UserName == userName);
            if (check != null)
            {
                return true;
            }
            return false;
        }

        public UserAssign GetUserById(int id)
        {
            var user = _context.UserAssigns.Find(id);
            return user;
        }

    }
}
