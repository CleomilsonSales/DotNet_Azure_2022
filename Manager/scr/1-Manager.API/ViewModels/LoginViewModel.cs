using System.ComponentModel.DataAnnotations;

namespace Manager.API.ViewModels{
    public class LoginViewModel{
        [Required(ErrorMessage = "Login obrigatorio")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Senha obrigatoria")]
        public string Password { get; set; }
    }
}