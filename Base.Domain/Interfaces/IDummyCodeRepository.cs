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
        public IEnumerable<DummyCodeVM> GetAllDummyCode();

        public void AddDummyCode(DummyCodeVM dummyCodeVM);

        public DummyCodeVM GetDummyCodeById(int id);

        public bool CheckDummyCodeById(int id);

        public void UpdateDummyCode(DummyCodeVM dummyCodeVM);

        public void DeleteDummyCode(int id);
    }

}
