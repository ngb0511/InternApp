using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.RequestModels
{
    public class LogRM
    {
        public int Id { get; set; }

        public string Detail { get; set; } = null!;

        public DateTime CreatedDate { get; set; }

        public int CreatedBy { get; set; }
    }
}
