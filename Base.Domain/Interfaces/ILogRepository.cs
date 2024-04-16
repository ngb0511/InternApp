using Base.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Interfaces
{
    public interface ILogRepository 
    {
        public void AddLog(LogVM logVM);

        public IEnumerable<LogVM> GetAllLog();

        public LogVM GetLogById(int id);

        public bool CheckLogById(int id);
    }
}
