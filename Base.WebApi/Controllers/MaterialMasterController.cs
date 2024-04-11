using Base.Domain.Interfaces;
using Base.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http;
namespace Base.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialMasterController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public MaterialMasterController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
         [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var materialMasters = _unitOfWork.MaterialMaster.GetAll();
            return Ok(materialMasters);
        }
        [HttpGet("GetById/{id}")]
        public IActionResult GetById(int id)
        {
            MaterialMasterVM ?materialMasterVM = _unitOfWork.MaterialMaster.GetById(id);
            if (materialMasterVM == null)
            {
                return NotFound($"Material master with ID {id} was not found.");
            }

            return Ok(materialMasterVM);
        }
        [HttpPost("Add")]
        public IActionResult Add(MaterialMasterVM materialMasterVM)
        {
            _unitOfWork.MaterialMaster.Add(materialMasterVM);
            _unitOfWork.Complete();

            return Ok();
        }

        
        [HttpDelete("DeleteMaterialMaster/{id}")]
        public IActionResult Delete(int id)
        {
            var result = _unitOfWork.MaterialMaster.RemoveByID(id);
            if (result > -1)
            {
                _unitOfWork.Complete();
                return NoContent();
  
            }
            return NotFound($"Material master with ID {id} was not found.");
        }

        [HttpPut("UpdateMaterialMaster/{id}")]
        public IActionResult Update(int id, MaterialMasterVM materialMasterVM)
        {
            materialMasterVM.Id = id;
            var result = _unitOfWork.MaterialMaster.UpdateByID(materialMasterVM);
            if (result > -1)
            {
                _unitOfWork.Complete();
                return NoContent();

            }
            return NotFound($"Material master with ID {materialMasterVM.Id} was not found.");
        }

        [HttpPost]
        [Route("UploadFile")]
        public IActionResult UploadFile(IFormFile file)
        {
            if (file == null || file.Length <= 0)
            {
                return BadRequest("File is not provided or empty.");
            }

            try
            {
                _unitOfWork.MaterialMaster.ProcessFileAsync(file);
                return Ok("File uploaded and data processed successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("ExportFile")]
        public IActionResult ExportMaterialMasterToExcel()
        {
            var materialMasterList = _unitOfWork.MaterialMaster.GetAll();
            var excelBytes = _unitOfWork.MaterialMaster.ExportToExcel(materialMasterList, "MaterialMaster.xlsx");

            // Trả về file Excel dưới dạng phản hồi
            return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "MaterialMaster.xlsx");
        }

    }
}
