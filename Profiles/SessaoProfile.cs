using AutoMapper;
using FilmesAPI.Data.Dtos;
using FilmesAPI.Models;

public class SessaoProfile : Profile
{
	public SessaoProfile()
	{
        CreateMap<CreateSessaoDto, Sessao>(); //mapear de um CreateSessaoDto para um Sessao. Assim nao precisa instanciar com todos as propriedades do Model de Sessao em CrateSessaoDto
       // CreateMap<UpdateSessaoDto, Sessao>();
      //  CreateMap<Sessao, UpdateSessaoDto>();
        CreateMap<Sessao, ReadSessaoDto>();
    }
    
}
