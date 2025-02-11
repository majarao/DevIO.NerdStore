using FluentValidation.Results;

namespace DevIO.NerdStore.Core.SpecificationResult.Validation;

public abstract class ValidatorObject<T>
{
    protected ObjectAbstractValidator<T> Validator { get; } = new();
    private ValidationResult? ValidationResult { get; set; }

    protected bool Validate(T obj, ValidationResult validationResult)
    {
        ValidationResult = Validator.Validate(obj);

        foreach (ValidationFailure? error in ValidationResult.Errors)
            validationResult.Errors.Add(error);

        return ValidationResult.IsValid;
    }
}