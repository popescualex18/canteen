using Authentication.BusinessLogic.Interfaces;
using Authentication.DomainModels;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Api.Controllers
{
    [Route("api/")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBusinessService _userBusinessService;
        public UserController(IUserBusinessService userBusinessService)
        {
            _userBusinessService = userBusinessService;
        }
       

        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegistrationDto model)
        {
            var result = _userBusinessService.Register(model);
            return string.IsNullOrEmpty(result.Error) ? Ok(result) : BadRequest(result);
        }
        //[SwaggerRequestExample(typeof(LoginUserDto), typeof(LoginUserDtoExample))]

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginUserDto model)
        {
            var result = _userBusinessService.Login(model);
            return string.IsNullOrEmpty(result.Error) ? Ok(result) : BadRequest(result);
        }

        [HttpPost("refresh-token")]
        public IActionResult RefreshToken([FromBody] RefreshTokenDto model)
        {
            var result = _userBusinessService.RefreshToken(model);
            return string.IsNullOrEmpty(result) ? Forbid() : Ok(new RefreshTokenDto
            {
                RefreshToken = result
            });
        }
    }
}
