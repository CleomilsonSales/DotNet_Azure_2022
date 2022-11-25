using System;
using Microsoft.AspNetCore.Mvc;
using Manager.API.Token;
using Microsoft.Extensions.Configuration;
using Manager.API.ViewModels;
using Manager.API.Utilities;
//using Manager.Core.Communication.Messages.Notifications;


namespace Manager.API.Controllers{
    [ApiController]
    public class AuthController : ControllerBase{
        private readonly IConfiguration _configuration;
        private readonly ITokenGenerator _tokenGenerator;


        public AuthController(IConfiguration configuration, ITokenGenerator tokenGenerator){
            _configuration = configuration;
            _tokenGenerator = tokenGenerator;
        }

        [HttpPost]
        [Route("/api/v1/auth/login")]
        public IActionResult Login([FromBody] LoginViewModel loginViewModel){
            try{
                //login fixo apenas para estudos
                var tokenLogin = _configuration["Jwt:Login"];
                var tokenPassword = _configuration["Jwt:Password"];

                if (loginViewModel.Login == tokenLogin && loginViewModel.Password == tokenPassword){
                    return Ok(new ResultViewModel{
                        Message = "Usuario autenticado com sucesso",
                        Success = true,
                        Data = new{
                            Token = _tokenGenerator.GenerateToken(),
                            TokenExpires = DateTime.UtcNow.AddHours(int.Parse(_configuration["Jwt:HoursToExpire"]))
                        }

                    });
                }else{
                    return StatusCode(401, Responses.UnauthorizedErrorMessage());
                }
            }
            catch (Exception){
                return StatusCode(500, Responses.ApplicationErrorMessage());
            }
        }
    }
}