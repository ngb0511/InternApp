using Base.Domain.RequestModels;
using Base.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Interfaces
{
    public interface IDummyCodeRepository : IGenericRepository<DummyCodeVM>, IExcelRepository<DummyCodeVM>
    {
        public IEnumerable<DummyCodeVM> GetResultModel(int pageSize, int CurrentPage);

        public bool CheckDummyCodeById(int id);

        public bool CheckDummyCodeExisted(DummyCodeVM dummyCodeVM);

        public IEnumerable<DummyCodeVM> GetDummyCodeFromExcel(string fileName, int userId);
    }

}
