using Azure.Core;
using Base.Common.Models;
using Base.Data.Models;
using Base.Domain.Models;
using Base.Domain.Models.TimingPost;
using Base.Domain.ViewModels;
using Base.Intergration.Constracts;
using Base.Intergration.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Base.Intergration.ApiClients
{   
    public class TimingPostApiClient : BaseApiClient, ITimingPostApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public TimingPostApiClient(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<RequestResponse> Create(TimingRequest request)
        {
            try
            {
                var response = await AddAsync<RequestResponse, TimingRequest>($"api/TimingPost/Create", request);
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<RequestResponse> Edit(TimingRequest timingPost)
        {
            throw new NotImplementedException();
        }

        public async Task<TimingPostVM> GetById(int id)
        {
            try
            {
                var response = await GetAsync<TimingPostVM>($"api/TimingPost/" + id);
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<PagedResult<TimingPost>> GetTotalRecord()
        {
            throw new NotImplementedException();
        }
    }
}
