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
