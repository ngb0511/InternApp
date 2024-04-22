using Base.Common.Models;
using Base.Data.Models;
using Base.Domain.Models;
using Base.Domain.Models.TimingPost;
using Base.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Intergration.Constracts
{
    public interface ITimingPostApiClient
    {

        Task<RequestResponse> Create(TimingRequest timingPost);

        Task<TimingPostVM> GetById(int id);

        Task<RequestResponse> Edit(TimingRequest timingPost);

        Task<PagedResult<TimingPost>> GetTotalRecord();

    }
}
