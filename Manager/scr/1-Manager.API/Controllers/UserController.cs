using Microsoft.AspNetCore.Mvc;
using System;
using Manager.API.ViewModels;
using System.Threading.Tasks;
using Manager.Core.Expeceptions;
using Manager.Services.Interfaces;
using AutoMapper;
using Manager.Services.DTO;
using Manager.API.Utilities;
using Microsoft.AspNetCore.Authorization;

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
        [Authorize]
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

        [HttpPut]
        [Route("/api/v1/users/update")]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] UpdateUserViewModel userViewModel){
            try{
                var userDTO = _mapper.Map<UserDTO>(userViewModel);
                var userUpdated = await _userService.Update(userDTO);

                return Ok(new ResultViewModel{
                    Message = "Usuario atualizado com sucesso",
                    Success = true,
                    Data = userUpdated

                });
            }
            catch (DomainException ex){
                
                return BadRequest(Responses.DomainErrorMessage(ex.Message, ex.Errors));
            }
            catch (Exception){
                
                return StatusCode(500, Responses.ApplicationErrorMessage());
            }
        }

        [HttpDelete]
        [Route("/api/v1/users/remove/{id}")]
        [Authorize]
        public async Task<IActionResult> Remove(long id){
            try{
                await _userService.Remove(id);

                return Ok(new ResultViewModel{
                    Message = "Usuario removido com sucesso",
                    Success = true,
                    Data = null

                });
            }
            catch (DomainException ex){
                
                return BadRequest(Responses.DomainErrorMessage(ex.Message));
            }
            catch (Exception){
                
                return StatusCode(500, Responses.ApplicationErrorMessage());
            }
        }

        [HttpGet]
        [Route("/api/v1/users/get/{id}")]
        [Authorize]
        public async Task<IActionResult> Get(long id){
            try{
                
                var user = await _userService.Get(id);

                if(user == null)
                    return Ok(new ResultViewModel{
                        Message = "Usuario não encontrado",
                        Success = true,
                        Data = user

                    });
                
                return Ok(new ResultViewModel{
                    Message = "Usuario encontrado com sucesso",
                    Success = true,
                    Data = user

                });
                
            }
            catch (DomainException ex){
                
                return BadRequest(Responses.DomainErrorMessage(ex.Message));
            }
            catch (Exception){
                
                return StatusCode(500, Responses.ApplicationErrorMessage());
            }
        }

        [HttpGet]
        [Route("/api/v1/users/get-all")]
        [Authorize]
        public async Task<IActionResult> Get(){
            try{
                
                var allUser = await _userService.Get();
                
                return Ok(new ResultViewModel{
                    Message = "Usuarios encontrados com sucesso",
                    Success = true,
                    Data = allUser

                });
                
            }
            catch (DomainException ex){
                
                return BadRequest(Responses.DomainErrorMessage(ex.Message));
            }
            catch (Exception){
                
                return StatusCode(500, Responses.ApplicationErrorMessage());
            }
        }

        [HttpGet]
        [Route("/api/v1/users/get-by-email")]
        [Authorize]
        public async Task<IActionResult> GetByEmail([FromBody] string email){
            try{
                
                var user = await _userService.GetByEmail(email);
                
                if(user == null)
                    return Ok(new ResultViewModel{
                        Message = "Email não encontrado",
                        Success = true,
                        Data = user

                    });
                
                return Ok(new ResultViewModel{
                    Message = "Email encontrado com sucesso",
                    Success = true,
                    Data = user

                });
                
            }
            catch (DomainException ex){
                
                return BadRequest(Responses.DomainErrorMessage(ex.Message));
            }
            catch (Exception){
                
                return StatusCode(500, Responses.ApplicationErrorMessage());
            }
        }

        [HttpGet]
        [Route("/api/v1/users/search-by-name")]
        [Authorize]
        public async Task<IActionResult> SearchByName([FromBody] string name){
            try{
                
                var allUser = await _userService.SearchByName(name);
                
                if(allUser.Count == 0)
                    return Ok(new ResultViewModel{
                        Message = "Usuario não encontrado por nome",
                        Success = true,
                        Data = null

                    });
                
                return Ok(new ResultViewModel{
                    Message = "Usuario encontrado por nome",
                    Success = true,
                    Data = allUser

                });
                
            }
            catch (DomainException ex){
                
                return BadRequest(Responses.DomainErrorMessage(ex.Message));
            }
            catch (Exception){
                
                return StatusCode(500, Responses.ApplicationErrorMessage());
            }
        }

        [HttpGet]
        [Route("/api/v1/users/search-by-email")]
        [Authorize]
        public async Task<IActionResult> SearchByEmail([FromBody] string email){
            try{
                
                var allUser = await _userService.SearchByEmail(email);
                
                if(allUser.Count == 0)
                    return Ok(new ResultViewModel{
                        Message = "Usuario não encontrado por email",
                        Success = true,
                        Data = null

                    });
                
                return Ok(new ResultViewModel{
                    Message = "Usuario encontrado por email",
                    Success = true,
                    Data = allUser

                });
                
            }
            catch (DomainException ex){
                
                return BadRequest(Responses.DomainErrorMessage(ex.Message));
            }
            catch (Exception){
                
                return StatusCode(500, Responses.ApplicationErrorMessage());
            }
        }

    }
}