using System.ComponentModel.DataAnnotations;

namespace MentalCheck.API.DTOs;

public class CreateUsuarioDto
{
    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    [StringLength(100, ErrorMessage = "Email deve ter no máximo 100 caracteres")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Senha é obrigatória")]
    [StringLength(200, MinimumLength = 6, ErrorMessage = "Senha deve ter entre 6 e 200 caracteres")]
    public string Senha { get; set; } = string.Empty;

    [StringLength(50, ErrorMessage = "Cargo deve ter no máximo 50 caracteres")]
    public string? Cargo { get; set; }

    [StringLength(20, ErrorMessage = "Modalidade de trabalho deve ter no máximo 20 caracteres")]
    public string? ModalidadeTrabalho { get; set; }
}
