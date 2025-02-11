namespace DevIO.NerdStore.Core.Specification.Validation;

public class Rule<T>(Specification<T> spec, string errorMessage)
{
    private Specification<T> SpecificationSpec { get; } = spec;
    public string ErrorMessage { get; } = errorMessage;

    public bool Validate(T obj) => SpecificationSpec.IsSatisfiedBy(obj);
}