using Microsoft.AspNetCore.Mvc.Razor;
using System.Security.Cryptography;
using System.Text;

namespace DevIO.NerdStore.WebApp.MVC.Extensions;

public static class RazorHelpers
{
    public static string HashEmailForGravatar(this RazorPage page, string email)
    {
        byte[] data = MD5.HashData(Encoding.Default.GetBytes(email));
        StringBuilder sBuilder = new();
        foreach (byte t in data)
            sBuilder.Append(t.ToString("x2"));

        return sBuilder.ToString();
    }

    public static string FormatoMoeda(this RazorPage page, decimal valor) =>
        valor > 0 ? string.Format(Thread.CurrentThread.CurrentCulture, "{0:C}", valor) : "Gratuito";

    private static string FormatoMoeda(decimal valor) => string.Format(Thread.CurrentThread.CurrentCulture, "{0:C}", valor);

    public static string MensagemEstoque(this RazorPage page, int quantidade) =>
        quantidade > 0 ? $"Apenas {quantidade} em estoque!" : "Produto esgotado!";

    public static string UnidadesPorProduto(this RazorPage page, int unidades) => unidades > 1 ? $"{unidades} unidades" : $"{unidades} unidade";

    public static string SelectOptionsPorQuantidade(this RazorPage page, int quantidade, int valorSelecionado = 0)
    {
        StringBuilder sb = new();
        for (int i = 1; i <= quantidade; i++)
        {
            string selected = "";
            if (i == valorSelecionado)
                selected = "selected";

            sb.Append($"<option {selected} value='{i}'>{i}</option>");
        }

        return sb.ToString();
    }

    public static string UnidadesPorProdutoValorTotal(this RazorPage page, int unidades, decimal valor) =>
        $"{unidades}x {FormatoMoeda(valor)} = Total: {FormatoMoeda(valor * unidades)}";

    public static string ExibeStatus(this RazorPage page, int status)
    {
        string statusMensagem = "";
        string statusClasse = "";

        switch (status)
        {
            case 1:
                statusClasse = "info";
                statusMensagem = "Em aprovação";
                break;
            case 2:
                statusClasse = "primary";
                statusMensagem = "Aprovado";
                break;
            case 3:
                statusClasse = "danger";
                statusMensagem = "Recusado";
                break;
            case 4:
                statusClasse = "success";
                statusMensagem = "Entregue";
                break;
            case 5:
                statusClasse = "warning";
                statusMensagem = "Cancelado";
                break;

        }

        return $"<span class='badge badge-{statusClasse}'>{statusMensagem}</span>";
    }
}