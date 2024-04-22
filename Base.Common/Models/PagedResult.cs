using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Common.Models
{
    public class PagedResult<T>
    {
        public List<T>? Items { set; get; }
        public int TotalRecords { get; set; }
    }
}
