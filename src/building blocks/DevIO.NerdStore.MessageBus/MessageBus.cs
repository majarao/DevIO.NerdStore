using DevIO.NerdStore.Core.Messages.Integration;
using EasyNetQ;
using Polly;
using Polly.Retry;
using RabbitMQ.Client.Exceptions;

namespace DevIO.NerdStore.MessageBus;

public class MessageBus : IMessageBus
{
    private IBus? Bus { get; set; }
    private string ConnectionString { get; }

    public bool IsConnected => Bus?.Advanced.IsConnected ?? false;

    private void TryConnect()
    {
        if (IsConnected)
            return;

        RetryPolicy policy = Policy
            .Handle<EasyNetQException>()
            .Or<BrokerUnreachableException>()
            .WaitAndRetry(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        policy.Execute(() => Bus = RabbitHutch.CreateBus(ConnectionString, s => s.EnableSystemTextJson()));
    }

    public MessageBus(string connectionString)
    {
        ConnectionString = connectionString;
        TryConnect();
    }

    public void Publish<T>(T message) where T : IntegrationEvent
    {
        TryConnect();
        Bus!.PubSub.Publish(message);
    }

    public async Task PublishAsync<T>(T message) where T : IntegrationEvent
    {
        TryConnect();
        await Bus!.PubSub.PublishAsync(message);
    }

    public void Subscribe<T>(string subscriptionId, Action<T> onMessage) where T : class
    {
        TryConnect();
        Bus!.PubSub.Subscribe(subscriptionId, onMessage);
    }

    public async void SubscribeAsync<T>(string subscriptionId, Func<T, Task> onMessage) where T : class
    {
        TryConnect();
        await Bus!.PubSub.SubscribeAsync(subscriptionId, onMessage);
    }

    public TResponse Request<TRequest, TResponse>(TRequest request)
        where TRequest : IntegrationEvent
        where TResponse : ResponseMessage
    {
        TryConnect();
        return Bus!.Rpc.Request<TRequest, TResponse>(request);
    }

    public async Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest request)
        where TRequest : IntegrationEvent
        where TResponse : ResponseMessage
    {
        TryConnect();
        return await Bus!.Rpc.RequestAsync<TRequest, TResponse>(request);
    }

    public IDisposable Respond<TRequest, TResponse>(Func<TRequest, TResponse> responder)
        where TRequest : IntegrationEvent
        where TResponse : ResponseMessage
    {
        TryConnect();
        return Bus!.Rpc.Respond(responder);
    }

    public async Task<IDisposable> RespondAsync<TRequest, TResponse>(Func<TRequest, Task<TResponse>> responder)
        where TRequest : IntegrationEvent
        where TResponse : ResponseMessage
    {
        TryConnect();
        return await Bus!.Rpc.RespondAsync(responder);
    }

    public void Dispose() => GC.SuppressFinalize(this);
}
