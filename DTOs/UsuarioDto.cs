namespace MentalCheck.API.DTOs;

public class UsuarioDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Cargo { get; set; }
    public string? ModalidadeTrabalho { get; set; }
    public DateTime DataCadastro { get; set; }
    public List<LinkDto> Links { get; set; } = new();
}
