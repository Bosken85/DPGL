using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Digipolis.Web.Guidelines.Helpers
{
    public static class OrderExtensions
    {
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string ordering)
        {
            var type = typeof(T);
            var property = type.GetProperty(ordering);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);
            MethodCallExpression resultExp = Expression.Call(typeof(IQueryable), "OrderBy", new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExp));
            return source.Provider.CreateQuery<T>(resultExp);
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, params string[] ordering)
        {
            IQueryable<T> result = source;
            var type = typeof(T);
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(x=> x.CanRead && ordering.Contains(x.Name, StringComparer.CurrentCultureIgnoreCase)).ToList();
            for (int i = 0; i < properties.Count(); i++)
            {
                var property = properties.ElementAt(i);
                var parameter = Expression.Parameter(type, "p");
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExp = Expression.Lambda(propertyAccess, parameter);
                MethodCallExpression resultExp = i == 0 
                    ? Expression.Call(typeof(Queryable), "OrderBy", new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExp))
                    : Expression.Call(typeof(Queryable), "ThenBy", new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExp));
                result = source.Provider.CreateQuery<T>(resultExp);
            }
            return result;
        }

        public static IQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string ordering)
        {
            var type = typeof(T);
            var property = type.GetProperty(ordering);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);
            MethodCallExpression resultExp = Expression.Call(typeof(IQueryable), "OrderByDescending", new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExp));
            return source.Provider.CreateQuery<T>(resultExp);
        }
    }
}
