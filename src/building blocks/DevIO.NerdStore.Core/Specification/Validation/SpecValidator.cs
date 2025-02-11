using FluentValidation.Results;

namespace DevIO.NerdStore.Core.Specification.Validation;

public class SpecValidator<T>
{
    private Dictionary<string, Rule<T>> Validations { get; } = [];

    public ValidationResult Validate(T obj)
    {
        ValidationResult validationResult = new();

        foreach (string rule in Validations.Keys)
        {
            Rule<T> validation = Validations[rule];
            if (!validation.Validate(obj))
                validationResult.Errors.Add(new ValidationFailure(obj?.GetType().Name, validation.ErrorMessage));
        }

        return validationResult;
    }

    protected void Add(string name, Rule<T> rule) => Validations.Add(name, rule);

    protected void Remove(string name) => Validations.Remove(name);

    protected Rule<T>? GetRule(string name)
    {
        Validations.TryGetValue(name, out Rule<T>? rule);
        return rule;
    }
}