using Base.Domain.Interfaces;
using Base.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Base.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public LogController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllLog")]
        public IActionResult GetAllLog()
        {
            var GetAllLog = _unitOfWork.Logs.GetAllLog();
            return Ok(GetAllLog);
        }

        [HttpGet("GetLogById/{id}")]
        public IActionResult GetLogById(int id)
        {
            var getLogById = _unitOfWork.Logs.GetLogById(id);
            return Ok(getLogById);
        }
        [HttpPost("AddLog")]
        public IActionResult AddLog(LogVM logVM)
        {
            _unitOfWork.Logs.AddLog(logVM);
            _unitOfWork.Complete();

            return Ok();
        }
    }
}
