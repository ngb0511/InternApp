using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Data.Exceptions
{
    public class MaterialMasterNotFoundException : Exception
    {
        public MaterialMasterNotFoundException()
        {
        }

        public MaterialMasterNotFoundException(string message)
            : base(message)
        {
        }

        public MaterialMasterNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
