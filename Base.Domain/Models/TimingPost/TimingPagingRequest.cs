using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Models.TimingPost
{
    public class TimingPagingRequest
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
