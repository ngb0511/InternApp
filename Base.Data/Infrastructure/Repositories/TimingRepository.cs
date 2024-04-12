using Base.Data.Infrastructure.UnitOfWork;
using Base.Data.Models;
using Base.Domain.Interfaces;
using Base.Domain.ViewModels;
using ExcelDataReader;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Base.Data.Infrastructure.Repositories
{
    public class TimingRepository : GenericRepository<TimingPostVM>, ITimingPostRepository
    {
        public TimingRepository(Task01Context context) : base(context)
        {
        }

        public IEnumerable<TimingPostVM> GetAll()
        {
            var listTimingPost = _context.TimingPosts.ToList();
            List<TimingPostVM> listVM = new List<TimingPostVM>();

            foreach (var timingPost in listTimingPost)
            {
                var TimePost = GetTimingPostVM(timingPost);
                listVM.Add(TimePost);
            }
            return listVM;
        }

        public TimingPostVM GetById(int id)
        {
            var listTimingPost = _context.TimingPosts.Find(id);                      
            var TimePost = GetTimingPostVM(listTimingPost);                
            return TimePost;
        }

        public bool IsExisted(TimingPostVM timingPost)
        {
            return (_context.TimingPosts?.Any(tp => (tp.Customer == timingPost.Customer) && (tp.PostName == timingPost.PostName))).GetValueOrDefault();
        }

        public bool IsExistedById(int id)
        {
            return (_context.TimingPosts?.Any( tp=>tp.Id == id)).GetValueOrDefault();
        }

        public void Add(TimingPostVM entity)
        {
            var data = new TimingPost()
            {
                Customer = entity.Customer,
                PostName = entity.PostName,
                PostStart = entity.PostStart,
                PostEnd = entity.PostEnd,
                CreatedDate = DateTime.Now,
                CreatedBy = 1,
            };
            _context.TimingPosts.Add(data);
        }

        public void AddRangeTimingPost(IEnumerable<TimingPostVM> timingPostVMs)
        {
            List<TimingPost> timingPosts = new List<TimingPost>();
            foreach (TimingPostVM item in timingPostVMs)
            {
                TimingPost timingPost = GetTimingPost(item);
                timingPosts.Add(timingPost);
            }
            _context.TimingPosts.AddRange(timingPosts);
        }

        public TimingPostVM GetTimingPostVM(TimingPost timingPost)
        {
            var TimePost = new TimingPostVM();
            TimePost.Id = timingPost.Id;
            TimePost.Customer = timingPost.Customer;
            TimePost.PostName = timingPost.PostName;
            TimePost.PostStart = timingPost.PostStart;
            TimePost.PostEnd = timingPost.PostEnd;
            TimePost.CreatedDate = timingPost.CreatedDate;
            TimePost.CreatedBy = timingPost.CreatedBy;
            return TimePost;
        }

        public TimingPost GetTimingPost(TimingPostVM timingPost)
        {
            var TimePost = new TimingPost();
            TimePost.Id = timingPost.Id;
            TimePost.Customer = timingPost.Customer;
            TimePost.PostName = timingPost.PostName;
            TimePost.PostStart = timingPost.PostStart;
            TimePost.PostEnd = timingPost.PostEnd;
            TimePost.CreatedDate = DateTime.Now;
            TimePost.CreatedBy = 1;
            return TimePost;
        }



        public void Update(TimingPostVM entity)
        {
            var timingPost = _context.TimingPosts.FirstOrDefault(tp => tp.Id == entity.Id);
            if(timingPost != null)
            {
                timingPost.Customer = entity.Customer;
                timingPost.PostName = entity.PostName;
                timingPost.PostStart = entity.PostStart;
                timingPost.PostEnd = entity.PostEnd;
                _context.TimingPosts.Attach(timingPost);
                _context.Entry(timingPost).State = EntityState.Modified;
            }                      
        }

        public void Remove(int id)
        {
            var timingPost = _context.TimingPosts.FirstOrDefault(tp => tp.Id == id);
            if (timingPost != null)
            {
                _context.Remove(timingPost);
            }
        }

        public IEnumerable<TimingPostVM> GetTimingPostFromExcel(string filePath)
        {
            DataSet ds;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            string extension = Path.GetExtension(filePath);
            using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var reader = extension == ".xls" ? ExcelReaderFactory.CreateBinaryReader(stream) : ExcelReaderFactory.CreateOpenXmlReader(stream))
                {
                    ds = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true
                        }
                    });
                    reader.Close();
                }

                ds.Tables[0].Columns[0].ColumnName = "Customer";
                ds.Tables[0].Columns[1].ColumnName = "PostName";
                ds.Tables[0].Columns[2].ColumnName = "PostStart";
                ds.Tables[0].Columns[3].ColumnName = "PostEnd";
                ds.Tables[0].Columns.Add("index");

                ds.Tables[0].AcceptChanges();
                DataTable dtCloned = ds.Tables[0].Clone();

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ds.Tables[0].Rows[i]["index"] = i + 2;
                    dtCloned.ImportRow(ds.Tables[0].Rows[i]);
                }

                List<TimingPostVM> timingPosts = new List<TimingPostVM>();
                timingPosts = (from DataRow dr in dtCloned.Rows
                               select new TimingPostVM()
                               {
                                   Customer = dr["Customer"].ToString(),
                                   PostName = dr["PostName"].ToString(),
                                   PostStart = DateTime.Parse(dr["PostStart"].ToString()),
                                   PostEnd = DateTime.Parse(dr["PostEnd"].ToString()),
                               }).ToList();
                return timingPosts;
            }
        }

        public async Task<byte[]> ExportExcel(List<TimingPostVM> timingPosts)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var package = new ExcelPackage(); 
            var workSheet = package.Workbook.Worksheets.Add("TimingPost");
            //Create Title
            workSheet.Cells["A1"].Value = "Customer";
            workSheet.Cells["B1"].Value = "PostName";
            workSheet.Cells["C1"].Value = "PostStart";
            workSheet.Cells["D1"].Value = "PostEnd";
            workSheet.Cells["E1"].Value = "Created";
            workSheet.Cells["F1"].Value = "CreatedBy";

            //Fill data
            for(int i = 0; i< timingPosts.Count(); i++)
            {
                workSheet.Cells[i + 1,1].Value = (i+1).ToString();
                workSheet.Cells[i + 1,2].Value = timingPosts[i].Customer.ToString();
                workSheet.Cells[i + 1, 3].Value = timingPosts[i].PostName.ToString();
                workSheet.Cells[i + 1, 4].Value = timingPosts[i].PostStart.ToString();
                workSheet.Cells[i + 1, 5].Value = timingPosts[i].PostEnd.ToString();
                workSheet.Cells[i + 1, 6].Value = timingPosts[i].CreatedDate.ToString();
                workSheet.Cells[i + 1, 7].Value = timingPosts[i].CreatedBy.ToString();
            }

            //Format column width
            for(int i =1; i < 8; i++)
            {
                workSheet.Column(i).Width = 10;
            }
            //Format cell border
            for(int i =0; i <timingPosts.Count(); i++)
            {
                for(int j =1;j<8;j++)
                {
                    workSheet.Cells[i + 1,j].Style.Font.Size = 10;
                    workSheet.Cells[i + 1, j].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[i + 1, j].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[i + 1, j].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[i + 1, j].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                }
            }

            return await package.GetAsByteArrayAsync();            
        }


    }
}
