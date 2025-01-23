using DevIO.NerdStore.Core.DomainObjects;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace DevIO.NerdStore.WebApp.MVC.Extensions;

public class CpfAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) =>
        Cpf.Validar(value?.ToString()) ? ValidationResult.Success : new ValidationResult("CPF em formato inválido");
}

public class CpfAttributeAdapter(CpfAttribute attribute, IStringLocalizer? stringLocalizer) : AttributeAdapterBase<CpfAttribute>(attribute, stringLocalizer)
{
    public override void AddValidation(ClientModelValidationContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        MergeAttribute(context.Attributes, "data-val", "true");
        MergeAttribute(context.Attributes, "data-val-cpf", GetErrorMessage(context));
    }

    public override string GetErrorMessage(ModelValidationContextBase validationContext) => "CPF em formato inválido";
}

public class CpfValidationAttributeAdapterProvider : IValidationAttributeAdapterProvider
{
    private ValidationAttributeAdapterProvider BaseProvider { get; } = new();

    public IAttributeAdapter? GetAttributeAdapter(ValidationAttribute attribute, IStringLocalizer? stringLocalizer)
    {
        if (attribute is CpfAttribute CpfAttribute)
            return new CpfAttributeAdapter(CpfAttribute, stringLocalizer);

        return BaseProvider.GetAttributeAdapter(attribute, stringLocalizer);
    }
}