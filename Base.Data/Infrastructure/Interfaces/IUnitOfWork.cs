﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Domain.Interfaces;

namespace Base.Data.Infrastructure.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public Task SaveChangesAsync();

        public void SaveChanges();
    }

}
