namespace Infrastructure.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> predicate)
    {
        return condition ? query.Where(predicate) : query;
    }

    public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, string? value, Expression<Func<T, bool>> predicate)
    {
        return !string.IsNullOrWhiteSpace(value) ? query.Where(predicate) : query;
    }
}
