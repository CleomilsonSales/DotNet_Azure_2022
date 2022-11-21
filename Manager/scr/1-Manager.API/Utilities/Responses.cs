using System.Collections.Generic;
using Manager.API.ViewModels;

namespace Manager.API.Utilities{
    //static é uma classe que não precisa ser instanciado (não precisa cria o objeto)
    public static class Responses{
        public static ResultViewModel ApplicationErrorMessage(){
            return new ResultViewModel{
                Message = "Ocorreu algum erro interno. Tente novamente!",
                Success = false,
                Data = null
            };
        }

        public static ResultViewModel DomainErrorMessage(string message){
            return new ResultViewModel{
                Message = message,
                Success = false,
                Data = null
            };
        }

        public static ResultViewModel DomainErrorMessage(string message, IReadOnlyCollection<string> errors){
            return new ResultViewModel{
                Message = message,
                Success = false,
                Data = errors
            };
        }

        public static ResultViewModel UnauthorizedErrorMessage(){
            return new ResultViewModel{
                Message = "Login invalido",
                Success = false,
                Data = null
            };
        }
    }
}