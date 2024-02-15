using AutoMapper;
using FilmesAPI.Data.Dtos;
using FilmesAPI.Models;

public class FilmeProfile : Profile
{
	public FilmeProfile()
	{
        CreateMap<CreateFilmeDto, Filme>(); //mapear de um CreateFilmeDto para um Filme. Assim nao precisa instanciar com todos as propriedades do Model de Filme em CrateFilmeDto
        CreateMap<UpdateFilmeDto, Filme>();
        CreateMap<Filme, UpdateFilmeDto>();
        CreateMap<Filme, ReadFilmeDto>().ForMember(filmeDto => filmeDto.Sessoes, opt => opt.MapFrom(filme => filme.Sessoes));
    }
    
}
