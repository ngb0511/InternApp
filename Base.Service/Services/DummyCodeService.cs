using Base.Data.Infrastructure;
using Base.Data.Infrastructure.Interfaces;
using Base.Data.Models;
using Base.Data.Repositories;
using Base.Domain.ViewModels;
using Base.Service.Constract;
using ExcelDataReader;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Base.Service.Services
{
    public class DummyCodeService : IDummyCodeService
    {
        private readonly IDummyCodeRepository _dummyCodeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DummyCodeService(IDummyCodeRepository dummyCodeRepository, IUnitOfWork unitOfWork)
        {
            _dummyCodeRepository = dummyCodeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> AddDummyCode(DummyCodeVM dummyCodeVM)
        {
            try
            {
                if (CheckDummyCodeExisted(dummyCodeVM))
                {
                    return false;
                }
                else
                {
                    DummyCode dummyCode = GetDummyCodeValue(dummyCodeVM);
                    dummyCode.CreatedDate = DateTime.Now;

                    _dummyCodeRepository.Add(dummyCode);
                    await _unitOfWork.SaveChangesAsync();

                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<DummyCodeVM>> AddRangeDummyCode(IEnumerable<DummyCodeVM> dummyCodeVMs)
        {
            List<DummyCodeVM> dummyCodeVMError = new List<DummyCodeVM>();

            try
            {
                foreach (var item in dummyCodeVMs)
                {
                    if (CheckDummyCodeExisted(item))
                    {
                        dummyCodeVMError.Add(item);
                    }
                    if ((item.Material == 0) || (item.DpName == "") || (item.Description == ""))
                    {
                        dummyCodeVMError.Add(item);
                    }
                }

                if (dummyCodeVMError.Count != 0)
                {
                    return dummyCodeVMError;
                }
                else
                {
                    List<DummyCode> dummyCodes = new List<DummyCode>();
                    foreach (DummyCodeVM item in dummyCodeVMs)
                    {
                        DummyCode dummyCode = GetDummyCodeValue(item);
                        dummyCode.CreatedDate = DateTime.Now;
                        dummyCodes.Add(dummyCode);
                    }

                    _dummyCodeRepository.AddRange(dummyCodes);
                    await _unitOfWork.SaveChangesAsync();

                    return dummyCodeVMError;
                }
            }
            catch (Exception)
            {
                return dummyCodeVMError;
            }
        }

        public async Task<IEnumerable<DummyCodeVM>> RemoveRangeDummyCode(IEnumerable<DummyCodeVM> dummyCodeVMs)
        {
            List<DummyCodeVM> dummyCodeVMError = new List<DummyCodeVM>();

            try
            {
                foreach (var item in dummyCodeVMs)
                {
                    if (!CheckDummyCodeExisted(item))
                    {
                        dummyCodeVMError.Add(item);
                    }
                }

                if (dummyCodeVMError.Count != 0)
                {
                    return dummyCodeVMError;
                }
                else
                {
                    List<DummyCode> dummyCodes = new List<DummyCode>();
                    foreach (DummyCodeVM item in dummyCodeVMs)
                    {
                        DummyCode dummyCode = GetDummyCodeValue(item);
                        dummyCodes.Add(dummyCode);
                    }

                    //_context.Set<DummyCode>().Add(dummyCode);
                    _dummyCodeRepository.RemoveRange(dummyCodes);
                    await _unitOfWork.SaveChangesAsync();

                    return dummyCodeVMError;
                }
            }
            catch (Exception)
            {
                return dummyCodeVMError;
            }
        }

        public IEnumerable<DummyCodeVM> GetAllDummyCode()
        {
            var listDummyCode = _dummyCodeRepository.GetAll().ToList();

            List<DummyCodeVM> listDummyCodeVM = new List<DummyCodeVM>();

            foreach (DummyCode dummyCode in listDummyCode)
            {
                DummyCodeVM dummyCodeVM = GetDummyCodeVMValue(dummyCode);

                listDummyCodeVM.Add(dummyCodeVM);
            }

            return listDummyCodeVM;
        }

        public IEnumerable<DummyCodeVM> GetResultModel(int currentPage, int pageSize)
        {
            var listDummyCode = _dummyCodeRepository.GetAll();

            var totalCount = listDummyCode.Count();

            List<DummyCodeVM> listDummyCodeVM = new List<DummyCodeVM>();

            foreach (DummyCode dummyCode in listDummyCode)
            {
                DummyCodeVM dummyCodeVM = GetDummyCodeVMValue(dummyCode);

                listDummyCodeVM.Add(dummyCodeVM);
            }

            var DummyCodeVMs = listDummyCodeVM
                                            .Skip((currentPage - 1) * pageSize)
                                            .Take(pageSize)
                                            .ToList();
            return DummyCodeVMs;
        }

        public bool CheckDummyCodeById(int id)
        {
            var dummyCode = _dummyCodeRepository.Find(e => e.Id == id);
            if (dummyCode != null)
            {
                return true;
            }
            else return false;
        }

        public bool CheckDummyCodeExisted(DummyCodeVM dummyCodeVM)
        {
            //return _context.Set<T>().Where(expression);
            var dummyCode = _dummyCodeRepository.Find(e => (e.Material == dummyCodeVM.Material && e.DpName == dummyCodeVM.DpName && e.Description == dummyCodeVM.Description));
            if (dummyCode.Count() != 0)
            {
                return true;
            }
            else return false;
        }

        public DummyCodeVM GetDummyCodeById(int id)
        {
            var dummyCode = _dummyCodeRepository.Find(e => e.Id == id).ToList();
            DummyCodeVM dummyCodeVM = new DummyCodeVM();

            if (dummyCode != null)
            {
                dummyCodeVM = GetDummyCodeVMValue(dummyCode[0]);
            }

            return dummyCodeVM;
        }

        public IEnumerable<DummyCodeVM> FindDummyCode(Expression<Func<DummyCodeVM, bool>> expression)
        {
            var dummyCodeVMs = GetAllDummyCode();

            return dummyCodeVMs.Where(expression.Compile());
        }

        public async Task<bool> UpdateDummyCode(DummyCodeVM dummyCodeVM)
        {
            try
            {
                DummyCode dummyCode = GetDummyCodeValue(dummyCodeVM);

                _dummyCodeRepository.Update(dummyCode);
                await _unitOfWork.SaveChangesAsync();

                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        public async Task<bool> RemoveDummyCode(DummyCodeVM dummyCodeVM)
        {
            try
            {
                var dummyCode = _dummyCodeRepository.Find(e => e.Id == dummyCodeVM.Id).ToList();

                //DummyCodeVM dummyCodeVM = new DummyCodeVM();

                if (dummyCode != null)
                {
                    _dummyCodeRepository.Remove(dummyCode[0]);
                    await _unitOfWork.SaveChangesAsync();

                    return true;
                }
                else return false;
            }
            catch (Exception)
            {
                return false;
            }
            
        }

        public IEnumerable<DummyCodeVM> GetDummyCodeFromExcel(string fileName, int userId)
        {
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), "Files", fileName);

            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filepath, out var contenttype))
            {
                contenttype = "application/octet-stream";
            }

            var bytes = File.ReadAllBytesAsync(filepath);

            List<DummyCodeVM> dummyCodeVMs = new List<DummyCodeVM>();

            DataSet ds;
            var exactpath = Path.Combine(Directory.GetCurrentDirectory(), "Files", fileName);

            string extension = Path.GetExtension(exactpath);
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            using (var stream = File.Open(exactpath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
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
                                        Material = int.TryParse(dr["Material"].ToString(), out int materialValue) ? materialValue : 0,
                                        DpName = dr["DpName"]?.ToString() ?? "",
                                        Description = dr["Description"]?.ToString() ?? "",
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
            // Thiết lập LicenseContext
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var package = new ExcelPackage();
            var workSheet = package.Workbook.Worksheets.Add("DummyCode");

            // fill header
            string[] headers = { "Material", "DpName", "Description", "TotalMapping", "CreatedDate", "CreatedBy" };
            for (int i = 0; i < headers.Length; i++)
            {
                workSheet.Cells[1, i + 1].Value = headers[i];
                workSheet.Cells[1, i + 1].Style.Font.Bold = true;
                workSheet.Cells[1, i + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }

            // fill data
            int row = 2;
            foreach (var dummyCodeVM in dummyCodeVMList)
            {
                workSheet.Cells[row, 1].Value = dummyCodeVM.Material;
                workSheet.Cells[row, 2].Value = dummyCodeVM.DpName;
                workSheet.Cells[row, 3].Value = dummyCodeVM.Description;
                workSheet.Cells[row, 4].Value = dummyCodeVM.TotalMapping;
                workSheet.Cells[row, 5].Value = dummyCodeVM.CreatedDate.ToString("dd-MM-yy");
                workSheet.Cells[row, 6].Value = dummyCodeVM.CreatedBy;

                for (int i = 1; i <= 6; i++)
                {
                    workSheet.Cells[row, i].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                }

                row++;
            }

            // format column width
            for (int i = 1; i <= 6; i++)
            {
                workSheet.Column(i).AutoFit();
            }

            return await package.GetAsByteArrayAsync();
        }

        DummyCode GetDummyCodeValue(DummyCodeVM dummyCodeVM)
        {
            DummyCode dummyCode = new DummyCode();

            dummyCode.Id = dummyCodeVM.Id;
            dummyCode.DpName = dummyCodeVM.DpName;
            dummyCode.Description = dummyCodeVM.Description;
            dummyCode.CreatedDate = DateTime.Parse(dummyCodeVM.CreatedDate.ToString("yyyy-MM-dd"));
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
