using Base.Data.Infrastructure.Interfaces;
using Base.Data.Infrastructure.UnitOfWork;
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
    public interface IUserAssignRepository : IGenericRepository<UserAssign>
    {

    }
    public class UserAssignRepository : GenericRepository<UserAssign>, IUserAssignRepository
    {
        public UserAssignRepository(Task01Context context) : base(context)
        {

        }

    }
}
