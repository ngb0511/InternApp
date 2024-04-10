﻿using Base.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Interfaces
{
    public interface ILogRepository : IGenericRepository<LogVM>
    {
        //IEnumerable<LogVM> GetPopularDevelopers(int count);
    }
}
