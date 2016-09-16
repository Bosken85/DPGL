using System;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Digipolis.Web.Guidelines.Helpers
{
    public static class OrderExtensions
    {
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> query, string propertyname)
        {
            var param = Expression.Parameter(typeof(T), "x");
            var prop = Expression.PropertyOrField(param, propertyname);
            var sortLambda = Expression.Lambda(prop, param);

            Expression<Func<IOrderedQueryable<T>>> sortMethod = () => query.OrderBy<T, object>(k => null);

            var methodCallExpression = (sortMethod.Body as MethodCallExpression);
            var method = methodCallExpression.Method.GetGenericMethodDefinition();
            var genericSortMethod = method.MakeGenericMethod(typeof(T), prop.Type);
            var orderedQuery = (IOrderedQueryable<T>)genericSortMethod.Invoke(query, new object[] { query, sortLambda });

            return orderedQuery;
        }

        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> query, string propertyname)
        {
            var param = Expression.Parameter(typeof(T), "x");
            var prop = Expression.PropertyOrField(param, propertyname);
            var sortLambda = Expression.Lambda(prop, param);

            Expression<Func<IOrderedQueryable<T>>> sortMethod = () => query.OrderByDescending<T, object>(k => null);

            var methodCallExpression = (sortMethod.Body as MethodCallExpression);
            var method = methodCallExpression.Method.GetGenericMethodDefinition();
            var genericSortMethod = method.MakeGenericMethod(typeof(T), prop.Type);
            var orderedQuery = (IOrderedQueryable<T>)genericSortMethod.Invoke(query, new object[] { query, sortLambda });

            return orderedQuery;
        }

        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> query, string propertyname)
        {
            var param = Expression.Parameter(typeof(T), "x");
            var prop = Expression.PropertyOrField(param, propertyname);
            var sortLambda = Expression.Lambda(prop, param);

            Expression<Func<IOrderedQueryable<T>>> sortMethod = () => query.ThenBy<T, object>(k => null);

            var methodCallExpression = (sortMethod.Body as MethodCallExpression);
            var method = methodCallExpression.Method.GetGenericMethodDefinition();
            var genericSortMethod = method.MakeGenericMethod(typeof(T), prop.Type);
            var orderedQuery = (IOrderedQueryable<T>)genericSortMethod.Invoke(query, new object[] { query, sortLambda });

            return orderedQuery;
        }

        public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> query, string propertyname)
        {
            var param = Expression.Parameter(typeof(T), "x");
            var prop = Expression.PropertyOrField(param, propertyname);
            var sortLambda = Expression.Lambda(prop, param);

            Expression<Func<IOrderedQueryable<T>>> sortMethod = () => query.ThenByDescending<T, object>(k => null);

            var methodCallExpression = (sortMethod.Body as MethodCallExpression);
            var method = methodCallExpression.Method.GetGenericMethodDefinition();
            var genericSortMethod = method.MakeGenericMethod(typeof(T), prop.Type);
            var orderedQuery = (IOrderedQueryable<T>)genericSortMethod.Invoke(query, new object[] { query, sortLambda });

            return orderedQuery;
        }
    }

    public static class SelectExtensions
    {
        public static IQueryable<T> Select<T>(this IQueryable<T> source, params string[] fields) where T : class
        {
            var param = Expression.Parameter(typeof(T), "x");
            var newStatement = Expression.New(typeof(ExpandoObject));
            var bindings = fields.Select(x =>
            {
                var field = typeof(ExpandoObject).GetProperty(x);
                var prop = Expression.PropertyOrField(param, x);
                return Expression.Bind(field, prop);
            });

            var xInit = Expression.MemberInit(newStatement, bindings);
            var lambda = Expression.Lambda<Func<T, ExpandoObject>>(xInit, param);
            return source.Provider.CreateQuery<T>(lambda);

        }

        //public static IQueryable<T> Select<T>(this IQueryable<T> source, params string[] fields) where T : class
        //{
        //    TypeBuilder.
        //    var param = Expression.Parameter(typeof(T), "x");
        //    var bindings = fields.Select<MemberAssignment>(x =>
        //    {
        //        var prop = Expression.PropertyOrField(param, x);
        //        test[x] = prop.Type.GetTypeInfo().IsValueType ? Activator.CreateInstance(prop.Type) : null;
        //        var field = test.GetType().GetProperty(x);
        //        return Expression.Bind(field, prop);
        //    });

        //    var newStatement = Expression.New(test.GetType());
        //    var xInit = Expression.MemberInit(newStatement, bindings);
        //    var lambda = Expression.Lambda<Func<T, ExpandoObject>>(xInit, param);
        //    return source.Provider.CreateQuery<T>(lambda);
        //}
    }
}
