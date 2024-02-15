using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Data.Dtos;

//Com DTOs podemos definir os parâmetros enviados de maneira isolada do nosso modelo do banco de dados.
//Há parâmetros que não precisamos enviar, como por exemplo o id.
//Outro fator é que não estamos mais o nosso modelo do banco de dados.
public class CreateSessaoDto
{
    public int FilmeId { get; set; }
    public int CinemaId { get; set; }
}
