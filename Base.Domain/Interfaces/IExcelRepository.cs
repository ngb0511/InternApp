using Base.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Interfaces
{
    public interface IExcelRepository<T> where T : class
    {
        public Task<byte[]> ExportExcel(IEnumerable<T> entities);
    }
}
