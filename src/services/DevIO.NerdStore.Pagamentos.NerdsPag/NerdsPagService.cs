namespace DevIO.NerdStore.Pagamentos.NerdsPag;

public class NerdsPagService(string apiKey, string encryptionKey)
{
    public string ApiKey { get; } = apiKey;
    public string EncryptionKey { get; } = encryptionKey;
}