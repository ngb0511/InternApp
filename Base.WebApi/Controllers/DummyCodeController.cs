using Base.Data.Models;
using Base.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text;
using System.IO;
using ExcelDataReader;
using Microsoft.AspNetCore.StaticFiles;
using System.Drawing;
using System.Drawing.Printing;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Base.Data.Infrastructure.Interfaces;
using Base.Service.Constract;
//using Cake.Core.IO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Base.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DummyCodeController : ControllerBase
    {
        //private readonly IUnitOfWork _unitOfWork;
        private readonly IDummyCodeService _dummyCodeService;
        public DummyCodeController(IDummyCodeService dummyCodeService)
        {
            _dummyCodeService = dummyCodeService;
        }

        [HttpGet("GetAllDummyCode")]
        public IActionResult GetAllDummyCode()
        {
            var getAllDummyCode = _dummyCodeService.GetAllDummyCode();
            return Ok(getAllDummyCode);
        }

        [HttpGet("GetPagedDummyCode")]
        public IActionResult GetPagedDummyCode([FromQuery] int page, [FromQuery] int pageSize)
        {
            var DummyCodeVMs = _dummyCodeService.GetResultModel(page, pageSize); // Số lượng tổng số dòng trong cơ sở dữ liệu

            // Trả về dữ liệu phân trang và thông tin về tổng số trang, trang hiện tại và dữ liệu của trang đó
            return Ok(DummyCodeVMs);
        }

        [HttpGet("GetTotalPages/{pageSize}")]
        public int GetTotalPages(int pageSize)
        {
            if (_dummyCodeService.GetAllDummyCode() != null)
            {
                var totalCount = _dummyCodeService.GetAllDummyCode().Count();
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

                return totalPages;
            }

            return 0;
        }


        [HttpGet("FindDummyCodeByDpName")]
        public IActionResult FindDummyCodeByDpName(string dpName)
        {
            var DummyCodes = _dummyCodeService.FindDummyCode(e => e.DpName == dpName);
            return Ok(DummyCodes);
        }

        [HttpGet("GetDummyCodeById/{id}")]
        public IActionResult GetDummyCodeById(int id)   
        {
            var getDummyCode = _dummyCodeService.GetDummyCodeById(id);
            return Ok(getDummyCode);
        }

        [HttpPost("AddDummyCode")]
        public async Task<IActionResult> AddDummyCode(DummyCodeVM dummyCodeVM)
        {
            var result = await _dummyCodeService.AddDummyCode(dummyCodeVM);
            if (!result)
            {
                return BadRequest("Thêm ko thành công");
            }

            return Ok();
        }

        [HttpPut("UpdateDummyCode/{id}")]
        public async Task<IActionResult> UpdateDummyCode(int id, DummyCodeVM dummyCodeVM)
        {
            if (!_dummyCodeService.CheckDummyCodeById(id))
            {
                return NotFound();
            }

            var result = await _dummyCodeService.UpdateDummyCode(dummyCodeVM);

            return Ok(result);
        }

        [HttpDelete("DeleteDummyCode/{id}")]
        public async Task<IActionResult> DeleteDummyCode(int id)
        {
            var dummyCodeVM = _dummyCodeService.GetDummyCodeById(id);

            var result = await _dummyCodeService.RemoveDummyCode(dummyCodeVM);

            if (!result)
            {
                return BadRequest("Không thể xóa");
            }

            return Ok(dummyCodeVM);
        }

        private async Task<string> WriteFile(IFormFile file)
        {
            string filename = "";
            try
            {
                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                filename = file.FileName.Split('.')[file.FileName.Split('.').Length - 2] + extension;

                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "Files");

                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                var exactpath = Path.Combine(Directory.GetCurrentDirectory(), "Files", filename);
                using (var stream = new FileStream(exactpath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            catch(Exception) { }

            return filename;
        }

        [HttpPost("ImportDummyCodeFromExcel/{id}")]
        public async Task<IActionResult> ImportDummyCodeFromExcel(int id, IFormFile file, CancellationToken cancellationtoken)
        {
            var result = await WriteFile(file);

            List<DummyCodeVM> dummyCodeVMs = _dummyCodeService.GetDummyCodeFromExcel(result, id).ToList();

            IEnumerable<DummyCodeVM> dummyCodeVMError = await _dummyCodeService.AddRangeDummyCode(dummyCodeVMs);

            List<DummyCodeVM> dummyCodeVMErrorList = dummyCodeVMError.ToList();

            if (dummyCodeVMErrorList.Count != 0)
            {
                return NotFound(dummyCodeVMError);
            }

            return Ok();
        }

        [HttpGet("ExportDummyCodeToExcel")]
        public async Task<IActionResult> ExportDummyCodeToExcel()
        {
            var getAllDummyCode = _dummyCodeService.GetAllDummyCode();
            byte[] data = await _dummyCodeService.ExportExcel(getAllDummyCode);

            // Trả về tệp Excel từ dữ liệu
            return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Exported_DummyCode.xlsx");
        }

        private string? GetError()
        {
            throw new NotImplementedException();
        }
    }
}
