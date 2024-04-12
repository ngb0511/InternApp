using Base.Data.Infrastructure.UnitOfWork;
using Base.Data.Models;
using Base.Domain.Interfaces;
using Base.Domain.ViewModels;
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
        private readonly IUnitOfWork _unitOfWork;

        public TimingPostController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            try
            {                
                return Ok(_unitOfWork.TimingPosts.GetAll());
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetAll(int id)
        {
            try
            {                
                return Ok(_unitOfWork.TimingPosts.GetById(id));
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("Create")]
        public IActionResult Add(TimingPostVM timingPost)
        {
            try
            {
                _unitOfWork.TimingPosts.Add(timingPost);
                _unitOfWork.Complete();
                return Ok(new { Message = "Thêm thành công", Success = true});
            }
            catch
            {
                return BadRequest();
            }
                        
        }

        [HttpPut("Update")]
        public IActionResult Update(TimingPostVM timingPost)
        {
            try
            {
                if (!_unitOfWork.TimingPosts.IsExistedById(timingPost.Id))
                {
                    return NotFound();
                }
                _unitOfWork.TimingPosts.Update(timingPost);
                _unitOfWork.Complete();
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
                _unitOfWork.TimingPosts.Remove(id);
                _unitOfWork.Complete();
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("ImportTimingPostFromExcel")]
        public async Task<IActionResult> ImportExcel(IFormFile file, CancellationToken cancellationToken)
        {
            var result = await WriteFile(file);

            List<TimingPostVM> timingPostVMs = _unitOfWork.TimingPosts.GetTimingPostFromExcel(result).ToList();
            List<TimingPostVM> timingPostVMError = new List<TimingPostVM>();

            foreach (var item in timingPostVMs)
            {
                if (_unitOfWork.TimingPosts.IsExisted(item))
                {
                    timingPostVMError.Add(item);
                }
                if ((item.Customer == null) || (item.PostName == string.Empty) || (item.PostStart == null) || (item.PostEnd == null))
                {
                    timingPostVMError.Add(item);
                }
            }

            _unitOfWork.TimingPosts.AddRangeTimingPost(timingPostVMs);
            try
            {
                _unitOfWork.Complete();
            }
            catch (Exception ex) { }


            return Ok("");
        }

        private async Task<string> WriteFile(IFormFile file)
        {                  
                string filePath = string.Empty;

                var path = Path.Combine(Directory.GetCurrentDirectory(), "Import/TimingPost/");
                filePath = path + Path.GetFileName(file.FileName);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }            
                string extension = Path.GetExtension(file.FileName);
                // Sử dụng luồng để ghi nội dung của tệp tin vào đường dẫn
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
    
                return filePath;            
        }

        [HttpGet("ExportExcelFile")]
        public async Task<IActionResult> ExportExcelFile()
        {
            var listTiming = _unitOfWork.TimingPosts.GetAll().ToList();
            byte[] data = await _unitOfWork.TimingPosts.ExportExcel(listTiming);            
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Exports/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filePath = path+ "TimingPost.xlsx";
            System.IO.File.WriteAllBytes(filePath, data);
            return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "TimingPost"); ;
        }

    }
}
