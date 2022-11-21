using System.ComponentModel.DataAnnotations;

namespace Manager.API.ViewModels{
    public class CreateUserViewModel{
        
        [Required(ErrorMessage="Informe o nome")]
        [MinLength(3, ErrorMessage="No minimo 3 caracteres")]
        [MaxLength(80, ErrorMessage="No maximo 80 caracteres")]
        public string Name { get; set; }

        [Required(ErrorMessage="Informe o email")]
        [MinLength(10, ErrorMessage="No minimo 10 caracteres")]
        [MaxLength(180, ErrorMessage="No maximo 80 caracteres")]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", 
                        ErrorMessage="Email não é valido")]
        public string Email { get; set; }
        
        [Required(ErrorMessage="Informe a senha")]
        [MinLength(10, ErrorMessage="No minimo 10 caracteres")]
        [MaxLength(30, ErrorMessage="No maximo 30 caracteres")]
        public string Password { get; set; }

    }
}