using Base.Data.Models;
using Base.Domain.Interfaces;
using Base.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Base.Service.Constract
{
    public interface IDummyCodeService : IExcelService
    {
        public Task<bool> AddDummyCode(DummyCodeVM dummyCodeVM);

        public Task<IEnumerable<DummyCodeVM>> AddRangeDummyCode(IEnumerable<DummyCodeVM> dummyCodeVMs);

        public Task<IEnumerable<DummyCodeVM>> RemoveRangeDummyCode(IEnumerable<DummyCodeVM> dummyCodeVMs);

        public IEnumerable<DummyCodeVM> GetAllDummyCode();

        public IEnumerable<DummyCodeVM> GetResultModel(int pageSize, int CurrentPage);

        public bool CheckDummyCodeById(int id);

        public bool CheckDummyCodeExisted(DummyCodeVM dummyCodeVM);

        public DummyCodeVM GetDummyCodeById(int id);

        public IEnumerable<DummyCodeVM> FindDummyCode(Expression<Func<DummyCodeVM, bool>> expression);

        public Task<bool> UpdateDummyCode(DummyCodeVM dummyCodeVM);

        public Task<bool> RemoveDummyCode(DummyCodeVM dummyCodeVM);

        public IEnumerable<DummyCodeVM> GetDummyCodeFromExcel(string fileName, int userId);
    }
}
