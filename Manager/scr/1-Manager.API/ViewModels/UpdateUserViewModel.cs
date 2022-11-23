using System.ComponentModel.DataAnnotations;

namespace Manager.API.ViewModels{
    public class UpdateUserViewModel{

        [Required(ErrorMessage="Informe o id")]
        [Range(1, int.MaxValue, ErrorMessage="No minimo 1 numero")]
        public int Id {get; set; }
        
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
        [MinLength(6, ErrorMessage="No minimo 6 caracteres")]
        [MaxLength(80, ErrorMessage="No maximo 80 caracteres")]
        public string Password { get; set; }

    }
}