using Base.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Service.Constract
{
    public interface IExcelService
    {
        public Task<byte[]> ExportExcel(IEnumerable<DummyCodeVM> dummyCodeVMList);
    }
}
