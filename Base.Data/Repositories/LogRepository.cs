using Base.Data.Models;
using Base.Domain.Interfaces;
using Base.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Data.Repositories
{
    public class LogRepository : GenericRepository<LogVM>, ILogRepository
    {
        public LogRepository(Task01Context context) : base(context)
        {
        }
        public void AddLog(LogVM logVM)
        {
            Log log = GetLogValue(logVM);

            _context.Set<Log>().Add(log);
        }

        public IEnumerable<LogVM> GetAllLog()
        {
            var listLog = _context.Set<Log>().ToList();
            List<LogVM> listLogVM = new List<LogVM>();

            foreach (Log log in listLog)
            {
                LogVM logVM = GetLogVMValue(log);

                listLogVM.Add(logVM);
            }

            return listLogVM;
        }

        public LogVM GetLogById(int id)
        {
            LogVM log = GetLogVMValue(_context.Set<Log>().Find(id));

            return log;
        }

        public bool CheckLogById(int id)
        {
            //return _context.Set<T>().Where(expression);
            return (_context.Logs?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        Log GetLogValue(LogVM logVM)
        {
            Log log = new Log();

            log.Id = logVM.Id;
            log.Detail = logVM.Detail;
            log.CreatedDate = logVM.CreatedDate;
            log.CreatedBy = logVM.CreatedBy;

            return log;
        }

        LogVM GetLogVMValue(Log log)
        {
            LogVM logVM = new LogVM();

            logVM.Id = log.Id;
            logVM.Detail = log.Detail;
            logVM.CreatedDate = log.CreatedDate;
            logVM.CreatedBy = log.CreatedBy;

            return logVM;
        }
    }
}
