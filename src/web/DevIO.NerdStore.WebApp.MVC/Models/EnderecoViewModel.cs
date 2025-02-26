using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DevIO.NerdStore.WebApp.MVC.Models;

public class EnderecoViewModel
{
    [Required]
    public string Logradouro { get; set; } = string.Empty;

    [Required]
    [DisplayName("Número")]
    public string Numero { get; set; } = string.Empty;

    public string Complemento { get; set; } = string.Empty;

    [Required]
    public string Bairro { get; set; } = string.Empty;

    [Required]
    [DisplayName("CEP")]
    public string Cep { get; set; } = string.Empty;

    [Required]
    public string Cidade { get; set; } = string.Empty;

    [Required]
    public string Estado { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"{Logradouro}, {Numero} {Complemento} - {Bairro} - {Cidade} - {Estado}";
    }
}
