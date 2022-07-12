using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Evolution.Common.Extensions
{
    public static class QueryableExtension
    {
        public static IQueryable<T> WhereLike<T>(this IQueryable<T> source, Expression<Func<T, string>> valueSelector, string value, char wildcard)
        {
            return source.Where(BuildLikeExpression(valueSelector, value, wildcard));
        }

        public static IQueryable<T> WhereLikeOr<T>(this IQueryable<T> source, Expression<Func<T, string>> firstSelector, Expression<Func<T, string>> secondSelector, string value, char wildcard)
        {
            var firstExpression=BuildLikeExpression(firstSelector, value, wildcard);
            var secondExpression = BuildLikeExpression(secondSelector, value, wildcard);
            var orExpression = Expression.OrElse(firstExpression.Body, secondExpression.Body);
            var param = Expression.Parameter(typeof(T));
            var whereExpression= Expression.Lambda<Func<T, bool>>(orExpression, param);

            return source.Where(whereExpression);
        }

        private static Expression<Func<T, bool>> BuildLikeExpression<T>(Expression<Func<T, string>> valueSelector, string value, char wildcard)
        {
            if (valueSelector == null) throw new ArgumentNullException("valueSelector");

            var method = GetLikeMethod(value, wildcard);
            value = value.Trim(wildcard);
            var body = Expression.Call(valueSelector.Body, method, Expression.Constant(value));
            var parameter = valueSelector.Parameters.Single();
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        private static MethodInfo GetLikeMethod(string value, char wildcard)
        {
            var methodName = "Equals";
            //ITK DEF 678 fix
            if (value.StartsWith(wildcard) && value.EndsWith(wildcard))
            {
                methodName = "Contains";
            } else if (value.EndsWith(wildcard))
            {
                methodName = "StartsWith";
            }
            else if (value.StartsWith(wildcard))
            {
                methodName = "EndsWith";
            } 
            var stringType = typeof(string);
            return stringType.GetMethod(methodName, new[] { stringType });
        }
    }
}
