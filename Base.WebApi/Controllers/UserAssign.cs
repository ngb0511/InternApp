using Base.Data.Infrastructure.UnitOfWork;
using Base.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Base.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAssign : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserAssign(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult CheckUserAssign(string userName)
        {
            if (_unitOfWork.UserAssigns.IsExistedUserName(userName))
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
