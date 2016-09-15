using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

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
}
