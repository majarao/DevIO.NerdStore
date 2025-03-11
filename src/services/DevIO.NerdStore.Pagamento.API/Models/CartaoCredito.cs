﻿namespace DevIO.NerdStore.Pagamentos.API.Models;

public class CartaoCredito
{
    public string NomeCartao { get; set; } = string.Empty;
    public string NumeroCartao { get; set; } = string.Empty;
    public string MesAnoVencimento { get; set; } = string.Empty;
    public string CVV { get; set; } = string.Empty;

    protected CartaoCredito() { }

    public CartaoCredito(string nomeCartao, string numeroCartao, string mesAnoVencimento, string cvv)
    {
        NomeCartao = nomeCartao;
        NumeroCartao = numeroCartao;
        MesAnoVencimento = mesAnoVencimento;
        CVV = cvv;
    }
}