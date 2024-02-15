using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Data.Dtos;

//Com DTOs podemos definir os parâmetros enviados de maneira isolada do nosso modelo do banco de dados.
//Há parâmetros que não precisamos enviar, como por exemplo o id.
//Outro fator é que não estamos mais o nosso modelo do banco de dados.
public class UpdateFilmeDto
{

    [Required(ErrorMessage = "O titulo do filme é obrigatório")]
    public string Titulo { get; set; }
    [Required(ErrorMessage = "O gênero do filme é obrigatório")]
    [StringLength(50, ErrorMessage = "O tamanho do gênero não pode exceder 50 caracteres")]
    public string Genero { get; set; }
    [Required(ErrorMessage = "A duração do filme é obrigatória")]
    [Range(70, 600, ErrorMessage = "A duração deve ser entre 70 e 600 minutos")]
    public int Duracao { get; set; }
}
