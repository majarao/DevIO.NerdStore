using System.Linq.Expressions;

namespace DevIO.NerdStore.Core.Specification;

public class GenericSpecification<T>(Expression<Func<T, bool>> expression)
{
    private Expression<Func<T, bool>> Expression { get; } = expression;

    public bool IsSatisfiedBy(T entity) => Expression.Compile().Invoke(entity);
}