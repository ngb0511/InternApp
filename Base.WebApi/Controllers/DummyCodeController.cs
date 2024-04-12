using Base.Data.Models;
using Base.Domain.Interfaces;
using Base.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text;
using System.IO;
using ExcelDataReader;
using Microsoft.AspNetCore.StaticFiles;
using System.Drawing;
//using Cake.Core.IO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Base.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DummyCodeController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public DummyCodeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllDummyCode")]
        public IActionResult GetAllDummyCode()
        {
            var getAllDummyCode = _unitOfWork.DummyCodes.GetAllDummyCode();
            return Ok(getAllDummyCode);
        }

        [HttpGet("GetDummyCodeById/{id}")]
        public IActionResult GetDummyCodeById(int id)   
        {
            var getDummyCode = _unitOfWork.DummyCodes.GetDummyCodeById(id);
            return Ok(getDummyCode);
        }

        [HttpPost("AddDummyCode")]
        public IActionResult AddDummyCode(DummyCodeVM dummyCodeVM)
        {
            _unitOfWork.DummyCodes.AddDummyCode(dummyCodeVM);
            _unitOfWork.Complete();

            return Ok();
        }

        [HttpPut("UpdateDummyCode/{id}")]
        public IActionResult UpdateDummyCode(int id, DummyCodeVM dummyCodeVM)
        {
            if (id != dummyCodeVM.Id)
            {
                return BadRequest();
            }

            var checkExist = _unitOfWork.DummyCodes.CheckDummyCodeById(id);
            if (checkExist == null)
            {
                return NotFound();
            }

            _unitOfWork.DummyCodes.UpdateDummyCode(dummyCodeVM);

            try
            {
                _unitOfWork.Complete();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }

        [HttpDelete("DeleteDummyCode/{id}")]
        public IActionResult DeleteDummyCode(int id)
        {
            /*var checkExist = _unitOfWork.DummyCodes.CheckDummyCodeById(id);
            if (checkExist == null)
            {
                return NotFound();
            }*/

            //var dummyCode = _unitOfWork.DummyCodes.GetDummyCodeById(id);

            _unitOfWork.DummyCodes.DeleteDummyCode(id);
            _unitOfWork.Complete();

            return NoContent();
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
            catch (Exception ex)
            {
            }
            return filename;
        }

        [HttpPost("ImportDummyCodeFromExcel/{id}")]
        public async Task<IActionResult> ImportDummyCodeFromExcel(int id, IFormFile file, CancellationToken cancellationtoken)
        {
            var result = await WriteFile(file);

            List<DummyCodeVM> dummyCodeVMs = _unitOfWork.DummyCodes.GetDummyCodeFromExcel(result, id).ToList();
            List<DummyCodeVM> dummyCodeVMError = new List<DummyCodeVM>();

            foreach (var item in dummyCodeVMs)
            {
                if (_unitOfWork.DummyCodes.CheckDummyCodeExisted(item))
                {
                    dummyCodeVMError.Add(item);
                }
                if ((item.Material == null) || (item.DpName == string.Empty) || (item.Description == string.Empty))
                {
                    dummyCodeVMError.Add(item);
                }
            }

            if (dummyCodeVMError.Count != 0)
            {
                return NotFound(dummyCodeVMError);
            }
            else
            {
                _unitOfWork.DummyCodes.AddRangeDummyCode(dummyCodeVMs);
            }

            try
            {
                _unitOfWork.Complete();
            }
            catch (Exception ex) { }
            return Ok();
        }

        [HttpGet("ExportDummyCodeToExcel")]
        public async Task<IActionResult> ExportDummyCodeToExcel()
        {
            var getAllDummyCode = _unitOfWork.DummyCodes.GetAllDummyCode();

            byte[] data = await _unitOfWork.DummyCodes.ExportExcel(getAllDummyCode);
            /*string filePath = Path.Combine(Path.GetTempPath(), "Exported_DummyCode.xlsx");

            int i = 1;
            while (System.IO.File.Exists(filePath))
            {
                filePath = Path.Combine(Path.GetTempPath(), "Exported_DummyCode" + "(" + i + ")" +".xlsx");
                i++;
            }

            System.IO.File.WriteAllBytes(filePath, data);*/

            return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Exported_DummyCode.xlsx");
        }

        private string? GetError()
        {
            throw new NotImplementedException();
        }

        /*bool CheckExistedDummyCode(DummyCodeVM dummyCodeVM)
        {
            return false;
        }*/
    }
}
