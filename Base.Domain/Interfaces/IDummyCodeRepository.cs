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
        //IEnumerable<DummyCodeVM> GetPopularDevelopers(int count);
    }

}
