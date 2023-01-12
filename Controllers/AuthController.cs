using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RPG_game_dotnet.Dtos.User;

namespace RPG_game_dotnet.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpPost("Register")]
        [ProducesResponseType(200, Type = typeof(ServiceResponse<int>))]
        public async Task<IActionResult> Register (UserRegisterDto request)
        {
            var response = await _authRepository.Register(new User{Username = request.Username}, request.Password);
            if(!response.Success){
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("Login")]
        [ProducesResponseType(200, Type = typeof(ServiceResponse<string>))]
        public async Task<IActionResult> Login(UserLoginDto request)
        {
            var response = await _authRepository.Login(request.Username, request.Password);
            if(!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}