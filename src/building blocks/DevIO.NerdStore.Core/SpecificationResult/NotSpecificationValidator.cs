using FluentValidation.Results;
using System.Linq.Expressions;

namespace DevIO.NerdStore.Core.SpecificationResult;

internal sealed class NotSpecificationValidator<T>(SpecificationValidator<T> specification) : SpecificationValidator<T> where T : class
{
    private SpecificationValidator<T> Specification { get; } = specification;

    public override Expression<Func<T, ValidationResult, bool>> ToExpression()
    {
        Expression<Func<T, ValidationResult, bool>> expression = Specification.ToExpression();
        UnaryExpression notExpression = Expression.Not(expression.Body);

        return Expression.Lambda<Func<T, ValidationResult, bool>>(notExpression, expression.Parameters);
    }
}