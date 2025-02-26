using System.ComponentModel.DataAnnotations;

namespace DevIO.NerdStore.Core.Validation;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
public class CartaoExpiracaoAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is null)
            return false;

        string mes = value.ToString()!.Split('/')[0];
        string ano = $"20{value.ToString()!.Split('/')[1]}";

        if (int.TryParse(mes, out int month) && int.TryParse(ano, out int year))
        {
            DateTime d = new(year, month, 1);

            return d > DateTime.UtcNow;
        }

        return false;
    }
}