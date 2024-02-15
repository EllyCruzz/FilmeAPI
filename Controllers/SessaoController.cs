using AutoMapper;
using FilmesAPI.Data;
using FilmesAPI.Data.Dtos;
using FilmesAPI.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace FilmesAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class SessaoController : ControllerBase
{
    //o controler agora depende do Context pra poder funcionar(injeção de dependência)
    private FilmeContext _context;
    private IMapper _mapper;
  

    public SessaoController(FilmeContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Adiciona um Sessao ao banco de dados
    /// </summary>
    /// <param name="sessaoDto">Objeto com os campos necessários para criação de um Sessao</param>
    /// <returns>IActionResult</returns>
    /// <response code="201">Caso inserção seja feita com sucesso</response>


    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult AdicionaSessao([FromBody] CreateSessaoDto sessaoDto)
    {
        Sessao sessao = _mapper.Map<Sessao>(sessaoDto);
        _context.Sessoes.Add(sessao);
        _context.SaveChanges();
        return CreatedAtAction(nameof(RecuperaSessaoPorId), new { filmeId = sessao.FilmeId}, sessao);

    }

    [HttpGet]
    //como List implementa IEnumerable(nesse caso nao ta implementando mas no do prof apareceu no ctrl click de List) colocamos o metodo como IEnumerable pra se trocarmos nossa classe pra uma que tbm implementa o IEnumerable(interface) nao precisamos trocar o cabeçalho do método
    //estamos deixando mais abstrato possivel. dependendo menos de classes concretas
    public IEnumerable<ReadSessaoDto> RecuperaSessoes([FromQuery]int skip = 0, [FromQuery] int take = 50)
    {
        return _mapper.Map<List<ReadSessaoDto>>(_context.Sessoes.Skip(skip).Take(take));
    }

    [HttpGet("{ilmeId}/{cinemaId}")]
    //esse ponto de interrogação informa que o tipo de retorno pode retornar nulo ou não
    //public Sessao? RecuperaSessaoPorId([FromBody] int id)
    public IActionResult RecuperaSessaoPorId([FromBody] int filmeId, int cinemaId)
    {
        //retorna o primeiro elemento em que o Id da lista seja o mesmo do id do parametro passado
        var sessao =  _context.Sessoes.FirstOrDefault(sessao => sessao.FilmeId == filmeId && sessao.CinemaId == cinemaId);
        if (sessao == null) return NotFound();
        var sessaoDto = _mapper.Map<ReadSessaoDto>(sessao);
        return Ok(sessaoDto);
    }

    
}
