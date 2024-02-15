using AutoMapper;
using FilmesAPI.Data.Dtos;
using FilmesAPI.Models;

public class EnderecoProfile : Profile
{
	public EnderecoProfile()
	{
        CreateMap<CreateEnderecoDto, Endereco>(); //mapear de um CreateEnderecoDto para um Endereco. Assim nao precisa instanciar com todos as propriedades do Model de Endereco em CrateEnderecoDto
        CreateMap<UpdateEnderecoDto, Endereco>();
        CreateMap<Endereco, UpdateEnderecoDto>();
        CreateMap<Endereco, ReadEnderecoDto>();
    }
    
}
