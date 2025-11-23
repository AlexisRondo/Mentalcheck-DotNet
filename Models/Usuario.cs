using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MentalCheck.API.Models;

[Table("GS_USUARIO")]
public class Usuario
{
    [Key]
    [Column("ID_USUARIO")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [Column("NOME")]
    [StringLength(100)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [Column("EMAIL")]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Column("SENHA")]
    [StringLength(200)]
    public string Senha { get; set; } = string.Empty;

    [Column("CARGO")]
    [StringLength(50)]
    public string? Cargo { get; set; }

    [Column("MODALIDADE_TRABALHO")]
    [StringLength(20)]
    public string? ModalidadeTrabalho { get; set; }

    [Column("DATA_CADASTRO")]
    public DateTime DataCadastro { get; set; } = DateTime.Now;

    public ICollection<Checkin> Checkins { get; set; } = new List<Checkin>();
    public ICollection<Insight> Insights { get; set; } = new List<Insight>();
}
