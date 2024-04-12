using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        T ? GetById(int id);
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Expression<Func<T, bool>> expression);
        int Add(T entity);
        void AddRange(IEnumerable<T> entities);
        int Remove(T entity);
        int RemoveByID(int Id);
        IEnumerable<T> RemoveRange(IEnumerable<T> entities);
        void Update(T entity);

        int UpdateByID(T entity);
        IEnumerable<T> ProcessFileAsync (IFormFile file);
        public byte[] ExportToExcel(IEnumerable<T> data, string fileName);

    }
}
