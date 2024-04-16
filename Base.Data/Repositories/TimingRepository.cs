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
        public bool IsExisted(TimingPost timingPost);

        public bool IsExistedById(int id);

        public string GetUserFullName(int id);

        public IEnumerable<TimingPost> PagingTimingPost(int pageIndex, int pageSize);

    }

    public class TimingRepository : GenericRepository<TimingPost>, ITimingPostRepository
    {
        public TimingRepository(Task01Context context) : base(context)
        {
        }
        public IEnumerable<TimingPost> PagingTimingPost(int pageIndex, int pageSize)
        {
             var list = _context.TimingPosts.ToList().Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();    
            return list;
        }

        public string GetUserFullName(int id)
        {
            var user = _context.UserAssigns.Find(id);
            return user.UserFullName;
        }

        public bool IsExisted(TimingPost timingPost)
        {
            return (_context.TimingPosts?.Any(tp => tp.Customer == timingPost.Customer && tp.PostName == timingPost.PostName)).GetValueOrDefault();
        }

        public bool IsExistedById(int id)
        {
            return (_context.TimingPosts?.Any(tp => tp.Id == id)).GetValueOrDefault();
        }
    }
}
