using System.Text;
using ExpressionBuilderLib.src.Core;

namespace ExpressionBuilderLib.src.Extensions;




public static class ExpressionExtensions
{



    public static Expression<Func<T, bool>> And<T>(
        this Expression<Func<T, bool>> expr1,
        Expression<Func<T, bool>> expr2)
    {
        return ExpressionCombiner.Combine(expr1, expr2, Core.Enums.LogicalOperator.And);
    }




    public static Expression<Func<T, bool>> Or<T>(
        this Expression<Func<T, bool>> expr1,
        Expression<Func<T, bool>> expr2)
    {
        return ExpressionCombiner.Combine(expr1, expr2, Core.Enums.LogicalOperator.Or);
    }




    public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> expression)
    {
        return ExpressionCombiner.Negate(expression);
    }




    public static string GetPropertyName<T, TProperty>(
        this Expression<Func<T, TProperty>> propertyExpression)
    {
        if (propertyExpression.Body is MemberExpression memberExpression)
            return memberExpression.Member.Name;

        throw new ArgumentException("Expression is not a property access", nameof(propertyExpression));
    }
}
