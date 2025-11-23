using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MentalCheck.API.Models;

[Table("GS_DICA")]
public class Dica
{
    [Key]
    [Column("ID_DICA")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [Column("TITULO")]
    [StringLength(50)]
    public string Titulo { get; set; } = string.Empty;

    [Required]
    [Column("DESCRICAO")]
    [StringLength(1000)]
    public string Descricao { get; set; } = string.Empty;

    [Required]
    [Column("CATEGORIA")]
    [StringLength(50)]
    public string Categoria { get; set; } = string.Empty;

    [Column("CONDICAO_APLICACAO")]
    [StringLength(100)]
    public string? CondicaoAplicacao { get; set; }

    public ICollection<InsightDica> InsightDicas { get; set; } = new List<InsightDica>();
}
