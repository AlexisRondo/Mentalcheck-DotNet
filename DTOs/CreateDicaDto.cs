using System.ComponentModel.DataAnnotations;

namespace MentalCheck.API.DTOs;

public class CreateDicaDto
{
    [Required(ErrorMessage = "Título é obrigatório")]
    [StringLength(50, ErrorMessage = "Título deve ter no máximo 50 caracteres")]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "Descrição é obrigatória")]
    [StringLength(1000, ErrorMessage = "Descrição deve ter no máximo 1000 caracteres")]
    public string Descricao { get; set; } = string.Empty;

    [Required(ErrorMessage = "Categoria é obrigatória")]
    [StringLength(50, ErrorMessage = "Categoria deve ter no máximo 50 caracteres")]
    public string Categoria { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "Condição de aplicação deve ter no máximo 100 caracteres")]
    public string? CondicaoAplicacao { get; set; }
}
