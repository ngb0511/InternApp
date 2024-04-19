using Base.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.RequestModels
{
    public class DummyCodeSearchRequest
    {
        public int? Material { get; set; }

        public string DpName { get; set; } = null!;

        public string Description { get; set; } = null!;

        public UserAssign CreatedBy { get; set; } = null!;
    }
}
