﻿namespace DevIO.NerdStore.Pedido.Domain.Pedidos;

public class PedidoItem
{
    public Guid PedidoId { get; private set; }
    public Guid ProdutoId { get; private set; }
    public string ProdutoNome { get; private set; } = string.Empty;
    public int Quantidade { get; private set; }
    public decimal ValorUnitario { get; private set; }
    public string? ProdutoImagem { get; set; }

    public Pedido Pedido { get; set; } = null!;

    protected PedidoItem() { }

    public PedidoItem(
        Guid produtoId, 
        string produtoNome, 
        int quantidade,
        decimal valorUnitario, 
        string? produtoImagem = null)
    {
        ProdutoId = produtoId;
        ProdutoNome = produtoNome;
        Quantidade = quantidade;
        ValorUnitario = valorUnitario;
        ProdutoImagem = produtoImagem;
    }

    internal decimal CalcularValor() => Quantidade * ValorUnitario;
}
