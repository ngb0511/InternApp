using Base.Data.Infrastructure.Interfaces;
using Base.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Base.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        //private readonly IUnitOfWork _unitOfWork;
        //public LogController(IUnitOfWork unitOfWork)
        //{
        //    _unitOfWork = unitOfWork;
        //}

        //[HttpGet("GetAllLog")]
        //public IActionResult GetAllLog()
        //{
        //    var GetAllLog = _unitOfWork.Logs.GetAll();
        //    return Ok(GetAllLog);
        //}

        //[HttpGet("GetLogById/{id}")]
        //public IActionResult GetLogById(int id)
        //{
        //    var getLogById = _unitOfWork.Logs.GetById(id);
        //    return Ok(getLogById);
        //}
        //[HttpPost("AddLog")]
        //public IActionResult AddLog(LogVM logVM)
        //{
        //    _unitOfWork.Logs.Add(logVM);
        //    _unitOfWork.Complete();

        //    return Ok();
        //}

        //[HttpGet("ExportLogToExcel")]
        //public async Task<IActionResult> ExportLogToExcel()
        //{
        //    var getAllLog = _unitOfWork.Logs.GetAll();

        //    byte[] data = await _unitOfWork.Logs.ExportExcel(getAllLog);
        //    /*string filePath = Path.Combine(Path.GetTempPath(), "Exported_Log.xlsx");

        //    int i = 1;
        //    while (System.IO.File.Exists(filePath))
        //    {
        //        filePath = Path.Combine(Path.GetTempPath(), "Exported_Log" + "(" + i + ")" +".xlsx");
        //        i++;
        //    }

        //    System.IO.File.WriteAllBytes(filePath, data);*/

        //    return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Exported_Log.xlsx");
        //}
    }
}
