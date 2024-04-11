using Base.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Interfaces
{
    public interface IDummyCodeRepository : IGenericRepository<DummyCodeVM>
    {
        public IEnumerable<DummyCodeVM> GetAll();

        public void Add(DummyCodeVM dummyCodeVM);

        public void AddRange(IEnumerable<DummyCodeVM> dummyCodeVMs);

        public DummyCodeVM GetById(int id);

        public bool CheckDummyCodeById(int id);

        public bool CheckDummyCodeExisted(DummyCodeVM dummyCodeVM);

        public void UpdateDummyCode(DummyCodeVM dummyCodeVM);

        public void DeleteDummyCode(int id);

        public IEnumerable<DummyCodeVM> GetDummyCodeFromExcel(string fileName, int userId);

        public Task<byte[]> ExportExcel(IEnumerable<DummyCodeVM> dummyCodeVMList);
    }

}
