using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MentalCheck.API.Models;

[Table("GS_INSIGHT")]
public class Insight
{
    [Key]
    [Column("ID_INSIGHT")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [Column("ID_USUARIO")]
    public int UsuarioId { get; set; }

    [Required]
    [Column("TIPO")]
    [StringLength(50)]
    public string Tipo { get; set; } = string.Empty;

    [Required]
    [Column("DESCRICAO")]
    [StringLength(500)]
    public string Descricao { get; set; } = string.Empty;

    [Column("DATA_IDENTIFICACAO")]
    public DateTime DataIdentificacao { get; set; } = DateTime.Now;

    [ForeignKey("UsuarioId")]
    public Usuario? Usuario { get; set; }

    public ICollection<InsightDica> InsightDicas { get; set; } = new List<InsightDica>();
}
