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
public class CheckinsController : ControllerBase
{
    private readonly AppDbContext _context;

    public CheckinsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<CheckinDto>>> GetAll()
    {
        var checkins = await _context.Checkins
            .Include(c => c.Usuario)
            .ToListAsync();

        var dtos = checkins.Select(c => new CheckinDto
        {
            Id = c.Id,
            UsuarioId = c.UsuarioId,
            NomeUsuario = c.Usuario?.Nome,
            DataCheckin = c.DataCheckin,
            NivelEstresse = c.NivelEstresse,
            NivelMotivacao = c.NivelMotivacao,
            NivelCansaco = c.NivelCansaco,
            NivelSatisfacao = c.NivelSatisfacao,
            QualidadeSono = c.QualidadeSono,
            LocalTrabalho = c.LocalTrabalho,
            Observacao = c.Observacao,
            Links = HateoasExtensions.CreateCheckinLinks(c.Id)
        }).ToList();

        return Ok(dtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CheckinDto>> GetById(int id)
    {
        var checkin = await _context.Checkins
            .Include(c => c.Usuario)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (checkin == null)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Check-in não encontrado",
                Detail = $"Não existe check-in com ID {id}",
                Status = StatusCodes.Status404NotFound,
                Instance = HttpContext.Request.Path
            });
        }

        var dto = new CheckinDto
        {
            Id = checkin.Id,
            UsuarioId = checkin.UsuarioId,
            NomeUsuario = checkin.Usuario?.Nome,
            DataCheckin = checkin.DataCheckin,
            NivelEstresse = checkin.NivelEstresse,
            NivelMotivacao = checkin.NivelMotivacao,
            NivelCansaco = checkin.NivelCansaco,
            NivelSatisfacao = checkin.NivelSatisfacao,
            QualidadeSono = checkin.QualidadeSono,
            LocalTrabalho = checkin.LocalTrabalho,
            Observacao = checkin.Observacao,
            Links = HateoasExtensions.CreateCheckinLinks(checkin.Id)
        };

        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<CheckinDto>> Create([FromBody] CreateCheckinDto dto)
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

        var usuarioExiste = await _context.Usuarios.AnyAsync(u => u.Id == dto.UsuarioId);
        if (!usuarioExiste)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Usuário não encontrado",
                Detail = $"Não existe usuário com ID {dto.UsuarioId}",
                Status = StatusCodes.Status404NotFound,
                Instance = HttpContext.Request.Path
            });
        }

        var checkin = new Checkin
        {
            UsuarioId = dto.UsuarioId,
            DataCheckin = DateTime.Now,
            NivelEstresse = dto.NivelEstresse,
            NivelMotivacao = dto.NivelMotivacao,
            NivelCansaco = dto.NivelCansaco,
            NivelSatisfacao = dto.NivelSatisfacao,
            QualidadeSono = dto.QualidadeSono,
            LocalTrabalho = dto.LocalTrabalho,
            Observacao = dto.Observacao
        };

        _context.Checkins.Add(checkin);
        await _context.SaveChangesAsync();

        var usuario = await _context.Usuarios.FindAsync(dto.UsuarioId);

        var responseDto = new CheckinDto
        {
            Id = checkin.Id,
            UsuarioId = checkin.UsuarioId,
            NomeUsuario = usuario?.Nome,
            DataCheckin = checkin.DataCheckin,
            NivelEstresse = checkin.NivelEstresse,
            NivelMotivacao = checkin.NivelMotivacao,
            NivelCansaco = checkin.NivelCansaco,
            NivelSatisfacao = checkin.NivelSatisfacao,
            QualidadeSono = checkin.QualidadeSono,
            LocalTrabalho = checkin.LocalTrabalho,
            Observacao = checkin.Observacao,
            Links = HateoasExtensions.CreateCheckinLinks(checkin.Id)
        };

        return CreatedAtAction(nameof(GetById), new { id = checkin.Id }, responseDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CheckinDto>> Update(int id, [FromBody] UpdateCheckinDto dto)
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

        var checkin = await _context.Checkins
            .Include(c => c.Usuario)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (checkin == null)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Check-in não encontrado",
                Detail = $"Não existe check-in com ID {id}",
                Status = StatusCodes.Status404NotFound,
                Instance = HttpContext.Request.Path
            });
        }

        checkin.NivelEstresse = dto.NivelEstresse;
        checkin.NivelMotivacao = dto.NivelMotivacao;
        checkin.NivelCansaco = dto.NivelCansaco;
        checkin.NivelSatisfacao = dto.NivelSatisfacao;
        checkin.QualidadeSono = dto.QualidadeSono;
        checkin.LocalTrabalho = dto.LocalTrabalho;
        checkin.Observacao = dto.Observacao;

        await _context.SaveChangesAsync();

        var responseDto = new CheckinDto
        {
            Id = checkin.Id,
            UsuarioId = checkin.UsuarioId,
            NomeUsuario = checkin.Usuario?.Nome,
            DataCheckin = checkin.DataCheckin,
            NivelEstresse = checkin.NivelEstresse,
            NivelMotivacao = checkin.NivelMotivacao,
            NivelCansaco = checkin.NivelCansaco,
            NivelSatisfacao = checkin.NivelSatisfacao,
            QualidadeSono = checkin.QualidadeSono,
            LocalTrabalho = checkin.LocalTrabalho,
            Observacao = checkin.Observacao,
            Links = HateoasExtensions.CreateCheckinLinks(checkin.Id)
        };

        return Ok(responseDto);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var checkin = await _context.Checkins.FindAsync(id);
        if (checkin == null)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Check-in não encontrado",
                Detail = $"Não existe check-in com ID {id}",
                Status = StatusCodes.Status404NotFound,
                Instance = HttpContext.Request.Path
            });
        }

        _context.Checkins.Remove(checkin);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("search")]
    public async Task<ActionResult<PagedResultDto<CheckinDto>>> Search(
        [FromQuery] int? usuarioId,
        [FromQuery] DateTime? dataInicio,
        [FromQuery] DateTime? dataFim,
        [FromQuery] int? nivelEstresseMin,
        [FromQuery] int? nivelEstresseMax,
        [FromQuery] string? localTrabalho,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string orderBy = "datacheckin",
        [FromQuery] string direction = "desc")
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var query = _context.Checkins.Include(c => c.Usuario).AsQueryable();

        if (usuarioId.HasValue)
            query = query.Where(c => c.UsuarioId == usuarioId.Value);

        if (dataInicio.HasValue)
            query = query.Where(c => c.DataCheckin >= dataInicio.Value);

        if (dataFim.HasValue)
            query = query.Where(c => c.DataCheckin <= dataFim.Value);

        if (nivelEstresseMin.HasValue)
            query = query.Where(c => c.NivelEstresse >= nivelEstresseMin.Value);

        if (nivelEstresseMax.HasValue)
            query = query.Where(c => c.NivelEstresse <= nivelEstresseMax.Value);

        if (!string.IsNullOrWhiteSpace(localTrabalho))
            query = query.Where(c => c.LocalTrabalho == localTrabalho);

        query = orderBy.ToLower() switch
        {
            "nivelestresse" => direction.ToLower() == "desc" ? query.OrderByDescending(c => c.NivelEstresse) : query.OrderBy(c => c.NivelEstresse),
            "nivelmotivacao" => direction.ToLower() == "desc" ? query.OrderByDescending(c => c.NivelMotivacao) : query.OrderBy(c => c.NivelMotivacao),
            "nivelcansaco" => direction.ToLower() == "desc" ? query.OrderByDescending(c => c.NivelCansaco) : query.OrderBy(c => c.NivelCansaco),
            "nivelsatisfacao" => direction.ToLower() == "desc" ? query.OrderByDescending(c => c.NivelSatisfacao) : query.OrderBy(c => c.NivelSatisfacao),
            _ => direction.ToLower() == "desc" ? query.OrderByDescending(c => c.DataCheckin) : query.OrderBy(c => c.DataCheckin)
        };

        var totalItems = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        var checkins = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var dtos = checkins.Select(c => new CheckinDto
        {
            Id = c.Id,
            UsuarioId = c.UsuarioId,
            NomeUsuario = c.Usuario?.Nome,
            DataCheckin = c.DataCheckin,
            NivelEstresse = c.NivelEstresse,
            NivelMotivacao = c.NivelMotivacao,
            NivelCansaco = c.NivelCansaco,
            NivelSatisfacao = c.NivelSatisfacao,
            QualidadeSono = c.QualidadeSono,
            LocalTrabalho = c.LocalTrabalho,
            Observacao = c.Observacao,
            Links = HateoasExtensions.CreateCheckinLinks(c.Id)
        }).ToList();

        var result = new PagedResultDto<CheckinDto>
        {
            Items = dtos,
            Page = page,
            PageSize = pageSize,
            TotalItems = totalItems,
            TotalPages = totalPages,
            Links = HateoasExtensions.CreatePaginationLinks("/api/checkins/search", page, pageSize, totalPages)
        };

        return Ok(result);
    }
}
