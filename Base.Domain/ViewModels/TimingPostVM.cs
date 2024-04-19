using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.ViewModels
{
    public class TimingPostVM
    {
        public int Id { get; set; }

        public string Customer { get; set; } = null!;

        public string PostName { get; set; } = null!;

        public DateTime PostStart { get; set; }

        public DateTime PostEnd { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string CreatedByName { get; set; }
        public int Index { get; set; }

    }
}
