using Base.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Data.Models;
using Base.Data.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;

namespace Base.Data.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Task01Context _context;


        public IMaterialMaster MaterialMaster { get; private set; }


        public UnitOfWork(Task01Context context)
        {
            _context = context;
            MaterialMaster = new MaterialMasterRepository(_context);
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
