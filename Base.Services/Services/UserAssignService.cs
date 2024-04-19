using Base.Data.Models;
using Base.Data.Repositories;
using Base.Service.Contracts;
using Microsoft.EntityFrameworkCore;
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
        public UserAssign GetUserById(int id)
        {
            var user = _userAssignRepository.GetById(id);
            return user;
        }

        public bool IsExistedUserName(string username)
        {
            var user = _userAssignRepository.Find(x=>x.UserName == username).Any();
            if(user)
            {
                return true;
            }

            return false; 
        }

        public string GetUserFullName(int id)
        {
            var user = _userAssignRepository.GetById(id);
            return user.UserFullName;
        }

    }
}
