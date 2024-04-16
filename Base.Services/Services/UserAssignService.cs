using Base.Data.Models;
using Base.Data.Repositories;
using Base.Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Service.Services
{
    public class UserAssignService : IUserAssignService
    {
        private readonly IUserAssignRepository _userAssignRepository;

        public UserAssignService(IUserAssignRepository userAssignRepository)
        {
            _userAssignRepository = userAssignRepository;
        }
        public async Task<UserAssign?> GetUserById(int id)
        {
            return _userAssignRepository.GetUserById(id);
        }

        public async Task<bool> IsExistedUserName(string username)
        {
            return _userAssignRepository.IsExistedUserName(username);
        }
    }
}
