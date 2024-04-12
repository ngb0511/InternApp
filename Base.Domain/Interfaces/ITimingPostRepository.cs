using Base.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Interfaces
{
    public interface ITimingPostRepository : IGenericRepository<TimingPostVM>
    {/*
        public IEnumerable<TimingPostVM> GetAllDummyCode();

        public void AddTimingPost(TimingPostVM timingPostVM);*/

        public void AddRangeTimingPost(IEnumerable<TimingPostVM> timingPostVM);

        public IEnumerable<TimingPostVM> GetTimingPostFromExcel(string filePath);

        public bool IsExisted(TimingPostVM timingPost);

        public bool IsExistedById(int id);

        public Task<byte[]> ExportExcel(List<TimingPostVM> timingPosts);

        /* public TimingPostVM GetTimingPostById(int id);

         public bool CheckTimingPostById(int id);

         public void UpdateTimingPost(TimingPostVM timingPostVM);

         public void DeleteTimingPost(int id);

         public IEnumerable<TimingPostVM> GetDummyCodeFromExcel(string fileName, int userId);*/
    }
}
