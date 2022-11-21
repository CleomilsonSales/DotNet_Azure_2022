using Microsoft.AspNetCore.Mvc;
using System;
using Manager.API.ViewModels;
using System.Threading.Tasks;
using Manager.Core.Expeceptions;
using Manager.Services.Interfaces;
using AutoMapper;
using Manager.Services.DTO;
using Manager.API.Utilities;

namespace Manager.API.Controllers{

    [ApiController]
    public class UserController : ControllerBase{
        //metodos em controller são chamadas de actions
        //injeção de dependencia - não esquece de configurar o Startup e o appsettings.json
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IMapper mapper, IUserService userService){
            _mapper = mapper;
            _userService = userService;
        }
        //fim injeção de dependencia

        [HttpPost]
        [Route("/api/v1/users/create")]

        //FromBody quer dizer que os dados vem do corpo da requisição
        public async Task<IActionResult> Create([FromBody] CreateUserViewModel userViewModel){
            try{
                var userDTO = _mapper.Map<UserDTO>(userViewModel);
                var userCreated = await _userService.Create(userDTO);

                return Ok(new ResultViewModel{
                    Message = "Usuario criado com sucesso",
                    Success = true,
                    Data = userCreated

                });
            }
            catch (DomainException ex){
                
                return BadRequest(Responses.DomainErrorMessage(ex.Message, ex.Errors));
            }
            catch (Exception){
                
                return StatusCode(500, Responses.ApplicationErrorMessage());
            }
        }

    }
}