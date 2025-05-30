using Floodless_MVC.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Floodless_MVC.Application.Dtos.Recurso
{
    public class RecursoDto
    {
        [Required(ErrorMessage = $"Campo {nameof(Nome)} é obrigatório")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "O campo deve conter no mínimo 3 caracteres")]
        public string Nome { get; set; }

        [Required(ErrorMessage = $"Campo {nameof(TipoRecurso)} é obrigatório")]
        public TipoRecurso TipoRecurso { get; set; }

        [Required(ErrorMessage = $"Campo {nameof(Quantidade)} é obrigatório")]
        public int Quantidade { get; set; }

        public int VoluntarioId { get; set; }
    }
}
