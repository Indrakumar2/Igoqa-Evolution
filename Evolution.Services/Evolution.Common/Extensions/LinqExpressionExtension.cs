using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Evolution.Common.Extensions
{
    public static class LinqExpressionExtension
    {
        private static MethodInfo containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
        private static MethodInfo startsWithMethod = typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) });
        private static MethodInfo endsWithMethod = typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) });

        public static Expression<Func<T, bool>> ToExpression<T>(this T entity) where T : class
        {
            //IList<Filter> predictFilter = new List<Filter>();
            //var modelProrties = LinqExpressionExtension.GetProperties<T>(entity);

            //foreach (KeyValuePair<string, KeyValuePair<object, string>> entityProperty in modelProrties)
            //{
            //    if (LinqExpressionExtension.IsIncludeFieldInFilter(entityProperty.Value))
            //    {
            //        predictFilter.Add(new Filter { PropertyName = entityProperty.Key, Operator = GetOperator(entityProperty.Value), Value = entityProperty.Value.Key });

            //        if (entityProperty.Value.Value.ToString().ToLower() == "string" && entityProperty.Value.Key.ToString().HasEvoWildCardChar())
            //            predictFilter.Where(x => x.PropertyName == entityProperty.Key).ToList()
            //                         .ForEach(s => s.Value = s.Value.ToString().Trim('*'));
            //    }
            //}
            //return LinqExpressionExtension.GetExpression<T>(predictFilter);

            return ToExpression<T>(entity, null, string.Empty);
        }

        public static Expression<Func<T, bool>> ToExpression<T>(this T entity, IList<string> excludePropertyNames = null, string paramName = "") where T : class
        {
            IList<Filter> predictFilter = new List<Filter>();
            var modelProrties = LinqExpressionExtension.GetProperties<T>(entity);
            if (excludePropertyNames?.Count > 0)
                modelProrties = modelProrties?.Where(x => !excludePropertyNames.Contains(x.Key)).ToList();

            foreach (KeyValuePair<string, KeyValuePair<object, string>> entityProperty in modelProrties)
            {
                if (LinqExpressionExtension.IsValueApplicableForFilter(entityProperty.Value))
                {
                    predictFilter.Add(new Filter { PropertyName = entityProperty.Key, Operator = GetOperator(entityProperty.Value), Value = entityProperty.Value.Key });
                    if (entityProperty.Value.Value.ToString().ToLower() == "string" && entityProperty.Value.Key.ToString().HasEvoWildCardChar())
                        predictFilter.Where(x => x.PropertyName == entityProperty.Key)
                                     .ToList()
                                     .ForEach(s => s.Value = s.Value.ToString().Trim('*'));
                }
            }
            return LinqExpressionExtension.GetExpression<T>(predictFilter, paramName);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> source, Expression<Func<T, bool>> expr1, string paramName = "") where T : class
        {
            var and = Expression.AndAlso(source.Body, expr1.Body);
            ParameterExpression param = null;
            if (string.IsNullOrEmpty(paramName))
                param = Expression.Parameter(typeof(T));
            else
                param = Expression.Parameter(typeof(T), paramName);
            return Expression.Lambda<Func<T, bool>>(and, param);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> source, Expression<Func<T, bool>> expr1, string paramName = "") where T : class
        {
            var orEXpression = Expression.OrElse(source.Body, expr1.Body);
            ParameterExpression param = null;
            if (string.IsNullOrEmpty(paramName))
                param = Expression.Parameter(typeof(T));
            else
                param = Expression.Parameter(typeof(T), paramName);
            return Expression.Lambda<Func<T, bool>>(orEXpression, param);
        }

        public static Expression<Func<T, bool>> ToMongoExpression<T>(this T entity, IList<string> excludePropertyNames = null, string paramName = "") where T : class
        {
            var predictFilter = new List<GroupFilter>();
            var modelProrties = LinqExpressionExtension.GetProperties<T>(entity);
            if (excludePropertyNames?.Count > 0)
                modelProrties = modelProrties?.Where(x => !excludePropertyNames.Contains(x.Key)).ToList();

            foreach (KeyValuePair<string, KeyValuePair<object, string>> entityProperty in modelProrties)
            {
                if (LinqExpressionExtension.IsValueApplicableForFilter(entityProperty.Value))
                {
                    string[] splittedText = null;
                    LogicalOperator groupLogicalOps = LogicalOperator.And;
                    string textToBeSplitted = entityProperty.Value.Key?.ToString();

                    if (!string.IsNullOrEmpty(textToBeSplitted))
                    {
                        if (textToBeSplitted.Split(" + ").Count() > 1)
                            splittedText = textToBeSplitted.Split(" + ");
                        else if (textToBeSplitted.Split(" - ")?.Count() > 1)
                        {
                            splittedText = textToBeSplitted.Split(" - ");
                            groupLogicalOps = LogicalOperator.Or;
                        }
                        else
                            splittedText = new string[] { textToBeSplitted };
                    }

                    if (splittedText?.Count() > 0)
                    {
                        int index = 0;
                        string grpName = DateTime.UtcNow.ToLongTimeString();
                        GroupFilter grpFilter = new GroupFilter() { Filters = new List<Filter>() };
                        foreach (var text in splittedText)
                        {
                            string formattedText = string.Empty;
                            Op ops = Op.Equals;
                            if (splittedText.Count() > 1)
                                formattedText = string.Format("*{0}*", text);
                            else
                                formattedText = text;

                            ops = GetOperator(new KeyValuePair<object, string>(formattedText, entityProperty.Value.Value));
                            var filter = new Filter()
                            {
                                GroupName = grpName,
                                PropertyName = entityProperty.Key,
                                Value = formattedText.TrimStart('*').TrimEnd('*'),
                                Operator = ops
                            };

                            if (index > 0)
                                filter.GroupOperator = groupLogicalOps;

                            grpFilter.Filters.Add(filter);
                            index = index + 1;
                        }

                        if (predictFilter.Count > 0)
                            grpFilter.GroupOperator = LogicalOperator.And;

                        predictFilter.Add(grpFilter);
                    }
                }
            }
            return LinqExpressionExtension.GetMongoExpression<T>(predictFilter, paramName);
        }

        private static IList<KeyValuePair<string, KeyValuePair<object, string>>> GetProperties<T>(T entity) where T : class
        {
            var result = new List<KeyValuePair<string, KeyValuePair<object, string>>>();
            if (entity != null)
            {
                var type = entity.GetType();
                var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (var prop in properties)
                {
                    var selfValue = type.GetProperty(prop.Name).GetValue(entity, null);
                    if (selfValue != null && !prop.PropertyType.Name.Contains("ICollection"))
                    {
                        KeyValuePair<object, string> valueOperator = new KeyValuePair<object, string>();
                        if (prop.PropertyType.GenericTypeArguments.Length == 1)
                            valueOperator = new KeyValuePair<object, string>(selfValue, prop.PropertyType.GenericTypeArguments[0].Name);
                        else
                            valueOperator = new KeyValuePair<object, string>(selfValue, prop.PropertyType.Name);

                        var propItem = new KeyValuePair<string, KeyValuePair<object, string>>(prop.Name, valueOperator);
                        result.Add(propItem);
                    }
                }
            }
            return result;
        }

        private static Expression<Func<T, bool>> GetExpression<T>(IList<Filter> filters, string paramName = "")
        {
            if (filters.Count == 0)
                return null;

            Expression exp = null;
            ParameterExpression param = null;
            if (string.IsNullOrEmpty(paramName))
                param = Expression.Parameter(typeof(T));
            else
                param = Expression.Parameter(typeof(T), paramName);

            if (filters.Count == 1)
                exp = GetExpression<T>(param, filters[0]);
            else if (filters.Count == 2)
                exp = GetExpression<T>(param, filters[0], filters[1]);
            else
            {
                while (filters.Count > 0)
                {
                    var f1 = filters[0];
                    var f2 = filters[1];

                    if (exp == null)
                        exp = GetExpression<T>(param, filters[0], filters[1]);
                    else
                        exp = Expression.AndAlso(exp, GetExpression<T>(param, filters[0], filters[1]));

                    filters.Remove(f1);
                    filters.Remove(f2);

                    if (filters.Count == 1)
                    {
                        exp = Expression.AndAlso(exp, GetExpression<T>(param, filters[0]));
                        filters.RemoveAt(0);
                    }
                }
            }

            return Expression.Lambda<Func<T, bool>>(exp, param);
        }

        private static Expression<Func<T, bool>> GetMongoExpression<T>(IList<GroupFilter> grpFilters, string paramName = "")
        {
            if (grpFilters?.Count <= 0)
                return null;

            Expression outerExp = null;
            ParameterExpression param = null;
            if (string.IsNullOrEmpty(paramName))
                param = Expression.Parameter(typeof(T));
            else
                param = Expression.Parameter(typeof(T), paramName);

            foreach (var grpFilter in grpFilters)
            {
                LogicalOperator outerGrpOp = grpFilter.GroupOperator;
                var filters = grpFilter.Filters;
                int count = 0;
                Expression innerExp = null;
                foreach (var filter in filters)
                {
                    if (count == 0)
                        innerExp = GetExpression<T>(param, filters[count]);
                    else
                        innerExp = MergeExpressions(filter.GroupOperator, innerExp, GetExpression<T>(param, filters[count]));

                    count = count + 1;
                }
                outerExp = MergeExpressions(outerGrpOp, outerExp, innerExp);
            }

            return outerExp == null ? null : Expression.Lambda<Func<T, bool>>(outerExp, param);
        }

        private static Expression GetExpression<T>(ParameterExpression param, Filter filter)
        {
            MemberExpression member = Expression.Property(param, filter.PropertyName);
            ConstantExpression constant = Expression.Constant(filter.Value, member.Type);
            switch (filter.Operator)
            {
                case Op.NotEquals:
                    return Expression.NotEqual(member, constant);

                case Op.GreaterThan:
                    return Expression.GreaterThan(member, constant);

                case Op.GreaterThanOrEqual:
                    return Expression.GreaterThanOrEqual(member, constant);

                case Op.LessThan:
                    return Expression.LessThan(member, constant);

                case Op.LessThanOrEqual:
                    return Expression.LessThanOrEqual(member, constant);

                case Op.Contains:
                    {
                        Expression left = Expression.Call(member, typeof(string).GetMethod("ToLower", System.Type.EmptyTypes));
                        Expression right = Expression.Call(constant, typeof(string).GetMethod("ToLower", System.Type.EmptyTypes));
                        return Expression.Call(left, containsMethod, right);
                    }
                case Op.StartsWith:
                    return Expression.Call(member, startsWithMethod, constant);

                case Op.EndsWith:
                    return Expression.Call(member, endsWithMethod, constant);
                default:
                    return Expression.Equal(member, constant);
            }
        }

        private static Expression MergeExpressions(LogicalOperator logicalOperator, Expression leftExpr, Expression rightExpr)
        {
            if (leftExpr != null && rightExpr != null)
            {
                if (logicalOperator == LogicalOperator.And)
                    return Expression.AndAlso(leftExpr, rightExpr);
                else if (logicalOperator == LogicalOperator.Or)
                    return Expression.OrElse(leftExpr, rightExpr);
                else
                    return null;
            }
            else if (leftExpr != null)
                return leftExpr;
            else
                return rightExpr;
        }

        private static BinaryExpression GetExpression<T>(ParameterExpression param, Filter filter1, Filter filter2)
        {
            Expression bin1 = GetExpression<T>(param, filter1);
            Expression bin2 = GetExpression<T>(param, filter2);

            return Expression.AndAlso(bin1, bin2);
        }

        private static bool IsValueApplicableForFilter(KeyValuePair<object, string> obj)
        {
            if (Type.GetTypeCode(obj.Key.GetType()) == TypeCode.Int16 && (Int16)obj.Key == 0)
                return false;
            else if (Type.GetTypeCode(obj.Key.GetType()) == TypeCode.Int32 && (int)obj.Key == 0)
                return false;
            else if (Type.GetTypeCode(obj.Key.GetType()) == TypeCode.Int64 && (Int64)obj.Key == 0)
                return false;
            else if (Type.GetTypeCode(obj.Key.GetType()) == TypeCode.String && string.IsNullOrWhiteSpace((string)obj.Key))
                return false;
            else if (Type.GetTypeCode(obj.Key.GetType()) == TypeCode.String && (string.IsNullOrWhiteSpace((string)obj.Key) || (string)obj.Key == "\0"))
                return false;
            else if (Type.GetTypeCode(obj.Key.GetType()) == TypeCode.DateTime && Convert.ToDateTime(obj.Key) == default(DateTime))
                return false;
            else if (Type.GetTypeCode(obj.Key.GetType()) == TypeCode.Decimal && (Decimal)obj.Key == 0)
                return false;
            else if (Type.GetTypeCode(obj.Key.GetType()) == TypeCode.Byte && (Byte)obj.Key == 0)
                return false;
            else
                return true;
        }

        private static Op GetOperator(KeyValuePair<object, string> valueAndType)
        {
            //Meanwhile considering only like operator later will consider other.
            if (valueAndType.Value.ToLower() == "string" && valueAndType.Key.ToString().HasEvoWildCardChar())
            {
                string value = valueAndType.Key.ToString();
                if (value.StartsWith("*") && value.EndsWith("*"))
                    return Op.Contains;
                else if (value.StartsWith("*"))
                    return Op.EndsWith;
                else if (value.EndsWith("*"))
                    return Op.StartsWith;
            }

            return Op.Equals;
        }

        public static Expression<Func<T, bool>> CombinePredicates<T>(this IList<Expression<Func<T, bool>>> predicateExpressions, Func<Expression, Expression, BinaryExpression> logicalFunction)
        {
            Expression<Func<T, bool>> filter = null;

            if (predicateExpressions.Count > 0)
            {
                var firstPredicate = predicateExpressions[0];
                Expression body = firstPredicate.Body;
                for (int i = 1; i < predicateExpressions.Count; i++)
                {
                    body = logicalFunction(body, Expression.Invoke(predicateExpressions[i], firstPredicate.Parameters));
                }
                filter = Expression.Lambda<Func<T, bool>>(body, firstPredicate.Parameters);
            }

            return filter;
        }
        /// <summary>
        /// usage :
        ///var combined = TryCombiningExpressions(c => c.FirstName == "Dog", c => c.LastName == "Boy");
        ///  public static Func<FullName, bool> TryCombiningExpressions(Expression<Func<FullName, bool>> func1, Expression<Func<FullName, bool>> func2)
        ///{
        ///    return func1.CombineWithAndAlso(func2).Compile();
        ///   }
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <param name="func1"></param>
        /// <param name="func2"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> CombineWithAndAlso<T>(this Expression<Func<T, bool>> func1, Expression<Func<T, bool>> func2)
        {
            return Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(
                    func1.Body, new ExpressionParameterReplacer(func2.Parameters, func1.Parameters).Visit(func2.Body)),
                func1.Parameters);
        }

        public static Expression<Func<T, bool>> CombineWithOrElse<T>(this Expression<Func<T, bool>> func1, Expression<Func<T, bool>> func2)
        {
            return Expression.Lambda<Func<T, bool>>(
                Expression.OrElse(
                    func1.Body, new ExpressionParameterReplacer(func2.Parameters, func1.Parameters).Visit(func2.Body)),
                func1.Parameters);
        }

        private class ExpressionParameterReplacer : ExpressionVisitor
        {
            public ExpressionParameterReplacer(IList<ParameterExpression> fromParameters, IList<ParameterExpression> toParameters)
            {
                ParameterReplacements = new Dictionary<ParameterExpression, ParameterExpression>();
                for (int i = 0; i != fromParameters.Count && i != toParameters.Count; i++)
                    ParameterReplacements.Add(fromParameters[i], toParameters[i]);
            }

            private IDictionary<ParameterExpression, ParameterExpression> ParameterReplacements { get; set; }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                ParameterExpression replacement;
                if (ParameterReplacements.TryGetValue(node, out replacement))
                    node = replacement;
                return base.VisitParameter(node);
            }
        }
    }


    public class Filter
    {
        public string PropertyName { get; set; }
        public Op Operator { get; set; }
        public object Value { get; set; }
        public LogicalOperator GroupOperator { get; set; } = LogicalOperator.None;
        public string GroupName { get; set; }
    }

    public class GroupFilter
    {
        public LogicalOperator GroupOperator { get; set; }

        public IList<Filter> Filters { get; set; }
    }

    public enum LogicalOperator
    {
        None,
        And,
        Or
    }

    public enum Op
    {
        Equals,
        NotEquals,
        GreaterThan,
        LessThan,
        GreaterThanOrEqual,
        LessThanOrEqual,
        Contains,
        StartsWith,
        EndsWith,
        Like
    }
}

