using Base.Data.Models;
using Base.Domain.Interfaces;
using Base.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Domain;
using System.Globalization;
using System.Linq.Expressions;

namespace Base.Data.Infrastructure.Repositories
{
    public class DummyCodeRepository : GenericRepository<DummyCodeVM>, IDummyCodeRepository
    {
        public DummyCodeRepository(Task01Context context) : base(context)
        {
        }

        public void AddDummyCode(DummyCodeVM dummyCodeVM)
        {
            DummyCode dummyCode = GetDummyCodeValue(dummyCodeVM);

            _context.Set<DummyCode>().Add(dummyCode);   
        }

        public IEnumerable<DummyCodeVM> GetAllDummyCode()
        {
            var listDummyCode = _context.Set<DummyCode>().ToList();
            List<DummyCodeVM> listDummyCodeVM = new List<DummyCodeVM>();

            foreach (DummyCode dummyCode in listDummyCode)
            {
                DummyCodeVM dummyCodeVM = GetDummyCodeVMValue(dummyCode);

                listDummyCodeVM.Add(dummyCodeVM);
            }

            return listDummyCodeVM;
        }

        public DummyCodeVM GetDummyCodeById(int id)
        {
            DummyCodeVM dummyCode = GetDummyCodeVMValue(_context.Set<DummyCode>().Find(id));

            return dummyCode;
        }

        public bool CheckDummyCodeById(int id)
        {
            //return _context.Set<T>().Where(expression);
            return (_context.DummyCodes?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public void UpdateDummyCode(DummyCodeVM dummyCodeVM)
        {
            DummyCode dummyCode = GetDummyCodeValue(dummyCodeVM);

            _context.Set<DummyCode>().Update(dummyCode);
        }

        public void DeleteDummyCode(int id)
        {
            DummyCode dummyCode = _context.Set<DummyCode>().Find(id);

            _context.Set<DummyCode>().Remove(dummyCode);
        }

        DummyCode GetDummyCodeValue(DummyCodeVM dummyCodeVM)
        {
            DummyCode dummyCode = new DummyCode();

            dummyCode.Id = dummyCodeVM.Id;
            dummyCode.DpName = dummyCodeVM.DpName;
            dummyCode.Description = dummyCodeVM.Description;
            dummyCode.CreatedDate = dummyCodeVM.CreatedDate;
            dummyCode.Material = dummyCodeVM.Material;
            dummyCode.TotalMapping = dummyCodeVM.TotalMapping;
            dummyCode.CreatedBy = dummyCodeVM.CreatedBy;

            return dummyCode;
        }

        DummyCodeVM GetDummyCodeVMValue(DummyCode dummyCode)
        {
            DummyCodeVM dummyCodeVM = new DummyCodeVM();

            dummyCodeVM.Id = dummyCode.Id;
            dummyCodeVM.DpName = dummyCode.DpName;
            dummyCodeVM.Description = dummyCode.Description;
            dummyCodeVM.CreatedDate = dummyCode.CreatedDate;
            dummyCodeVM.Material = dummyCode.Material;
            dummyCodeVM.TotalMapping = dummyCode.TotalMapping;
            dummyCodeVM.CreatedBy = dummyCode.CreatedBy;

            return dummyCodeVM;
        }
    }

}
