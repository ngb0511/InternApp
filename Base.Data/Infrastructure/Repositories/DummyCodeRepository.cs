using Base.Data.Models;
using Base.Domain.Interfaces;
using Base.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Domain;

namespace Base.Data.Infrastructure.Repositories
{
    public class DummyCodeRepository : GenericRepository<DummyCodeVM>, IDummyCodeRepository
    {
        public DummyCodeRepository(Task01Context context) : base(context)
        {
        }
        //public IEnumerable<DummyCode> GetPopularDevelopers(int count)
        //{
        //    return _context.Developers.OrderByDescending(d => d.Followers).Take(count).ToList();
        //}
    }

}
