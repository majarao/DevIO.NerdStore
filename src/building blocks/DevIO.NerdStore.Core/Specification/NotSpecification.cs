using System.Linq.Expressions;

namespace DevIO.NerdStore.Core.Specification;

internal sealed class NotSpecification<T>(Specification<T> specification) : Specification<T>
{
    private Specification<T> Specification { get; } = specification;

    public override Expression<Func<T, bool>> ToExpression()
    {
        Expression<Func<T, bool>> expression = Specification.ToExpression();
        UnaryExpression notExpression = Expression.Not(expression.Body);

        return Expression.Lambda<Func<T, bool>>(notExpression, expression.Parameters.Single());
    }
}