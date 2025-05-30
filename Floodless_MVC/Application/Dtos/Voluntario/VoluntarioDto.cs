using System.ComponentModel.DataAnnotations;

namespace Floodless_MVC.Application.Voluntario
{
    public class VoluntarioDto
    {
        [Required(ErrorMessage = $"Campo {nameof(Nome)} é obrigatório")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "O campo deve conter no mínimo 3 caracteres")]
        public string Nome { get; set; }

        [Required(ErrorMessage = $"Campo {nameof(Email)} é obrigatório")]
        [EmailAddress(ErrorMessage = "O email não é válido")]
        public string Email { get; set; }

        [StringLength(150, MinimumLength = 7, ErrorMessage = "O campo deve ter pelo menos 7 caracteres")]
        public string Contato { get; set; }
    }
}
