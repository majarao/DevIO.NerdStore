using FluentValidation.Results;
using System.Linq.Expressions;

namespace DevIO.NerdStore.Core.SpecificationResult;

internal sealed class OrSpecificationValidator<T>(SpecificationValidator<T> left, SpecificationValidator<T> right) : SpecificationValidator<T> where T : class
{
    private SpecificationValidator<T> Left { get; } = left;
    private SpecificationValidator<T> Right { get; } = right;

    public override Expression<Func<T, ValidationResult, bool>> ToExpression()
    {
        Expression<Func<T, ValidationResult, bool>> leftExpression = Left.ToExpression();
        Expression<Func<T, ValidationResult, bool>> rightExpression = Right.ToExpression();

        InvocationExpression invokedExpression = Expression.Invoke(rightExpression, leftExpression.Parameters);

        return (Expression<Func<T, ValidationResult, bool>>)Expression.Lambda(Expression.OrElse(leftExpression.Body, invokedExpression), leftExpression.Parameters);
    }
}