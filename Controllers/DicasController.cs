using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MentalCheck.API.Data;
using MentalCheck.API.DTOs;
using MentalCheck.API.Models;
using MentalCheck.API.Extensions;

namespace MentalCheck.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class DicasController : ControllerBase
{
    private readonly AppDbContext _context;

    public DicasController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<DicaDto>>> GetAll()
    {
        var dicas = await _context.Dicas.ToListAsync();

        var dtos = dicas.Select(d => new DicaDto
        {
            Id = d.Id,
            Titulo = d.Titulo,
            Descricao = d.Descricao,
            Categoria = d.Categoria,
            CondicaoAplicacao = d.CondicaoAplicacao,
            Links = HateoasExtensions.CreateDicaLinks(d.Id)
        }).ToList();

        return Ok(dtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DicaDto>> GetById(int id)
    {
        var dica = await _context.Dicas.FindAsync(id);

        if (dica == null)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Dica não encontrada",
                Detail = $"Não existe dica com ID {id}",
                Status = StatusCodes.Status404NotFound,
                Instance = HttpContext.Request.Path
            });
        }

        var dto = new DicaDto
        {
            Id = dica.Id,
            Titulo = dica.Titulo,
            Descricao = dica.Descricao,
            Categoria = dica.Categoria,
            CondicaoAplicacao = dica.CondicaoAplicacao,
            Links = HateoasExtensions.CreateDicaLinks(dica.Id)
        };

        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<DicaDto>> Create([FromBody] CreateDicaDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ValidationProblemDetails(ModelState)
            {
                Title = "Erro de validação",
                Status = StatusCodes.Status400BadRequest,
                Instance = HttpContext.Request.Path
            });
        }

        var dica = new Dica
        {
            Titulo = dto.Titulo,
            Descricao = dto.Descricao,
            Categoria = dto.Categoria,
            CondicaoAplicacao = dto.CondicaoAplicacao
        };

        _context.Dicas.Add(dica);
        await _context.SaveChangesAsync();

        var responseDto = new DicaDto
        {
            Id = dica.Id,
            Titulo = dica.Titulo,
            Descricao = dica.Descricao,
            Categoria = dica.Categoria,
            CondicaoAplicacao = dica.CondicaoAplicacao,
            Links = HateoasExtensions.CreateDicaLinks(dica.Id)
        };

        return CreatedAtAction(nameof(GetById), new { id = dica.Id }, responseDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<DicaDto>> Update(int id, [FromBody] UpdateDicaDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ValidationProblemDetails(ModelState)
            {
                Title = "Erro de validação",
                Status = StatusCodes.Status400BadRequest,
                Instance = HttpContext.Request.Path
            });
        }

        var dica = await _context.Dicas.FindAsync(id);
        if (dica == null)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Dica não encontrada",
                Detail = $"Não existe dica com ID {id}",
                Status = StatusCodes.Status404NotFound,
                Instance = HttpContext.Request.Path
            });
        }

        dica.Titulo = dto.Titulo;
        dica.Descricao = dto.Descricao;
        dica.Categoria = dto.Categoria;
        dica.CondicaoAplicacao = dto.CondicaoAplicacao;

        await _context.SaveChangesAsync();

        var responseDto = new DicaDto
        {
            Id = dica.Id,
            Titulo = dica.Titulo,
            Descricao = dica.Descricao,
            Categoria = dica.Categoria,
            CondicaoAplicacao = dica.CondicaoAplicacao,
            Links = HateoasExtensions.CreateDicaLinks(dica.Id)
        };

        return Ok(responseDto);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var dica = await _context.Dicas.FindAsync(id);
        if (dica == null)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Dica não encontrada",
                Detail = $"Não existe dica com ID {id}",
                Status = StatusCodes.Status404NotFound,
                Instance = HttpContext.Request.Path
            });
        }

        _context.Dicas.Remove(dica);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("search")]
    public async Task<ActionResult<PagedResultDto<DicaDto>>> Search(
        [FromQuery] string? titulo,
        [FromQuery] string? categoria,
        [FromQuery] string? descricao,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string orderBy = "titulo",
        [FromQuery] string direction = "asc")
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var query = _context.Dicas.AsQueryable();

        if (!string.IsNullOrWhiteSpace(titulo))
            query = query.Where(d => d.Titulo.Contains(titulo));

        if (!string.IsNullOrWhiteSpace(categoria))
            query = query.Where(d => d.Categoria.Contains(categoria));

        if (!string.IsNullOrWhiteSpace(descricao))
            query = query.Where(d => d.Descricao.Contains(descricao));

        query = orderBy.ToLower() switch
        {
            "categoria" => direction.ToLower() == "desc" ? query.OrderByDescending(d => d.Categoria) : query.OrderBy(d => d.Categoria),
            "id" => direction.ToLower() == "desc" ? query.OrderByDescending(d => d.Id) : query.OrderBy(d => d.Id),
            _ => direction.ToLower() == "desc" ? query.OrderByDescending(d => d.Titulo) : query.OrderBy(d => d.Titulo)
        };

        var totalItems = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        var dicas = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var dtos = dicas.Select(d => new DicaDto
        {
            Id = d.Id,
            Titulo = d.Titulo,
            Descricao = d.Descricao,
            Categoria = d.Categoria,
            CondicaoAplicacao = d.CondicaoAplicacao,
            Links = HateoasExtensions.CreateDicaLinks(d.Id)
        }).ToList();

        var result = new PagedResultDto<DicaDto>
        {
            Items = dtos,
            Page = page,
            PageSize = pageSize,
            TotalItems = totalItems,
            TotalPages = totalPages,
            Links = HateoasExtensions.CreatePaginationLinks("/api/dicas/search", page, pageSize, totalPages)
        };

        return Ok(result);
    }
}
