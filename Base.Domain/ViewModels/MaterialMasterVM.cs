using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.ViewModels
{
    public class MaterialMasterVM
    {
        public int Id { get; set; }
        public int Material { get; set; }
        public string ? Description { get; set; }
        public string ? DpName { get; set; }  

    }
}
