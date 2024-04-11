using Base.Data.Models;
using Base.Domain.Interfaces;
using Base.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text;

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
            var checkExist = _unitOfWork.DummyCodes.CheckDummyCodeById(id);
            if (checkExist == null)
            {
                return NotFound();
            }

            //var dummyCode = _unitOfWork.DummyCodes.GetDummyCodeById(id);

            _unitOfWork.DummyCodes.DeleteDummyCode(id);
            _unitOfWork.Complete();

            return NoContent();
        }

        /*[HttpPost("AddDummyCodeFromExcel")]
        public IActionResult AddDummyCodeFromExcel(DummyCodeVM dummyCode)
        {

        }*/
    }
}
