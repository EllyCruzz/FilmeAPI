using AutoMapper;
using FilmesAPI.Data;
using FilmesAPI.Data.Dtos;
using FilmesAPI.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace FilmesAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class EnderecoController : ControllerBase
{
    //o controler agora depende do Context pra poder funcionar(injeção de dependência)
    private FilmeContext _context;
    private IMapper _mapper;
  

    public EnderecoController(FilmeContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Adiciona um filme ao banco de dados
    /// </summary>
    /// <param name="enderecoDto">Objeto com os campos necessários para criação de um filme</param>
    /// <returns>IActionResult</returns>
    /// <response code="201">Caso inserção seja feita com sucesso</response>


    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult AdicionaEndereco([FromBody] CreateEnderecoDto enderecoDto)
    {
        Endereco endereco = _mapper.Map<Endereco>(enderecoDto);
        _context.Enderecos.Add(endereco);
        _context.SaveChanges();
        return CreatedAtAction(nameof(RecuperaEnderecoPorId), new { id = endereco.Id}, enderecoDto);

    }
     
    [HttpGet]
    //como List implementa IEnumerable(nesse caso nao ta implementando mas no do prof apareceu no ctrl click de List) colocamos o metodo como IEnumerable pra se trocarmos nossa classe pra uma que tbm implementa o IEnumerable(interface) nao precisamos trocar o cabeçalho do método
    //estamos deixando mais abstrato possivel. dependendo menos de classes concretas
    public IEnumerable<ReadEnderecoDto> RecuperaEnderecos([FromQuery]int skip = 0, [FromQuery] int take = 50)
    {
        return _mapper.Map<List<ReadEnderecoDto>>(_context.Enderecos.Skip(skip).Take(take));
    }

    [HttpGet("{id}")]
    //esse ponto de interrogação informa que o tipo de retorno pode retornar nulo ou não
    //public Filme? RecuperaFilmePorId([FromBody] int id)
    public IActionResult RecuperaEnderecoPorId([FromBody] int id)
    {
        //retorna o primeiro elemento em que o Id da lista seja o mesmo do id do parametro passado
        var endereco =  _context.Filmes.FirstOrDefault(endereco => endereco.Id ==id);
        if (endereco == null) return NotFound();
        var enderecoDto = _mapper.Map<ReadEnderecoDto>(endereco);
        return Ok(enderecoDto);
    }

    [HttpPut("{id}")]
    public IActionResult AtualizaEndereco(int id, [FromBody] UpdateEnderecoDto enderecoDto)
    {
        var endereco = _context.Enderecos.FirstOrDefault(endereco => endereco.Id == id);
        if(endereco == null) return NotFound();
        _mapper.Map(enderecoDto, endereco);
        _context.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeletaEndereco (int id)
    {
        var endereco = _context.Enderecos.FirstOrDefault(Endereco => Endereco.Id == id);
        if (endereco == null) return NotFound();
        _context.Remove(endereco);
        _context.SaveChanges();
        return NoContent();
    }

    
}
