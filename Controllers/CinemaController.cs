using AutoMapper;
using FilmesAPI.Data;
using FilmesAPI.Data.Dtos;
using FilmesAPI.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace FilmesAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CinemaController : ControllerBase
{
    //o controler agora depende do Context pra poder funcionar(injeção de dependência)
    private FilmeContext _context;
    private IMapper _mapper;
  

    public CinemaController(FilmeContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Adiciona um filme ao banco de dados
    /// </summary>
    /// <param name="filmeDto">Objeto com os campos necessários para criação de um filme</param>
    /// <returns>IActionResult</returns>
    /// <response code="201">Caso inserção seja feita com sucesso</response>


    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult AdicionaCinema([FromBody] CreateCinemaDto cinemaDto)
    {
        Cinema cinema = _mapper.Map<Cinema>(cinemaDto);
        _context.Cinemas.Add(cinema);
        _context.SaveChanges();
        return CreatedAtAction(nameof(RecuperaCinemaPorId), new { id = cinema.Id}, cinemaDto);

    }
     
    [HttpGet]
    //como List implementa IEnumerable(nesse caso nao ta implementando mas no do prof apareceu no ctrl click de List) colocamos o metodo como IEnumerable pra se trocarmos nossa classe pra uma que tbm implementa o IEnumerable(interface) nao precisamos trocar o cabeçalho do método
    //estamos deixando mais abstrato possivel. dependendo menos de classes concretas
    public IEnumerable<ReadCinemaDto> RecuperaCinemas([FromQuery]int skip = 0, [FromQuery] int take = 50,[FromQuery] int? enderecoId =null)
    {
        if(enderecoId == null)
        {
            return _mapper.Map<List<ReadCinemaDto>>(_context.Cinemas.Skip(skip).Take(take).Include(cinema => cinema.Endereco)).ToList();

        }
        //fazendo consulta usando código SQL.
        return _mapper.Map<List<ReadCinemaDto>>(_context.Cinemas.FromSqlRaw($"SELECT Id, Nome, EnderecoId FROM cinemas WHERE cinemas.EnderecoId = {enderecoId}").ToList());
    }

    [HttpGet("{id}")]
    //esse ponto de interrogação informa que o tipo de retorno pode retornar nulo ou não
    //public Filme? RecuperaFilmePorId([FromBody] int id)
    public IActionResult RecuperaCinemaPorId([FromBody] int id)
    {
        //retorna o primeiro elemento em que o Id da lista seja o mesmo do id do parametro passado
        var cinema =  _context.Filmes.FirstOrDefault(cinema => cinema.Id ==id);
        if (cinema == null) return NotFound();
        var cinemaDto = _mapper.Map<ReadCinemaDto>(cinema);
        return Ok(cinemaDto);
    }

    [HttpPut("{id}")]
    public IActionResult AtualizaCinema(int id, [FromBody] UpdateCinemaDto cinemaDto)
    {
        var cinema = _context.Cinemas.FirstOrDefault(cinema => cinema.Id == id);
        if(cinema == null) return NotFound();
        _mapper.Map(cinemaDto, cinema);
        _context.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeletaCinema (int id)
    {
        var cinema = _context.Cinemas.FirstOrDefault(cinema => cinema.Id == id);
        if (cinema == null) return NotFound();
        _context.Remove(cinema);
        _context.SaveChanges();
        return NoContent();
    }

    
}
