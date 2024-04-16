using Base.Data.Models;
using Base.Domain.Interfaces;
using Base.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Data.Repositories
{
    public class LogRepository : GenericRepository<LogVM>, ILogRepository
    {
        public LogRepository(Task01Context context) : base(context)
        {
        }
        //public IEnumerable<DummyCode> GetPopularDevelopers(int count)
        //{
        //    return _context.Developers.OrderByDescending(d => d.Followers).Take(count).ToList();
        //}
    }
}
