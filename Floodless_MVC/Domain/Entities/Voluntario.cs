using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Floodless_MVC.Domain.Entities
{
    [Table("TB_FLOODLESS_VOLUNTARIO")]
    public class Voluntario
    {
        [Key]
        [Column(name: "id_voluntario")]
        public int Id { get; set; }

        [Column(name: "nm_voluntario")]
        [Required(ErrorMessage = "Campo nome é obrigatório!!!")]
        [DisplayName("Nome do Voluntário")]
        public string Nome { get; set; }

        [Column(name: "email_voluntário")]
        [Required]
        public string Email { get; set; }

        [Column(name: "ds_contato")]
        [DisplayName("Número para contato")]
        public string Contato { get; set; } = string.Empty;

        [Column(name: "dt_registro")]
        public DateTime DataRegistro { get; set; } = DateTime.Now;

        //Setando o relacionamento
        public virtual ICollection<Recurso> Recursos { get; set; } = new List<Recurso>();

    }
}
