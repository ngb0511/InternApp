using Base.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Data.Models;
using Base.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Base.Data.Infrastructure.Interfaces;

namespace Base.Data.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Task01Context _context;

        public UnitOfWork(Task01Context context)
        {
            _context = context;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

}
