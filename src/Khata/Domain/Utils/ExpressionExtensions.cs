using System.Linq.Expressions;

namespace Domain.Utils;

internal class SubstExpressionVisitor : ExpressionVisitor
{
    public Dictionary<Expression, Expression> subst = new();

    protected override Expression VisitParameter(
        ParameterExpression node) =>
        subst.TryGetValue(node, out var newValue) ? newValue : node;
}

public static class PredicateBuilder
{
    public static Expression<Func<T, bool>> And<T>(
        this Expression<Func<T, bool>> a,
        Expression<Func<T, bool>> b
    )
    {

        var p = a.Parameters[0];

        var visitor = new SubstExpressionVisitor
        {
            subst = {[b.Parameters[0]] = p}
        };

        Expression body = 
            Expression.AndAlso(
                a.Body, 
                visitor.Visit(b.Body)
            );
        
        return Expression.Lambda<Func<T, bool>>(body, p);
    }

    public static Expression<Func<T, bool>> Or<T>(
        this Expression<Func<T, bool>> a,
        Expression<Func<T, bool>> b)
    {
        var p = a.Parameters[0];

        var visitor =
            new SubstExpressionVisitor
            {
                subst = {[b.Parameters[0]] = p}
            };

        Expression body = 
            Expression.OrElse(
                a.Body, 
                visitor.Visit(b.Body)
            );

        return Expression.Lambda<Func<T, bool>>(body, p);
    }

    public static Expression<Func<T, bool>> AddTrackedDocumentFilter<T>(
            this Expression<Func<T, bool>> predicate,
            DateTimeOffset? maybeFromDate,
            DateTimeOffset? maybeToDate) where T : TrackedDocument =>
        predicate.AddDocumentFilter(maybeFromDate, maybeToDate)
            .And(document => document.IsRemoved == false);

    public static Func<Document, DateTimeOffset?, DateTimeOffset?, bool> DocumentCreationTimeFilter =
        (document, maybeFromDate, maybeToDate) =>
            (maybeFromDate == null || document.Metadata.CreationTime >= maybeFromDate)
            && (maybeToDate == null || document.Metadata.CreationTime <= maybeFromDate);
    
    public static Expression<Func<T, bool>> AddDocumentFilter<T>(
            this Expression<Func<T, bool>> predicate,
            DateTimeOffset? maybeFromDate,
            DateTimeOffset? maybeToDate) where T : Document =>
        predicate.And(document => 
            DocumentCreationTimeFilter(
                document, 
                maybeFromDate, 
                maybeToDate));
}