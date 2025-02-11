using DevIO.NerdStore.Core.SpecificationResult.Result;
using DevIO.NerdStore.Core.SpecificationResult.Validation;
using FluentValidation.Results;
using System.Linq.Expressions;

namespace DevIO.NerdStore.Core.SpecificationResult;

public abstract class SpecificationValidator<T> : ValidatorObject<T> where T : class
{
    private ValidationResult Results { get; } = new ValidationResult();

    public SpecificationValidationResult Validate(T entity)
    {
        Func<T, ValidationResult, bool> predicate = ToExpression().Compile();
        bool evaluationResult = predicate(entity, Results);

        return new SpecificationValidationResult(evaluationResult, Results.Errors);
    }

    public bool IsSatisfiedBy(T entity)
    {
        Func<T, ValidationResult, bool> predicate = ToExpression().Compile();
        bool evaluationResult = predicate(entity, Results);

        return evaluationResult;
    }

    public virtual Expression<Func<T, ValidationResult, bool>> ToExpression() => (e, results) => Validate(e, results);

    public SpecificationValidator<T> And(SpecificationValidator<T> specification) => new AndSpecificationValidator<T>(this, specification);

    public SpecificationValidator<T> Or(SpecificationValidator<T> specification) => new OrSpecificationValidator<T>(this, specification);

    public SpecificationValidator<T> Not() => new NotSpecificationValidator<T>(this);
}