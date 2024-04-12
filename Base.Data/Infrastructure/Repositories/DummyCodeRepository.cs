using Base.Data.Models;
using Base.Domain.Interfaces;
using Base.Domain.ViewModels;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Domain;
using System.Globalization;
using System.Linq.Expressions;
using Microsoft.AspNetCore.StaticFiles;
using Base.Data.Infrastructure.UnitOfWork;
using System.Data;
using ExcelDataReader;
using System.Drawing;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Base.Data.Infrastructure.Repositories
{
    public class DummyCodeRepository : GenericRepository<DummyCodeVM>, IDummyCodeRepository
    {
        public DummyCodeRepository(Task01Context context) : base(context)
        {
        }

        public void AddDummyCode(DummyCodeVM dummyCodeVM)
        {
            DummyCode dummyCode = GetDummyCodeValue(dummyCodeVM);

            _context.Set<DummyCode>().Add(dummyCode);   
        }

        public void AddRangeDummyCode(IEnumerable<DummyCodeVM> dummyCodeVMs)
        {
            List<DummyCode> dummyCodes = new List<DummyCode>();
            foreach (DummyCodeVM item in dummyCodeVMs)
            {
                DummyCode dummyCode = GetDummyCodeValue(item);
                dummyCodes.Add(dummyCode);
            }
            
            //_context.Set<DummyCode>().Add(dummyCode);
            _context.Set<DummyCode>().AddRange(dummyCodes);
        }

        public IEnumerable<DummyCodeVM> GetAllDummyCode()
        {
            var listDummyCode = _context.Set<DummyCode>().ToList();
            List<DummyCodeVM> listDummyCodeVM = new List<DummyCodeVM>();

            foreach (DummyCode dummyCode in listDummyCode)
            {
                DummyCodeVM dummyCodeVM = GetDummyCodeVMValue(dummyCode);

                listDummyCodeVM.Add(dummyCodeVM);
            }

            return listDummyCodeVM;
        }

        public DummyCodeVM GetDummyCodeById(int id)
        {
            DummyCodeVM dummyCode = GetDummyCodeVMValue(_context.Set<DummyCode>().Find(id));

            return dummyCode;
        }

        public bool CheckDummyCodeById(int id)
        {
            //return _context.Set<T>().Where(expression);
            return (_context.DummyCodes?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public bool CheckDummyCodeExisted(DummyCodeVM dummyCodeVM)
        {
            //return _context.Set<T>().Where(expression);
            return (_context.DummyCodes?.Any(e => (e.Material == dummyCodeVM.Material) && (e.DpName == dummyCodeVM.DpName) && (e.Description == dummyCodeVM.Description))).GetValueOrDefault();
        }

        public void UpdateDummyCode(DummyCodeVM dummyCodeVM)
        {
            DummyCode dummyCode = GetDummyCodeValue(dummyCodeVM);

            _context.Set<DummyCode>().Update(dummyCode);
        }

        public void DeleteDummyCode(int id)
        {
            DummyCode dummyCode = _context.Set<DummyCode>().Find(id);

            _context.Set<DummyCode>().Remove(dummyCode);
        }

        public IEnumerable<DummyCodeVM> GetDummyCodeFromExcel(string fileName, int userId)
        {
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), "Files", fileName);

            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filepath, out var contenttype))
            {
                contenttype = "application/octet-stream";
            }

            var bytes = System.IO.File.ReadAllBytesAsync(filepath);

            List<DummyCodeVM> dummyCodeVMs = new List<DummyCodeVM>();

            DataSet ds;
            var exactpath = Path.Combine(Directory.GetCurrentDirectory(), "Files", fileName);

            string extension = Path.GetExtension(exactpath);
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            using (var stream = System.IO.File.Open(exactpath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var reader = extension == ".xls" ? ExcelReaderFactory.CreateBinaryReader(stream) : ExcelReaderFactory.CreateOpenXmlReader(stream))
                {
                    ds = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (__) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true
                        }

                    });
                    reader.Close();
                    ds.Tables[0].Columns[0].ColumnName = "Material";
                    ds.Tables[0].Columns[1].ColumnName = "Description";
                    ds.Tables[0].Columns[2].ColumnName = "DpName";
                    ds.Tables[0].Columns[3].ColumnName = "TotalMapping";
                    //ds.Tables[0].Columns.Add("index");

                    ds.Tables[0].AcceptChanges();
                    DataTable dtCloned = ds.Tables[0].Clone();

                    for (int i = 0; i < ds.Tables[0].Rows.Count - 1; i++)
                    {
                        //ds.Tables[0].Rows[i]["index"] = i + 2;
                        dtCloned.ImportRow(ds.Tables[0].Rows[i]);
                    }

                    dummyCodeVMs = (from DataRow dr in dtCloned.Rows
                                    select new DummyCodeVM()
                                    {
                                        Id = 0,
                                        Material = Convert.ToInt32(dr["Material"].ToString()),
                                        DpName = dr["DpName"].ToString(),
                                        Description = dr["Description"].ToString(),
                                        TotalMapping = Convert.ToInt32(dr["TotalMapping"].ToString()),
                                        CreatedDate = DateTime.Now,
                                        CreatedBy = userId,
                                    }).ToList();
                }
            }

            return dummyCodeVMs;
        }

        public async Task<byte[]> ExportExcel(IEnumerable<DummyCodeVM> dummyCodeVMList)
        {
            var dummyCodeVMs = dummyCodeVMList.ToList();
            // Thiết lập LicenseContext
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var package = new ExcelPackage();
            var workSheet = package.Workbook.Worksheets.Add("DummyCode");
            // fill header
            List<string> listHeader = new List<string>()
            {
                "A1","B1","C1","D1","E1","F1"
            };
            listHeader.ForEach(c =>
            {
                workSheet.Cells[c].Style.Font.Bold = true;
                workSheet.Cells[c].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                workSheet.Cells[c].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                workSheet.Cells[c].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                workSheet.Cells[c].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            });
            workSheet.Cells[listHeader[0]].Value = "Material";
            workSheet.Cells[listHeader[1]].Value = "DpName";
            workSheet.Cells[listHeader[2]].Value = "Description";
            workSheet.Cells[listHeader[3]].Value = "TotalMapping";
            workSheet.Cells[listHeader[4]].Value = "CreatedDate";
            workSheet.Cells[listHeader[5]].Value = "CreatedBy";
            //fill data
            for (int i = 0; i < dummyCodeVMs.Count; i++)
            {
                workSheet.Cells[i + 2, 1].Value = dummyCodeVMs[i].Material;
                workSheet.Cells[i + 2, 2].Value = dummyCodeVMs[i].DpName;
                workSheet.Cells[i + 2, 3].Value = dummyCodeVMs[i].Description;
                workSheet.Cells[i + 2, 4].Value = dummyCodeVMs[i].TotalMapping;
                workSheet.Cells[i + 2, 5].Value = dummyCodeVMs[i].CreatedDate.ToString("dd-MM-yy");
                workSheet.Cells[i + 2, 6].Value = dummyCodeVMs[i].CreatedBy;
            }
            // format column width
            for (int i = 1; i < 7; i++)
            {
                workSheet.Column(i).Width = 10;
            }
            // format cell border
            for (int i = 0; i < dummyCodeVMs.Count; i++)
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

        private DataTable GetDummyCodeData()
        {
            var listDummyCode = _context.Set<DummyCode>().ToList();
            List<DummyCodeVM> listDummyCodeVM = new List<DummyCodeVM>();

            foreach (DummyCode dummyCode in listDummyCode)
            {
                DummyCodeVM dummyCodeVM = GetDummyCodeVMValue(dummyCode);

                listDummyCodeVM.Add(dummyCodeVM);
            }

            DataTable dt = new DataTable();
            dt.TableName = "DummyCode";
            dt.Columns.Add("Material", typeof(int));
            dt.Columns.Add("DpName", typeof(string));
            dt.Columns.Add("Description", typeof(string));
            dt.Columns.Add("TotalMapping", typeof(string));
            dt.Columns.Add("CreatedDate", typeof(string));
            dt.Columns.Add("CreatedBy", typeof(string));

            //var _list = this._context.TblEmployees.ToList();
            if (listDummyCodeVM.Count > 0)
            {
                listDummyCodeVM.ForEach(item =>
                { 
                    dt.Rows.Add(item.Material, item.DpName, item.Description, item.TotalMapping, item.CreatedDate, item.CreatedBy);
                });
            }

            return dt;
        }

        DummyCode GetDummyCodeValue(DummyCodeVM dummyCodeVM)
        {
            DummyCode dummyCode = new DummyCode();

            dummyCode.Id = dummyCodeVM.Id;
            dummyCode.DpName = dummyCodeVM.DpName;
            dummyCode.Description = dummyCodeVM.Description;
            dummyCode.CreatedDate = dummyCodeVM.CreatedDate;
            dummyCode.Material = dummyCodeVM.Material;
            dummyCode.TotalMapping = dummyCodeVM.TotalMapping;
            dummyCode.CreatedBy = dummyCodeVM.CreatedBy;

            return dummyCode;
        }

        DummyCodeVM GetDummyCodeVMValue(DummyCode dummyCode)
        {
            DummyCodeVM dummyCodeVM = new DummyCodeVM();

            dummyCodeVM.Id = dummyCode.Id;
            dummyCodeVM.DpName = dummyCode.DpName;
            dummyCodeVM.Description = dummyCode.Description;
            dummyCodeVM.CreatedDate = dummyCode.CreatedDate;
            dummyCodeVM.Material = dummyCode.Material;
            dummyCodeVM.TotalMapping = dummyCode.TotalMapping;
            dummyCodeVM.CreatedBy = dummyCode.CreatedBy;

            return dummyCodeVM;
        }
    }

}
