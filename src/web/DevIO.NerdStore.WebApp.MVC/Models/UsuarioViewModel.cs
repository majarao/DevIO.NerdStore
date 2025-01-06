using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DevIO.NerdStore.WebApp.MVC.Models;

public class UsuarioRegistro
{
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 6)]
    public string Senha { get; set; } = string.Empty;

    [DisplayName("Confirme sua senha")]
    [Compare("Senha", ErrorMessage = "As senhas não conferem.")]
    public string SenhaConfirmacao { get; set; } = string.Empty;
}

public class UsuarioLogin
{
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 6)]
    public string Senha { get; set; } = string.Empty;
}

public class UsuarioRespostaLogin
{
    public string AccessToken { get; set; } = string.Empty;
    public double ExpiresIn { get; set; }
    public UsuarioToken? UsuarioToken { get; set; }
    public ResponseResult? ResponseResult { get; set; }
}

public class UsuarioToken
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public IEnumerable<UsuarioClaim>? Claims { get; set; }
}

public class UsuarioClaim
{
    public string Value { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
}