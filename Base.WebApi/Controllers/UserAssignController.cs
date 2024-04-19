using Base.Data.Infrastructure.UnitOfWork;
using Base.Domain.Interfaces;
using Base.Service.Contracts;
using Base.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Base.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAssignController : ControllerBase
    {
        private readonly IUserAssignService _userAssignService;

        public UserAssignController(IUserAssignService userAssignService)
        {
            _userAssignService = userAssignService;
        }

        [HttpGet]
        public IActionResult CheckUserAssign(string userName)
        {
            if (_userAssignService.IsExistedUserName(userName))
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
