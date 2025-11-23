using System.ComponentModel.DataAnnotations;

namespace MentalCheck.API.DTOs;

public class CreateCheckinDto
{
    [Required(ErrorMessage = "ID do usuário é obrigatório")]
    public int UsuarioId { get; set; }

    [Required(ErrorMessage = "Nível de estresse é obrigatório")]
    [Range(1, 10, ErrorMessage = "Nível de estresse deve estar entre 1 e 10")]
    public int NivelEstresse { get; set; }

    [Required(ErrorMessage = "Nível de motivação é obrigatório")]
    [Range(1, 10, ErrorMessage = "Nível de motivação deve estar entre 1 e 10")]
    public int NivelMotivacao { get; set; }

    [Required(ErrorMessage = "Nível de cansaço é obrigatório")]
    [Range(1, 10, ErrorMessage = "Nível de cansaço deve estar entre 1 e 10")]
    public int NivelCansaco { get; set; }

    [Required(ErrorMessage = "Nível de satisfação é obrigatório")]
    [Range(1, 10, ErrorMessage = "Nível de satisfação deve estar entre 1 e 10")]
    public int NivelSatisfacao { get; set; }

    [Required(ErrorMessage = "Qualidade do sono é obrigatória")]
    [Range(1, 10, ErrorMessage = "Qualidade do sono deve estar entre 1 e 10")]
    public int QualidadeSono { get; set; }

    [StringLength(20, ErrorMessage = "Local de trabalho deve ter no máximo 20 caracteres")]
    public string? LocalTrabalho { get; set; }

    [StringLength(500, ErrorMessage = "Observação deve ter no máximo 500 caracteres")]
    public string? Observacao { get; set; }
}
