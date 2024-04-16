using Base.Data.Models;
using Base.Service.Models.TimingPost;
using Base.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Service.Contracts
{
    public interface ITimingPostService
    {
        IEnumerable<TimingPostVM> GetAll();

        Task<bool> Add(TimingRequest TimingRequest);

        TimingPostVM GetById(int id);

        Task<bool> Update(TimingRequest TimingRequest);

        Task<bool> Remove(int id);

        Task<bool> ImportTimingPostAsync(string filePath);

        Task<byte[]> ExportExcel();

        IEnumerable<TimingPostVM> PagingTimingPost(int pageIndex, int pageSize);

    }
}
