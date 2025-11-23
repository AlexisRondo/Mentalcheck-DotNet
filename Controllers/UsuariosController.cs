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
public class UsuariosController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsuariosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<UsuarioDto>>> GetAll()
    {
        var usuarios = await _context.Usuarios.ToListAsync();
        
        var dtos = usuarios.Select(u => new UsuarioDto
        {
            Id = u.Id,
            Nome = u.Nome,
            Email = u.Email,
            Cargo = u.Cargo,
            ModalidadeTrabalho = u.ModalidadeTrabalho,
            DataCadastro = u.DataCadastro,
            Links = HateoasExtensions.CreateUsuarioLinks(u.Id)
        }).ToList();

        return Ok(dtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UsuarioDto>> GetById(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);

        if (usuario == null)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Usuário não encontrado",
                Detail = $"Não existe usuário com ID {id}",
                Status = StatusCodes.Status404NotFound,
                Instance = HttpContext.Request.Path
            });
        }

        var dto = new UsuarioDto
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            Email = usuario.Email,
            Cargo = usuario.Cargo,
            ModalidadeTrabalho = usuario.ModalidadeTrabalho,
            DataCadastro = usuario.DataCadastro,
            Links = HateoasExtensions.CreateUsuarioLinks(usuario.Id)
        };

        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<UsuarioDto>> Create([FromBody] CreateUsuarioDto dto)
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

        var emailExiste = await _context.Usuarios.AnyAsync(u => u.Email == dto.Email);
        if (emailExiste)
        {
            return Conflict(new ProblemDetails
            {
                Title = "Email já cadastrado",
                Detail = $"O email {dto.Email} já está em uso",
                Status = StatusCodes.Status409Conflict,
                Instance = HttpContext.Request.Path
            });
        }

        var usuario = new Usuario
        {
            Nome = dto.Nome,
            Email = dto.Email,
            Senha = dto.Senha,
            Cargo = dto.Cargo,
            ModalidadeTrabalho = dto.ModalidadeTrabalho,
            DataCadastro = DateTime.Now
        };

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        var responseDto = new UsuarioDto
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            Email = usuario.Email,
            Cargo = usuario.Cargo,
            ModalidadeTrabalho = usuario.ModalidadeTrabalho,
            DataCadastro = usuario.DataCadastro,
            Links = HateoasExtensions.CreateUsuarioLinks(usuario.Id)
        };

        return CreatedAtAction(nameof(GetById), new { id = usuario.Id }, responseDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<UsuarioDto>> Update(int id, [FromBody] UpdateUsuarioDto dto)
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

        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Usuário não encontrado",
                Detail = $"Não existe usuário com ID {id}",
                Status = StatusCodes.Status404NotFound,
                Instance = HttpContext.Request.Path
            });
        }

        var emailExiste = await _context.Usuarios.AnyAsync(u => u.Email == dto.Email && u.Id != id);
        if (emailExiste)
        {
            return Conflict(new ProblemDetails
            {
                Title = "Email já cadastrado",
                Detail = $"O email {dto.Email} já está em uso por outro usuário",
                Status = StatusCodes.Status409Conflict,
                Instance = HttpContext.Request.Path
            });
        }

        usuario.Nome = dto.Nome;
        usuario.Email = dto.Email;
        usuario.Cargo = dto.Cargo;
        usuario.ModalidadeTrabalho = dto.ModalidadeTrabalho;

        await _context.SaveChangesAsync();

        var responseDto = new UsuarioDto
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            Email = usuario.Email,
            Cargo = usuario.Cargo,
            ModalidadeTrabalho = usuario.ModalidadeTrabalho,
            DataCadastro = usuario.DataCadastro,
            Links = HateoasExtensions.CreateUsuarioLinks(usuario.Id)
        };

        return Ok(responseDto);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Usuário não encontrado",
                Detail = $"Não existe usuário com ID {id}",
                Status = StatusCodes.Status404NotFound,
                Instance = HttpContext.Request.Path
            });
        }

        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("search")]
    public async Task<ActionResult<PagedResultDto<UsuarioDto>>> Search(
        [FromQuery] string? nome,
        [FromQuery] string? email,
        [FromQuery] string? cargo,
        [FromQuery] string? modalidadeTrabalho,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string orderBy = "nome",
        [FromQuery] string direction = "asc")
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var query = _context.Usuarios.AsQueryable();

        if (!string.IsNullOrWhiteSpace(nome))
            query = query.Where(u => u.Nome.Contains(nome));

        if (!string.IsNullOrWhiteSpace(email))
            query = query.Where(u => u.Email.Contains(email));

        if (!string.IsNullOrWhiteSpace(cargo))
            query = query.Where(u => u.Cargo != null && u.Cargo.Contains(cargo));

        if (!string.IsNullOrWhiteSpace(modalidadeTrabalho))
            query = query.Where(u => u.ModalidadeTrabalho == modalidadeTrabalho);

        query = orderBy.ToLower() switch
        {
            "email" => direction.ToLower() == "desc" ? query.OrderByDescending(u => u.Email) : query.OrderBy(u => u.Email),
            "cargo" => direction.ToLower() == "desc" ? query.OrderByDescending(u => u.Cargo) : query.OrderBy(u => u.Cargo),
            "datacadastro" => direction.ToLower() == "desc" ? query.OrderByDescending(u => u.DataCadastro) : query.OrderBy(u => u.DataCadastro),
            _ => direction.ToLower() == "desc" ? query.OrderByDescending(u => u.Nome) : query.OrderBy(u => u.Nome)
        };

        var totalItems = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        var usuarios = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var dtos = usuarios.Select(u => new UsuarioDto
        {
            Id = u.Id,
            Nome = u.Nome,
            Email = u.Email,
            Cargo = u.Cargo,
            ModalidadeTrabalho = u.ModalidadeTrabalho,
            DataCadastro = u.DataCadastro,
            Links = HateoasExtensions.CreateUsuarioLinks(u.Id)
        }).ToList();

        var result = new PagedResultDto<UsuarioDto>
        {
            Items = dtos,
            Page = page,
            PageSize = pageSize,
            TotalItems = totalItems,
            TotalPages = totalPages,
            Links = HateoasExtensions.CreatePaginationLinks("/api/usuarios/search", page, pageSize, totalPages)
        };

        return Ok(result);
    }
}
