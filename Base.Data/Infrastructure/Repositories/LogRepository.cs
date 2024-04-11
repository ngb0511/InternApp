﻿using Base.Data.Models;
using Base.Domain.Interfaces;
using Base.Domain.ViewModels;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Data.Infrastructure.Repositories
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

        public async Task<byte[]> ExportExcel(IEnumerable<LogVM> logVMList)
        {
            var logVMs = logVMList.ToList();
            // Thiết lập LicenseContext
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var package = new ExcelPackage();
            var workSheet = package.Workbook.Worksheets.Add("Log");
            // fill header
            List<string> listHeader = new List<string>()
            {
                "A1","B1","C1"
            };
            listHeader.ForEach(c =>
            {
                workSheet.Cells[c].Style.Font.Bold = true;
                workSheet.Cells[c].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                workSheet.Cells[c].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                workSheet.Cells[c].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                workSheet.Cells[c].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            });
            workSheet.Cells[listHeader[0]].Value = "Detail";
            workSheet.Cells[listHeader[1]].Value = "CreatedDate";
            workSheet.Cells[listHeader[2]].Value = "CreatedBy";
            //fill data
            for (int i = 0; i < logVMs.Count; i++)
            {
                workSheet.Cells[i + 2, 1].Value = logVMs[i].Detail;
                workSheet.Cells[i + 2, 2].Value = logVMs[i].CreatedDate.ToString("dd-MM-yy");
                workSheet.Cells[i + 2, 3].Value = logVMs[i].CreatedBy;
            }
            // format column width
            for (int i = 1; i < 7; i++)
            {
                workSheet.Column(i).Width = 10;
            }
            // format cell border
            for (int i = 0; i < logVMs.Count; i++)
            {
                for (int j = 1; j < 7; j++)
                {
                    workSheet.Cells[i + 2, j].Style.Font.Size = 10;
                    workSheet.Cells[i + 2, j].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[i + 2, j].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[i + 2, j].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[i + 2, j].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                }
            }
            return await package.GetAsByteArrayAsync();
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
