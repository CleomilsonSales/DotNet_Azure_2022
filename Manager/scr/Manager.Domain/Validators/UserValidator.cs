using FluentValidation;
using Manager.Domain.Entities;

namespace Manager.Domain.Validators{
    public class UserValidator: AbstractValidator<User>{
        public UserValidator(){
            //x => x quer dizer que é para entidade inteira
            RuleFor(x => x)
                .NotEmpty()
                .WithMessage("A entidade não pode ser vazia")
                
                .NotNull()
                .WithMessage("A entidade não pode ser nula");

            RuleFor(x => x.Name)
                .NotNull()
                .WithMessage("O nome não pode ser nulo")

                .NotEmpty()
                .WithMessage("O nome não pode ser vazio")

                .MinimumLength(3)
                .WithMessage("Minimo de 3 caracteres")

                .MaximumLength(80)
                .WithMessage("Máximo de 80 caracteres");

            RuleFor(x => x.Email)
                .NotNull()
                .WithMessage("Email não pode ser nulo")

                .NotEmpty()
                .WithMessage("Email não pode ser vazio")

                .MinimumLength(10)
                .WithMessage("Minimo de 10 caracteres")

                .MaximumLength(180)
                .WithMessage("Máximo de 180 caracteres")

                .Matches(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$")
                .WithMessage("Email não é valido");

            RuleFor(x => x.Password)
                .NotNull()
                .WithMessage("Senha não pode ser nula")

                .NotEmpty()
                .WithMessage("Senha não pode ser vazia")

                .MinimumLength(6)
                .WithMessage("Minimo de 6 caracteres")

                .MaximumLength(30)
                .WithMessage("Máximo de 30 caracteres");
                
        }
    }
}