﻿using Microsoft.AspNetCore.Mvc.Razor;
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
}