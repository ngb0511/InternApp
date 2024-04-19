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
        UserAssign GetUserById(int id);
        bool IsExistedUserName(string username);
        string GetUserFullName(int id);
    }
}
