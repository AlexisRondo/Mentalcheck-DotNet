using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MentalCheck.API.Models;

[Table("GS_INSIGHT_DICA")]
public class InsightDica
{
    [Key]
    [Column("ID_INSIGHT_DICA")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [Column("ID_INSIGHT")]
    public int InsightId { get; set; }

    [Required]
    [Column("ID_DICA")]
    public int DicaId { get; set; }

    [Column("DATA_RECOMENDACAO")]
    public DateTime DataRecomendacao { get; set; } = DateTime.Now;

    [Column("STATUS_VISUALIZACAO")]
    [StringLength(20)]
    public string StatusVisualizacao { get; set; } = "PENDENTE";

    [ForeignKey("InsightId")]
    public Insight? Insight { get; set; }

    [ForeignKey("DicaId")]
    public Dica? Dica { get; set; }
}
