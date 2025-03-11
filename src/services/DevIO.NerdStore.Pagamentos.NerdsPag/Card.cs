using System.Security.Cryptography;
using System.Text;

namespace DevIO.NerdStore.Pagamentos.NerdsPag;

public class CardHash(NerdsPagService nerdsPagService)
{
    private NerdsPagService NerdsPagService { get; } = nerdsPagService;

    public string CardHolderName { get; set; } = string.Empty;
    public string CardNumber { get; set; } = string.Empty;
    public string CardExpirationDate { get; set; } = string.Empty;
    public string CardCvv { get; set; } = string.Empty;

    public string Generate()
    {
        using Aes aesAlg = Aes.Create();

        aesAlg.IV = Encoding.Default.GetBytes(NerdsPagService.EncryptionKey);
        aesAlg.Key = Encoding.Default.GetBytes(NerdsPagService.ApiKey);

        ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

        using MemoryStream msEncrypt = new();
        using CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write);

        using (StreamWriter swEncrypt = new(csEncrypt))
            swEncrypt.Write(CardHolderName + CardNumber + CardExpirationDate + CardCvv);

        return Encoding.ASCII.GetString(msEncrypt.ToArray());
    }
}