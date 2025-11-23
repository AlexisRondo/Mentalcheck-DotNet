namespace MentalCheck.API.DTOs;

public class CheckinDto
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public string? NomeUsuario { get; set; }
    public DateTime DataCheckin { get; set; }
    public int NivelEstresse { get; set; }
    public int NivelMotivacao { get; set; }
    public int NivelCansaco { get; set; }
    public int NivelSatisfacao { get; set; }
    public int QualidadeSono { get; set; }
    public string? LocalTrabalho { get; set; }
    public string? Observacao { get; set; }
    public List<LinkDto> Links { get; set; } = new();
}
