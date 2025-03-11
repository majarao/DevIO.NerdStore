namespace DevIO.NerdStore.Pagamentos.API.Models;

public enum StatusTransacao
{
    Autorizado = 1,
    Pago,
    Negado,
    Estornado,
    Cancelado
}