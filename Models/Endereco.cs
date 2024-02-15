using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Models;

public class Endereco
{
    [Key]
    [Required]
    public int Id { get; set; }
    [Required(ErrorMessage = "O campo de logradouro é obrigatório.")]
    public string Logradouro { get; set; }
    public int Numero { get; set; }

    public virtual Cinema Cinema { get; set; }
}
