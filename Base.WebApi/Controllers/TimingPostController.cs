using Base.Data.Infrastructure.UnitOfWork;
using Base.Data.Models;
using Base.Domain.Interfaces;
using Base.Domain.ViewModels;
using Base.Service.Contracts;
using Base.Service.Models.TimingPost;
using Base.Service.Services;
using Base.WebApi.Models;
using ExcelDataReader;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.IO;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Base.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimingPostController : ControllerBase
    {
        private readonly ITimingPostService _timingPostService;

        public TimingPostController(ITimingPostService timingPostService)
        {
            _timingPostService = timingPostService;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            try
            {                
                return Ok(_timingPostService.GetAll());
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                return Ok(_timingPostService.GetById(id));
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("Create")]
        public IActionResult Add(TimingRequest timingPost)
        {
            try
            {
                _timingPostService.Add(timingPost);
                return Ok(new { Message = "Thêm thành công", Success = true});
            }
            catch
            {
                return BadRequest();
            }
                        
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(TimingRequest timingPost)
        {
            try
            {
                bool result = _timingPostService.Update(timingPost).Result;
                if (!result)
                {
                    return BadRequest();
                }
                return Ok(new { Message = "Cập nhật thành công", Success = true });
            }
            catch
            {
                return BadRequest();
            }            
        }
    
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                bool result = _timingPostService.Remove(id).Result;
                if (!result)
                {
                    return BadRequest();
                }
                return Ok(new { Message = "Xóa thành công", Success = true});
            }
            catch
            {
                return BadRequest();
            }
        }
        
        [HttpPost("ImportTimingPostFromExcel")]
        public async Task<IActionResult> ImportExcel(IFormFile file, CancellationToken cancellationToken)
        {
            string filePath = string.Empty;
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Import/TimingPost/");
            filePath = path + Path.GetFileName(file.FileName);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string extension = Path.GetExtension(file.FileName);
            if(extension != ".xlsx")
            {
                return Ok(new { message = "Vui lòng chọn file định dạng Excel", success = false });
            }
            // Sử dụng luồng để ghi nội dung của tệp tin vào đường dẫn
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            if(await _timingPostService.ImportTimingPostAsync(filePath))
            {
                return Ok(new { message = "Import dữ liệu thành công", success = true});
            }
            return Ok(new { message = "Không có gì thay đổi", success = true});
        }

        [HttpGet("ExportExcelFile")]
        public async Task<IActionResult> ExportExcelFile()
        {
            byte[] data = await _timingPostService.ExportExcel();
            return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "TimingPost"); ;
        }

        [HttpGet("Paging")]
        public IActionResult PagingTimingPost(int pageIndex, int pageSize)
        {
            try
            {
                return Ok(_timingPostService.PagingTimingPost(pageIndex, pageSize));
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
