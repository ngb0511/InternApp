using Base.Data.ExcelModel;
using Base.Data.Exceptions;
using Base.Data.Infrastructure.UnitOfWork;
using Base.Data.Models;
using Base.Domain.Interfaces;
using Base.Domain.ViewModels;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;



namespace Base.Data.Infrastructure.Repositories
{

    public class MaterialMasterRepository : IMaterialMaster
    {
        private readonly Task01Context _context;
        public MaterialMasterRepository(Task01Context context) 
        {
            _context = context;
        }
        public int Add(MaterialMasterVM entity)
        {

            var masterMaterial = new MaterialMaster()
            {
                Material = entity.Material,
                Description = entity?.Description ?? string.Empty,
                DpName = entity?.DpName ?? string.Empty,    
            };
            _context.Add(masterMaterial);
            return masterMaterial.Id;
        }

        public void AddRange(IEnumerable<MaterialMasterVM> entities)
        {
            foreach (var entity in entities)
            {
                MaterialMaster materialMaster = new MaterialMaster()
                {
                    // Assuming MaterialMaster has properties corresponding to MaterialMasterVM
                    Id = entity.Id,
                    Material = entity.Material,
                    Description = entity?.Description ?? string.Empty,
                    DpName = entity?.DpName ?? string.Empty, 
                };

                _context.Set<MaterialMaster>().Add(materialMaster);
            }

            _context.SaveChanges();
        }

        public IEnumerable<MaterialMasterVM> Find(Expression<Func<MaterialMasterVM, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MaterialMasterVM> GetAll()
        {
            var listMaterialMaster = _context.Set<MaterialMaster>().ToList();
            List<MaterialMasterVM> listMaterialMasterVM = listMaterialMaster.Select(materialMaster => new MaterialMasterVM
            {
                 Id = materialMaster.Id,
                 Material = materialMaster.Material,
                 Description= materialMaster.Description,
                 DpName= materialMaster.DpName,
            }).ToList();

            return listMaterialMasterVM;
        }

        public MaterialMasterVM? GetById(int id)
        {
            MaterialMaster ? materialMaster = _context.Set<MaterialMaster>().Find(id);
            if (materialMaster == null)
            {
                return null; // Hoặc trả về một đối tượng rỗng, tùy thuộc vào thiết kế của bạn
            }

            MaterialMasterVM materialMasterVM = new MaterialMasterVM()
            {
                Id = materialMaster.Id,
                Material = materialMaster.Material,
                Description = materialMaster.Description,
                DpName = materialMaster.DpName,
            };
            return materialMasterVM;
        }


        public int RemoveByID(int Id)
        {
            // Find the corresponding MaterialMaster entity in the database
            MaterialMaster ? materialMaster = _context.Set<MaterialMaster>().Find(Id);

            if (materialMaster != null)
            {
                // If the entity exists, remove it from the context
                _context.Set<MaterialMaster>().Remove(materialMaster);
                _context.SaveChanges(); // Save changes to persist the removal
                return Id;
            }
            return -1; // not found id
            
        }

        public IEnumerable<MaterialMasterVM> RemoveRange(IEnumerable<MaterialMasterVM> entities)
        {
            List<MaterialMasterVM> notFoundEntities = new List<MaterialMasterVM>();

            foreach (var entity in entities)
            {
                MaterialMaster ? materialMaster = _context.Set<MaterialMaster>().Find(entity.Id);

                if (materialMaster != null)
                {
                    _context.Set<MaterialMaster>().Remove(materialMaster);
                }
                else
                {
                    notFoundEntities.Add(entity);
                }
            }

            _context.SaveChanges(); // Save changes to persist the removals

            return notFoundEntities;
        }


        public void Update(MaterialMasterVM entity) // not used
        {
            throw new NotImplementedException();
        }

        public int Remove(MaterialMasterVM entity) // not used
        {
            throw new NotImplementedException();
        }

        public int UpdateByID(MaterialMasterVM entity)
        {
            // Find the corresponding MaterialMaster entity in the database
            MaterialMaster ?materialMaster = _context.Set<MaterialMaster>().Find(entity.Id);

            if (materialMaster != null)
            {
                // Update properties of the MaterialMaster entity with values from the MaterialMasterVM entity
                materialMaster.Material = entity.Material;
                materialMaster.Description = entity?.Description ?? string.Empty;
                materialMaster.DpName = entity?.DpName ?? string.Empty;

                _context.SaveChanges(); // Save changes to persist the updates

                return 1; // Return 1 to indicate that one entity was updated
            }
            else
            {
                return -1; // Return 0 to indicate that no entity was updated (entity with the provided ID not found)
            }
        }

        public List<MaterialMasterEM> ReadExcelFile(IFormFile file)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            List<MaterialMasterEM> excelData = new List<MaterialMasterEM>();

            // Read the Excel file and populate the list
            using (var stream = file.OpenReadStream())
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream, new ExcelReaderConfiguration()
                {
                    FallbackEncoding = Encoding.GetEncoding(65001), // UTF-8 encoding
                }))
                {
                    DataSet ds = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true
                        }
                    });

                    // Read data from the first DataTable in the DataSet
                    DataTable dt = ds.Tables[0];

                    // Convert DataTable rows to ExcelDataModel objects
                    foreach (DataRow row in dt.Rows)
                    {
                        MaterialMasterEM data = new MaterialMasterEM()
                        {
                            Material = Convert.ToInt32(row["Material"]),
                            Description = row["Description"].ToString(),
                            DpName = row["DpName"].ToString()
                        };

                        excelData.Add(data);
                    }
                }
            }

            return excelData;
        }

        public IEnumerable<MaterialMasterVM> ProcessFileAsync(IFormFile file)
        {
            List<MaterialMasterVM> notFoundIds = new List<MaterialMasterVM>();
            try
            {
                List<MaterialMasterEM> excelData = ReadExcelFile(file);

                // Process the data as needed (e.g., save to database)
                foreach (var data in excelData)
                {
                    try
                    {
                        MaterialMasterVM materialMasterVM = new MaterialMasterVM()
                        {
                            Material = data.Material,
                            DpName = data.DpName,
                            Description = data.Description,
                        };
                        var savedId = Add(materialMasterVM);
                        if (savedId == 0)
                        {
                            notFoundIds.Add(materialMasterVM);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing row: {ex.Message}");
                    }
                }

                // Save changes to the database
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Handle any other exceptions that might occur during file processing
                // Log the error or take appropriate action
                Console.WriteLine($"An error occurred while processing the file: {ex.Message}");
                throw; // Re-throw the exception to be caught by the caller if necessary
            }

            return notFoundIds;
        }
        public byte[] ExportToExcel(IEnumerable<MaterialMasterVM> data, string fileName)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            try
            {
                using (ExcelPackage excelPackage = new ExcelPackage())
                {
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add(typeof(MaterialMasterEM).Name);
                    worksheet.Cells.LoadFromCollection(data, true);

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        excelPackage.SaveAs(memoryStream);
                        return memoryStream.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                Console.WriteLine($"An error occurred during Excel export: {ex.Message}");
                throw; // Re-throw the exception to propagate it further if necessary
            }
        }
    }
}

