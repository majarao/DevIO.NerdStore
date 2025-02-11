using FluentValidation.Results;

namespace DevIO.NerdStore.Core.SpecificationResult.Result;

public class SpecificationValidationResult(bool isValid, IEnumerable<ValidationFailure> errors)
{
    public bool IsValid { get; protected set; } = isValid;
    public IReadOnlyList<ValidationFailure> Errors { get; protected set; } = errors?.ToList().AsReadOnly() ?? new List<ValidationFailure>().AsReadOnly();
}