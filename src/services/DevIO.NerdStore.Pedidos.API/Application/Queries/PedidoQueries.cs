using Dapper;
using DevIO.NerdStore.Pedidos.API.Application.DTO;
using DevIO.NerdStore.Pedidos.Domain.Pedidos;

namespace DevIO.NerdStore.Pedidos.API.Application.Queries;

public class PedidoQueries(IPedidoRepository repository) : IPedidoQueries
{
    private IPedidoRepository Repository { get; } = repository;

    public async Task<IEnumerable<PedidoDTO>> ObterListaPorClienteId(Guid clienteId)
    {
        IEnumerable<Pedido> pedidos = await Repository.ObterListaPorClienteId(clienteId);

        return pedidos.Select(PedidoDTO.ParaPedidoDTO);
    }

    public async Task<PedidoDTO> ObterUltimoPedido(Guid clienteId)
    {
        const string sql = @"SELECT
                                P.ID AS 'ProdutoId', P.CODIGO, P.VOUCHERUTILIZADO, P.DESCONTO, P.VALORTOTAL,P.PEDIDOSTATUS,
                                P.LOGRADOURO,P.NUMERO, P.BAIRRO, P.CEP, P.COMPLEMENTO, P.CIDADE, P.ESTADO,
                                PIT.ID AS 'ProdutoItemId',PIT.PRODUTONOME, PIT.QUANTIDADE, PIT.PRODUTOIMAGEM, PIT.VALORUNITARIO 
                                FROM PEDIDOS P 
                                INNER JOIN PEDIDOITEMS PIT ON P.ID = PIT.PEDIDOID 
                                WHERE P.CLIENTEID = @clienteId 
                                AND P.DATACADASTRO between DATEADD(minute, -3,  GETDATE()) and DATEADD(minute, 0,  GETDATE())
                                AND P.PEDIDOSTATUS = 1 
                                ORDER BY P.DATACADASTRO DESC";

        IEnumerable<dynamic> pedido = await Repository.ObterConexao()
            .QueryAsync<dynamic>(sql, new { clienteId });

        return MapearPedido(pedido);
    }

    private static PedidoDTO MapearPedido(dynamic result)
    {
        PedidoDTO pedido = new()
        {
            Codigo = result[0].CODIGO,
            Status = result[0].PEDIDOSTATUS,
            ValorTotal = result[0].VALORTOTAL,
            Desconto = result[0].DESCONTO,
            VoucherUtilizado = result[0].VOUCHERUTILIZADO,

            PedidoItems = [],
            Endereco = new()
            {
                Logradouro = result[0].LOGRADOURO,
                Bairro = result[0].BAIRRO,
                Cep = result[0].CEP,
                Cidade = result[0].CIDADE,
                Complemento = result[0].COMPLEMENTO,
                Estado = result[0].ESTADO,
                Numero = result[0].NUMERO
            }
        };

        foreach (dynamic item in result)
        {
            PedidoItemDTO pedidoItem = new()
            {
                Nome = item.PRODUTONOME,
                Valor = item.VALORUNITARIO,
                Quantidade = item.QUANTIDADE,
                Imagem = item.PRODUTOIMAGEM
            };

            pedido.PedidoItems.Add(pedidoItem);
        }

        return pedido;
    }
}
