using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Models;

public class Cinema
{
    [Key]
    [Required]
    public int Id { get; set; }
    [Required(ErrorMessage = "O campo de nome é obrigatório.")]
    public string Nome { get; set; }

    public int EnderecoId { get; set; }

    //esse virtual vai dizer que o model de Cinema usa uma relação de 1 e apenas 1 endereço. [cardinalidade]
    //o virtual tbm tem a função de poder recuperar uma instância completa da classe (nesse caso Endereço), podendo ter acesso não só ao ID mas tbm Logradouro e Numero.
    //mas pra isso tem que instalar pacote Proxies e no Program.cs adicionar antes do UseSqlServer() o UseLazyLoadingProxies() 
    public virtual Endereco Endereco { get; set; }

    public virtual ICollection<Sessao> Sessoes { get; set; }

}
