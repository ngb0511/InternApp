using Base.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Data.Models;
using Base.Data.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Base.Data.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Task01Context _context;
        public IDummyCodeRepository DummyCodes { get; private set; }
        public ILogRepository Logs { get; private set; }


        public UnitOfWork(Task01Context context)
        {
            _context = context;
            DummyCodes = new DummyCodeRepository(_context);
            Logs = new LogRepository(_context);
        }

        public int Complete()
        {

            return _context.SaveChanges();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }

}
