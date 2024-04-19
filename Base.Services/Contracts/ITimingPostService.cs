using Base.Data.Models;
using Base.Domain.Models.TimingPost;
using Base.Domain.ViewModels;
using Base.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Service.Contracts
{
    public interface ITimingPostService
    {
        string GetErrorMessage();
        string GetSuccessMessage();
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
