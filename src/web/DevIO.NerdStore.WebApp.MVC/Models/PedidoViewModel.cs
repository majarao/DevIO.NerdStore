﻿namespace DevIO.NerdStore.WebApp.MVC.Models;

public class PedidoViewModel
{
    public int Codigo { get; set; }
    public int Status { get; set; }
    public DateTime Data { get; set; }
    public decimal ValorTotal { get; set; }
    public decimal Desconto { get; set; }
    public bool VoucherUtilizado { get; set; }

    public List<ItemPedidoViewModel> PedidoItems { get; set; } = [];

    public EnderecoViewModel? Endereco { get; set; }
}
