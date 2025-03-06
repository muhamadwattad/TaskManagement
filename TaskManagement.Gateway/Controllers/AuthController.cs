using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.BusinessLogic.BL;
using TaskManagement.BusinessLogic.BL.DTOs;

namespace TaskManagement.Gateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(AuthBL authBL) : ControllerBase
    {

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
        [HttpPost("Login")]
        public IActionResult Login(AuthDTOs.Request.Login dto)
        {
            var token = authBL.Login(dto);
            return Ok(token);
        }
    }
}
