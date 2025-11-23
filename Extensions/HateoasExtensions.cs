using MentalCheck.API.DTOs;

namespace MentalCheck.API.Extensions;

public static class HateoasExtensions
{
    public static void AddLink(this List<LinkDto> links, string rel, string href, string method)
    {
        links.Add(new LinkDto { Rel = rel, Href = href, Method = method });
    }

    public static List<LinkDto> CreateUsuarioLinks(int id)
    {
        var links = new List<LinkDto>();
        links.AddLink("self", $"/api/usuarios/{id}", "GET");
        links.AddLink("update", $"/api/usuarios/{id}", "PUT");
        links.AddLink("delete", $"/api/usuarios/{id}", "DELETE");
        links.AddLink("checkins", $"/api/checkins?usuarioId={id}", "GET");
        links.AddLink("all-usuarios", "/api/usuarios", "GET");
        return links;
    }

    public static List<LinkDto> CreateCheckinLinks(int id)
    {
        var links = new List<LinkDto>();
        links.AddLink("self", $"/api/checkins/{id}", "GET");
        links.AddLink("update", $"/api/checkins/{id}", "PUT");
        links.AddLink("delete", $"/api/checkins/{id}", "DELETE");
        links.AddLink("all-checkins", "/api/checkins", "GET");
        return links;
    }

    public static List<LinkDto> CreateDicaLinks(int id)
    {
        var links = new List<LinkDto>();
        links.AddLink("self", $"/api/dicas/{id}", "GET");
        links.AddLink("update", $"/api/dicas/{id}", "PUT");
        links.AddLink("delete", $"/api/dicas/{id}", "DELETE");
        links.AddLink("all-dicas", "/api/dicas", "GET");
        return links;
    }

    public static List<LinkDto> CreatePaginationLinks(string baseUrl, int page, int pageSize, int totalPages)
    {
        var links = new List<LinkDto>();
        links.AddLink("self", $"{baseUrl}?page={page}&pageSize={pageSize}", "GET");
        
        if (page > 1)
            links.AddLink("previous", $"{baseUrl}?page={page - 1}&pageSize={pageSize}", "GET");
        
        if (page < totalPages)
            links.AddLink("next", $"{baseUrl}?page={page + 1}&pageSize={pageSize}", "GET");
        
        links.AddLink("first", $"{baseUrl}?page=1&pageSize={pageSize}", "GET");
        links.AddLink("last", $"{baseUrl}?page={totalPages}&pageSize={pageSize}", "GET");
        
        return links;
    }
}
