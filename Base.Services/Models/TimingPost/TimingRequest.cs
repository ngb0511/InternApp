using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Service.Models.TimingPost
{
    public class TimingRequest
    {
        public int Id { get; set; }
        public string Customer { get; set; } = null!;

        public string PostName { get; set; } = null!;

        public DateTime PostStart { get; set; }

        public DateTime PostEnd { get; set; }

    }
}
