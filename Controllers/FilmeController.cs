using AutoMapper;
using FilmesAPI.Data;
using FilmesAPI.Data.Dtos;
using FilmesAPI.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace FilmesAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class FilmeController : ControllerBase
{
    //o controler agora depende do Context pra poder funcionar(injeção de dependência)
    private FilmeContext _context;
    private IMapper _mapper;
  

    public FilmeController(FilmeContext context, IMapper mapper)
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
    public IActionResult AdicionaFilme([FromBody] CreateFilmeDto filmeDto)
    {
        Filme filme = _mapper.Map<Filme>(filmeDto);
        _context.Filmes.Add(filme);
        _context.SaveChanges();
        return CreatedAtAction(nameof(RecuperaFilmePorId), new { id = filme.Id}, filme);

    }

    [HttpGet]
    //como List implementa IEnumerable(nesse caso nao ta implementando mas no do prof apareceu no ctrl click de List) colocamos o metodo como IEnumerable pra se trocarmos nossa classe pra uma que tbm implementa o IEnumerable(interface) nao precisamos trocar o cabeçalho do método
    //estamos deixando mais abstrato possivel. dependendo menos de classes concretas
    public IEnumerable<ReadFilmeDto> RecuperaFilmes([FromQuery] int skip = 0, [FromQuery] int take = 50, [FromQuery] string? nomeCinema = null)
    {
        if (nomeCinema == null)
        {
            return _mapper.Map<List<ReadFilmeDto>>(_context.Filmes.Skip(skip).Take(take).ToList());

        }
        //fazendo consulta usando LINQ(sem usar nenhum código SQL
        return _mapper.Map<List<ReadFilmeDto>>(_context.Filmes.Skip(skip).Take(take).Where(filme => filme.Sessoes.Any(sessao => sessao.Cinema.Nome == nomeCinema)).ToList());

    }

    [HttpGet("{id}")]
    //esse ponto de interrogação informa que o tipo de retorno pode retornar nulo ou não
    //public Filme? RecuperaFilmePorId([FromBody] int id)
    public IActionResult RecuperaFilmePorId([FromBody] int id)
    {
        //retorna o primeiro elemento em que o Id da lista seja o mesmo do id do parametro passado
        var filme =  _context.Filmes.FirstOrDefault(filme => filme.Id ==id);
        if (filme == null) return NotFound();
        var filmeDto = _mapper.Map<ReadFilmeDto>(filme);
        return Ok(filmeDto);
    }

    [HttpPut("{id}")]
    public IActionResult AtualizaFilme(int id, [FromBody] UpdateFilmeDto filmeDto)
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if(filme == null) return NotFound();
        _mapper.Map(filmeDto, filme);
        _context.SaveChanges();
        return NoContent();
    }

    //o patch vai atualizar um filme sem a necessidade de passar o json com todas as informações(se eu quiser atualizr só o titulo nao preciso passar o json todo, apenas a parte com o titulo)
    [HttpPatch("{id}")]
    public IActionResult AtualizaFilmeParcial(int id, JsonPatchDocument<UpdateFilmeDto> patch)
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if (filme == null) return NotFound();

        //vou converter o UpdateFilmeDto para filme para fazer a verificação se o dado digitado foi de acordo com o esperado
        var filmeParaAtualizar = _mapper.Map<UpdateFilmeDto>(filme);

        //aqui vai ver se o patch aplicado para o filmeParaAtualizar tem um modelo de estado válido(se é válido)
        patch.ApplyTo(filmeParaAtualizar, ModelState);

        //se caso não for válido, retorna um problema de validação do modelo de estado, mas se tudo der certo pode converter o mapper novamente pra UpdateFilmeDto(lembrando de permitir essa conversão no FilmeProfile)  
        if (!TryValidateModel(filmeParaAtualizar))
        {
            return ValidationProblem(ModelState);
        }
        _mapper.Map(filmeParaAtualizar, filme);
        _context.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeletaFilme (int id)
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if (filme == null) return NotFound();
        _context.Remove(filme);
        _context.SaveChanges();
        return NoContent();
    }

    
}
