using System;
using System.Collections.Generic;
using Manager.Domain.Validators;

namespace Manager.Domain.Entities{
    public class User: Base{
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        
        //encapsulando, a ententidade fica fechado somente sendo alterada por metodos ou criada por construtores, por isso o private
        public User(string name, string email, string password){
            Name = name;
            Email = email;
            Password = password;
            //instanciando lista de erro vazio
            _errors = new List<string>();
        }

        //EF - Para uso do Entity Framework
        public User(){}
    
        public void ChangeName(string name) { 
            Name = name;
            Validate();
        }

        public void ChangeEmail(string email) { 
            Email = email;
            Validate();
        }

        public void ChangePassword(string password) { 
            Password = password;
            Validate();
        }

        public override bool Validate(){
            var validator = new UserValidator();
            //relembrando que o this quer dizer que estou passando essa classe para o metodo
            var validation = validator.Validate(this);

            if(!validation.IsValid){
                foreach (var error in validation.Errors){
                    _errors.Add(error.ErrorMessage);

                    //Retornando apenas o primeiro erro
                    throw new Exception("Erro interno na camada de Dominio: " + _errors[0]);
                }
            }
            
            return true;
        }
    }
}