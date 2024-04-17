using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Domain.ViewModels;

namespace Base.Domain.RequestModels
{
    public class DummyCodeRM
    {
        public int Id { get; set; }

        public int Material { get; set; }

        public string DpName { get; set; } = null!;

        public string Description { get; set; } = null!;

        public int TotalMapping { get; set; }

        public DateTime CreatedDate { get; set; }

        public int CreatedBy { get; set; }
    }
}
