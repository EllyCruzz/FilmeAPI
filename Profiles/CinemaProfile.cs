using AutoMapper;
using FilmesAPI.Data.Dtos;
using FilmesAPI.Models;

public class CinemaProfile : Profile
{
	public CinemaProfile()
	{
        CreateMap<CreateCinemaDto, Cinema>(); //mapear de um CreateCinemaDto para um Cinema. Assim nao precisa instanciar com todos as propriedades do Model de Cinema em CrateCinemaDto
        CreateMap<UpdateCinemaDto, Cinema>();
        CreateMap<Cinema, UpdateCinemaDto>();
        CreateMap<Cinema, ReadCinemaDto>().ForMember(cinemaDto => cinemaDto.Endereco, opt => opt.MapFrom(cinema => cinema.Endereco)).ForMember(cinemaDto => cinemaDto.Sessoes, opt => opt.MapFrom(cinema => cinema.Sessoes));

    }

}
