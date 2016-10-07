using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Digipolis.Web.Guidelines.Helpers;
using Digipolis.Web.Guidelines.Models;
using Microsoft.AspNetCore.Routing;

namespace Digipolis.Web.Guidelines.Mvc
{
    public static class PageOptionsExtensions
    {
        private static Link GenerateLink(PageOptions query, int page, string actionName, string controllerName, object routeValues = null)
        {
            var values = new RouteValueDictionary(routeValues)
            {
                ["Page"] = page,
                ["PageSize"] = query.PageSize,
                ["Sort"] = query.Sort
            };

            var url = LinkProvider.AbsoluteAction(actionName, controllerName, values);
            return new Link(url);
        }

        private static Link GenerateLink(PageOptions query, int page, string routeName, object routeValues = null)
        {
            var values = new RouteValueDictionary(routeValues)
            {
                ["Page"] = page,
                ["PageSize"] = query.PageSize,
                ["Sort"] = query.Sort
            };

            var url = LinkProvider.AbsoluteRoute(routeName, values);
            return new Link(url);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="data"></param>
        /// <param name="total"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="routeValues"></param>
        /// <returns></returns>
        public static PagedResult<T> ToPagedResult<T>(this PageOptions query, IEnumerable<T> data, int total, string actionName, string controllerName, object routeValues = null) where T : class, new()
        {
            if (string.IsNullOrWhiteSpace(actionName)) throw new ArgumentNullException(nameof(actionName));
            if (string.IsNullOrWhiteSpace(controllerName)) throw new ArgumentNullException(nameof(controllerName));

            var result = new PagedResult<T>(query.Page, query.PageSize, total, data)
            {
                Links =
                {
                    First = GenerateLink(query, 1, actionName, controllerName, routeValues),
                    Self = GenerateLink(query, query.Page, actionName, controllerName, routeValues)
                }
            };
            result.Links.Last = GenerateLink(query, result.Page.TotalPages, actionName, controllerName, routeValues);

            if (query.Page - 1 > 0) result.Links.Previous = GenerateLink(query, query.Page - 1, actionName, controllerName, routeValues);
            if (query.Page + 1 < result.Page.TotalPages) result.Links.Next = GenerateLink(query, query.Page + 1, actionName, controllerName, routeValues);

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="data"></param>
        /// <param name="total"></param>
        /// <param name="routeName"></param>
        /// <param name="routeValues"></param>
        /// <returns></returns>
        public static PagedResult<T> ToPagedResult<T>(this PageOptions query, IEnumerable<T> data, int total, string routeName, object routeValues = null) where T : class, new()
        {
            if (string.IsNullOrWhiteSpace(routeName)) throw new ArgumentNullException(nameof(routeName));

            var result = new PagedResult<T>(query.Page, query.PageSize, total, data)
            {
                Links =
                {
                    First = GenerateLink(query, 1, routeName, routeValues),
                    Self = GenerateLink(query, query.Page, routeName, routeValues)
                }
            };
            result.Links.Last = GenerateLink(query, result.Page.TotalPages, routeName, routeValues);

            if (query.Page - 1 > 0) result.Links.Previous = GenerateLink(query, query.Page - 1,  routeName, routeValues);
            if (query.Page + 1 < result.Page.TotalPages) result.Links.Next = GenerateLink(query, query.Page + 1, routeName, routeValues);

            return result;
        }

        public static IOrderedQueryable<T> OrderByQuery<T>(this IQueryable<T> source, params string[] orderFields) where T : class
        {
            if (orderFields == null || !orderFields.Any()) return source.OrderBy<T, object>(x => null);

            IOrderedQueryable<T> result = null;
            for (int i = 0; i < orderFields.Length; i++)
            {
                var descending = orderFields[i].StartsWith("-");
                var field = descending ? orderFields[i].Remove(0, 1) : orderFields[i];
                if (i == 0) result = descending ? source.OrderByDescending(field) : source.OrderBy(field);
                else result = descending ? result.ThenByDescending(field) : result.ThenBy(field);
            }
            MethodCallExpression expression = (result != null ? result.Expression : source.Expression) as MethodCallExpression;
            return (IOrderedQueryable<T>)source.Provider.CreateQuery<T>(expression);
        }
    }
}