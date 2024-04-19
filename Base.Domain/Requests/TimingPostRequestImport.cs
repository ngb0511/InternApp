using Base.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Requests
{
    public class TimingPostRequestImport
    {
        public int Id { get; set; }

        public string? Customer { get; set; } = null!;

        public string? PostName { get; set; } = null!;

        public string? PostStart { get; set; }

        public string? PostEnd { get; set; }

        public int Index { get; set; }

    }
}
