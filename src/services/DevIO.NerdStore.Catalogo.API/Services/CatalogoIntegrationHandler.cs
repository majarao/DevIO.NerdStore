using DevIO.NerdStore.Catalogo.API.Models;
using DevIO.NerdStore.Core.DomainObjects;
using DevIO.NerdStore.Core.Messages.Integration;
using DevIO.NerdStore.MessageBus;

namespace DevIO.NerdStore.Catalogo.API.Services;

public class CatalogoIntegrationHandler(IServiceProvider serviceProvider, IMessageBus bus) : BackgroundService
{
    private IMessageBus Bus { get; } = bus;
    private IServiceProvider ServiceProvider { get; } = serviceProvider;

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        SetSubscribers();

        return Task.CompletedTask;
    }

    private void SetSubscribers() => 
        Bus.SubscribeAsync<PedidoAutorizadoIntegrationEvent>("PedidoAutorizado", async request => await BaixarEstoque(request));

    private async Task BaixarEstoque(PedidoAutorizadoIntegrationEvent message)
    {
        using IServiceScope scope = ServiceProvider.CreateScope();

        List<Produto> produtosComEstoque = [];

        IProdutoRepository produtoRepository = scope.ServiceProvider.GetRequiredService<IProdutoRepository>();

        string idsProdutos = string.Join(",", message.Itens.Select(c => c.Key));

        List<Produto> produtos = await produtoRepository.ObterProdutosPorId(idsProdutos);

        if (produtos.Count != message.Itens.Count)
        {
            CancelarPedidoSemEstoque(message);

            return;
        }

        foreach (Produto produto in produtos)
        {
            int quantidadeProduto = message.Itens.FirstOrDefault(p => p.Key == produto.Id).Value;

            if (produto.EstaDisponivel(quantidadeProduto))
            {
                produto.RetirarEstoque(quantidadeProduto);

                produtosComEstoque.Add(produto);
            }
        }

        if (produtosComEstoque.Count != message.Itens.Count)
        {
            CancelarPedidoSemEstoque(message);

            return;
        }

        foreach (Produto produto in produtosComEstoque)
            produtoRepository.Atualizar(produto);

        if (!await produtoRepository.UnitOfWork.Commit())
            throw new DomainException($"Problemas ao atualizar estoque do pedido {message.PedidoId}");

        PedidoBaixadoEstoqueIntegrationEvent pedidoBaixado = new(message.ClienteId, message.PedidoId);

        await Bus.PublishAsync(pedidoBaixado);
    }

    public async void CancelarPedidoSemEstoque(PedidoAutorizadoIntegrationEvent message)
    {
        PedidoCanceladoIntegrationEvent pedidoCancelado = new(message.ClienteId, message.PedidoId);

        await Bus.PublishAsync(pedidoCancelado);
    }
}
