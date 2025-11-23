using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MentalCheck.API.Models;

[Table("GS_CHECKIN")]
public class Checkin
{
    [Key]
    [Column("ID_CHECKIN")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [Column("ID_USUARIO")]
    public int UsuarioId { get; set; }

    [Column("DATA_CHECKIN")]
    public DateTime DataCheckin { get; set; } = DateTime.Now;

    [Required]
    [Column("NIVEL_ESTRESSE")]
    [Range(1, 10)]
    public int NivelEstresse { get; set; }

    [Required]
    [Column("NIVEL_MOTIVACAO")]
    [Range(1, 10)]
    public int NivelMotivacao { get; set; }

    [Required]
    [Column("NIVEL_CANSACO")]
    [Range(1, 10)]
    public int NivelCansaco { get; set; }

    [Required]
    [Column("NIVEL_SATISFACAO")]
    [Range(1, 10)]
    public int NivelSatisfacao { get; set; }

    [Required]
    [Column("QUALIDADE_SONO")]
    [Range(1, 10)]
    public int QualidadeSono { get; set; }

    [Column("LOCAL_TRABALHO")]
    [StringLength(20)]
    public string? LocalTrabalho { get; set; }

    [Column("OBSERVACAO")]
    [StringLength(500)]
    public string? Observacao { get; set; }

    [ForeignKey("UsuarioId")]
    public Usuario? Usuario { get; set; }
}
