using Base.Data.Models;
using Base.Data.Repositories;
using Base.Domain.ViewModels;
using Base.Service.Contracts;
using Base.Domain.Models.TimingPost;
using ExcelDataReader;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.Data;
using System.Linq.Expressions;
using System.Text;
using Base.Data.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Base.Domain.Requests;

namespace Base.Service.Services
{
    public class TimingPostService : AbsService, ITimingPostService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserAssignService _userAssignService;
        private readonly ITimingPostRepository _timingPostRepository;


        public TimingPostService(ITimingPostRepository timingPostRepository, 
            IUnitOfWork unitOfWork, 
            IUserAssignService userAssignService)
        {
            _timingPostRepository = timingPostRepository;
            _unitOfWork = unitOfWork;
            _userAssignService = userAssignService;
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
            try
            {
                var timingPost = Mapper(TimingRequest);
                var exist = IsExistTimingPost(timingPost);
                if (exist)
                {
                    return false;
                }
                _timingPostRepository.Add(timingPost);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> Update(TimingRequest TimingRequest)
        {
            try
            {
                var timingPost = Mapper(TimingRequest);
                var exist = IsExistTimingPost(timingPost);
                if (exist)
                {
                    return false;
                }
                _timingPostRepository.Update(timingPost);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> Remove(int id)
        {
            if (!IsExistedById(id))
            {
                return false;
            }
            _timingPostRepository.Remove(id);
            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
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
            timingPostVM.CreatedByName = _userAssignService.GetUserFullName(timing.CreatedBy);

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

                List<TimingPostRequestImport> timingPosts = new List<TimingPostRequestImport>();
                timingPosts = (from DataRow dr in dtCloned.Rows
                               select new TimingPostRequestImport()
                               {
                                   Index = Convert.ToInt32(dr["index"].ToString()),
                                   Customer = dr["Customer"].ToString(),
                                   PostName = dr["PostName"].ToString(),
                                   PostStart = dr["PostStart"].ToString(),
                                   PostEnd = dr["PostEnd"].ToString(),
                               }).ToList();
                List<int> listIndexEmpty = new List<int>();
                List<int> listIndexDuplicate = new List<int>();
                List<int> listIndexWrongLogic = new List<int>();

                List<TimingPost> listTimingPosts = new List<TimingPost>();
                foreach (var item in timingPosts)
                {
                    if (string.IsNullOrEmpty(item.PostName)
                            || string.IsNullOrEmpty(item.Customer)
                            || string.IsNullOrEmpty(item.PostStart)
                            || string.IsNullOrEmpty(item.PostEnd))
                    {
                        listIndexEmpty.Add(item.Index);
                        continue;
                    }


                    DateTime PostStart = DateTime.Parse(item.PostStart);
                    DateTime PostEnd = DateTime.Parse(item.PostEnd);
                    if (PostStart >= PostEnd)
                    {
                        listIndexWrongLogic.Add(item.Index);
                        continue;
                    }

                    var exist = _timingPostRepository.Find(x => x.Customer.Trim().ToUpper() == item.Customer.Trim().ToUpper()
                                                         && x.PostName.Trim().ToUpper() == item.PostName.Trim().ToUpper()
                                                         && x.PostStart == PostStart
                                                         && x.PostEnd == PostEnd).Any();
                    if (exist)
                    {
                        listIndexDuplicate.Add(item.Index);
                    }
                    var timingPost = new TimingPost()
                    {
                        Customer = item.Customer,
                        PostName = item.PostName,
                        PostStart = PostStart,
                        PostEnd = PostEnd,
                        CreatedDate = DateTime.Now,
                        CreatedBy = 1
                    };
                    listTimingPosts.Add(timingPost);
                }

                if (listIndexEmpty.Any())
                    SetError($"There is blank data at line: {string.Join(", ", listIndexEmpty)}");

                if (listIndexWrongLogic.Any())
                    SetError($"Post start must before post end or start key order must before end key order: {string.Join(", ", listIndexWrongLogic)}");

                if (listIndexDuplicate.Any())
                    SetError($"Timing post is already exist at line: {string.Join(", ", listIndexDuplicate)}");

                if (!String.IsNullOrEmpty(GetError()))
                    return false;

                try
                {
                    _timingPostRepository.AddRange(listTimingPosts);
                    await _unitOfWork.SaveChangesAsync();
                }
                catch (Exception) { }
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
                workSheet.Cells[i + 1, 4].Value = Date(timingPosts[i].PostStart);
                workSheet.Cells[i + 1, 5].Value = Date(timingPosts[i].PostEnd);
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

        public string Date(DateTime date)
        {
            var day = date.Day.ToString();
            var month = date.Month.ToString();
            var year = date.Year.ToString();

            return day+"/"+month+"/"+year;
        }

        public bool IsExistTimingPost(TimingPost timingPost)
        {
            var exist = _timingPostRepository.Find(x => x.Customer.Trim().ToUpper() == timingPost.Customer.Trim().ToUpper()
                                                         && x.PostName.Trim().ToUpper() == timingPost.PostName.Trim().ToUpper()
                                                         && x.PostStart == timingPost.PostStart
                                                         && x.PostEnd == timingPost.PostEnd).Any();
            return exist;
        }

        public bool IsExistedById(int id)
        {
            var timing = _timingPostRepository.GetById(id);
            if(timing != null)
            {
                return true;
            }
            return false;
        }

        public string GetErrorMessage()
        {
            return GetError();
        }

        public string GetSuccessMessage()
        {
            return GetMessage();
        }

    }
}
