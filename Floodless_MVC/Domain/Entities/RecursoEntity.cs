using Floodless_MVC.Domain.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Floodless_MVC.Domain.Entities
{
    [Table("TB_FLOODLESS_RECURSO")]
    public class RecursoEntity
    {
        [Key]
        [Column("id_recurso")]
        public int Id { get; set; }

        [Column("nm_recurso")]
        [Required]
        [DisplayName("Nome do recurso")]
        public string Nome { get; set; }

        [Column("tp_recurso")]
        [DisplayName("Tipo de recurso")]
        [Required]
        public TipoRecurso TipoRecurso { get; set; }

        [Column("qt_recurso")]
        [Required]
        public int Quantidade { get; set; }

        [Column("dt_criacao")]
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        //Chave estrangeira
        [ForeignKey(nameof(RecursoEntity))]
        public int VoluntarioId { get; set; }
        public virtual VoluntarioEntity Voluntario { get; set; }

    }
}
