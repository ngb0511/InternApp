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
        public string Customer {  get; set; }
        public string PostName { get; set; }
        public DateTime PostStart { get; set; }
        public DateTime PostEnd { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
    }
}
