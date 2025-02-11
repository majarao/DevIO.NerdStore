using System.Linq.Expressions;

namespace DevIO.NerdStore.Core.Specification;

internal sealed class IdentitySpecification<T> : Specification<T>
{
    public override Expression<Func<T, bool>> ToExpression() => x => true;
}