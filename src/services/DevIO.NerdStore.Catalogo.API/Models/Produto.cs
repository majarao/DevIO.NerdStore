﻿using DevIO.NerdStore.Core.DomainObjects;

namespace DevIO.NerdStore.Catalogo.API.Models;

public class Produto : Entity, IAggregateRoot
{
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public bool Ativo { get; set; }
    public decimal Valor { get; set; }
    public DateTime DataCadastro { get; set; }
    public string Imagem { get; set; } = string.Empty;
    public int QuantidadeEstoque { get; set; }

    public void RetirarEstoque(int quantidade)
    {
        if (QuantidadeEstoque >= quantidade)
            QuantidadeEstoque -= quantidade;
    }

    public bool EstaDisponivel(int quantidade) => Ativo && QuantidadeEstoque >= quantidade;
}
