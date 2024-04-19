using Base.Data.Infrastructure.Interfaces;
using Base.Data.Infrastructure.UnitOfWork;
using Base.Data.Models;
using Base.Domain.Interfaces;
using Base.Domain.ViewModels;
using ExcelDataReader;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace Base.Data.Repositories
{
    public interface ITimingPostRepository : IGenericRepository<TimingPost>
    {

    }

    public class TimingRepository : GenericRepository<TimingPost>, ITimingPostRepository
    {
        public TimingRepository(Task01Context context) : base(context)
        {

        }

    }
}
