using System.Linq.Expressions;

namespace DevIO.NerdStore.Core.Specification;

internal sealed class OrSpecification<T>(Specification<T> left, Specification<T> right) : Specification<T>
{
    private Specification<T> Left { get; } = left;
    private Specification<T> Right { get; } = right;

    public override Expression<Func<T, bool>> ToExpression()
    {
        Expression<Func<T, bool>> leftExpression = Left.ToExpression();
        Expression<Func<T, bool>> rightExpression = Right.ToExpression();

        InvocationExpression invokedExpression = Expression.Invoke(rightExpression, leftExpression.Parameters);

        return (Expression<Func<T, bool>>)Expression.Lambda(Expression.OrElse(leftExpression.Body, invokedExpression), leftExpression.Parameters);
    }
}