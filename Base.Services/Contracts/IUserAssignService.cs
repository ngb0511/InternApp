using Base.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Service.Contracts
{
    public interface IUserAssignService
    {
        Task<UserAssign?> GetUserById(int id);

        Task<bool> IsExistedUserName(string username);
    }
}
