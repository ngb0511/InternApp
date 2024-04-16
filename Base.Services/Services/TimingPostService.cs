using Base.Data.Infrastructure.UnitOfWork;
using Base.Data.Models;
using Base.Data.Repositories;
using Base.Domain.Interfaces;
using Base.Domain.ViewModels;
using Base.Service.Contracts;
using Base.Service.Models.TimingPost;
using ExcelDataReader;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.Data;
using System.Linq.Expressions;
using System.Text;
using Base.Service.ViewModel;

namespace Base.Service.Services
{
    public class TimingPostService : ITimingPostService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITimingPostRepository _timingPostRepository;

        public TimingPostService(ITimingPostRepository timingPostRepository, IUnitOfWork unitOfWork)
        {
            _timingPostRepository = timingPostRepository;
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<TimingPostVM> GetAll()
        {
            var listTimingPost = _timingPostRepository.GetAll();
            var listTimingPostVM = new List<TimingPostVM>();
            var index = 1;
            foreach (var item in listTimingPost)
            {
                TimingPostVM timingPostVM = ConvertToVM(item);
                timingPostVM.Index = index;
                listTimingPostVM.Add(timingPostVM);
                index++;
            }
            return listTimingPostVM;
        }

        public TimingPostVM GetById(int id)
        {
            var timingPostVM = ConvertToVM(_timingPostRepository.GetById(id));
            return timingPostVM;
        }

        public async Task<bool> Add(TimingRequest TimingRequest)
        {
            var timingPost = Mapper(TimingRequest);
            _timingPostRepository.Add(timingPost);
            try
            {
                _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> Update(TimingRequest TimingRequest)
        {
            var timingPost = Mapper(TimingRequest);
            if (!_timingPostRepository.IsExistedById(TimingRequest.Id))
            {
                return false;
            }
            _timingPostRepository.Update(timingPost);
            try
            {
                _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> Remove(int id)
        {
            if (!_timingPostRepository.IsExistedById(id))
            {
                return false;
            }
            _timingPostRepository.Remove(id);
            try
            {
                _unitOfWork.Complete();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }


        public TimingPost Mapper(TimingRequest request)
        {
            var timingPost = new TimingPost();
            timingPost.Id = request.Id;
            timingPost.Customer = request.Customer;
            timingPost.PostName = request.PostName;
            timingPost.PostStart = request.PostStart;
            timingPost.PostEnd = request.PostEnd;
            timingPost.CreatedDate = DateTime.Now;
            timingPost.CreatedBy = 1;

            return timingPost;
        }

        public TimingPostVM ConvertToVM(TimingPost timing)
        {
            TimingPostVM timingPostVM = new TimingPostVM();
            timingPostVM.Id = timing.Id;
            timingPostVM.Customer = timing.Customer;
            timingPostVM.PostName = timing.PostName;
            timingPostVM.PostStart = timing.PostStart;
            timingPostVM.PostEnd = timing.PostEnd;
            timingPostVM.CreatedDate = timing.CreatedDate;
            timingPostVM.CreatedByName = _timingPostRepository.GetUserFullName(timing.CreatedBy);

            return timingPostVM;
        }

        public async Task<bool> ImportTimingPostAsync(string filePath)
        {
            DataSet ds;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            string extension = Path.GetExtension(filePath);
            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
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

                List<TimingPost> timingPosts = new List<TimingPost>();
                timingPosts = (from DataRow dr in dtCloned.Rows
                               select new TimingPost()
                               {
                                   Customer = dr["Customer"].ToString(),
                                   PostName = dr["PostName"].ToString(),
                                   PostStart = DateTime.Parse(dr["PostStart"].ToString()),
                                   PostEnd = DateTime.Parse(dr["PostEnd"].ToString()),
                               }).ToList();
                List<TimingPost> timingPostError = new List<TimingPost>();
                foreach (var item in timingPosts)
                {
                    if (_timingPostRepository.IsExisted(item))
                    {
                        timingPostError.Add(item);
                    }
                    if ((item.Customer == null) || (item.PostName == string.Empty) || (item.PostStart.ToString() == "") || (item.PostEnd.ToString() == ""))
                    {
                        timingPostError.Add(item);
                    }
                }

                if(timingPostError.Count != 0)
                {
                    return false;
                }
                else
                {
                    _timingPostRepository.AddRange(timingPosts);
                }
                try
                {
                    _unitOfWork.Complete();
                }
                catch (Exception ex) { }
                return true;
            }
        }

        public async Task<byte[]> ExportExcel()
        {
            List<TimingPostVM> timingPosts = GetAll().ToList();
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
            for (int i = 0; i < timingPosts.Count(); i++)
            {
                workSheet.Cells[i + 1, 1].Value = (i + 1).ToString();
                workSheet.Cells[i + 1, 2].Value = timingPosts[i].Customer.ToString();
                workSheet.Cells[i + 1, 3].Value = timingPosts[i].PostName.ToString();
                workSheet.Cells[i + 1, 4].Value = timingPosts[i].PostStart.ToString();
                workSheet.Cells[i + 1, 5].Value = timingPosts[i].PostEnd.ToString();
                workSheet.Cells[i + 1, 6].Value = timingPosts[i].CreatedDate.ToString();
                workSheet.Cells[i + 1, 7].Value = timingPosts[i].CreatedByName.ToString();
            }

            //Format column width
            for (int i = 1; i < 8; i++)
            {
                workSheet.Column(i).Width = 10;
            }
            //Format cell border
            for (int i = 0; i < timingPosts.Count(); i++)
            {
                for (int j = 1; j < 8; j++)
                {
                    workSheet.Cells[i + 1, j].Style.Font.Size = 10;
                    workSheet.Cells[i + 1, j].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[i + 1, j].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[i + 1, j].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[i + 1, j].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                }
            }

            return await package.GetAsByteArrayAsync();
        }

        public IEnumerable<TimingPostVM> PagingTimingPost(int pageIndex,int pageSize)
        {
            var listTimingPostVM = GetAll().Skip((pageIndex-1)*pageSize).Take(pageSize).ToList();
            return listTimingPostVM;
        }
    }
}
