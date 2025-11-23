namespace MentalCheck.API.DTOs;

public class DicaDto
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public string? CondicaoAplicacao { get; set; }
    public List<LinkDto> Links { get; set; } = new();
}
