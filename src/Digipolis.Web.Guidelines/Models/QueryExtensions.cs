using System.Collections.Generic;
using Digipolis.Web.Guidelines.Helpers;
using Microsoft.AspNetCore.Routing;

namespace Digipolis.Web.Guidelines.Models
{
    public static class HelperExtensions
    {
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
        public static PagedResult<T> ToPagedResult<T>(this Query query, IEnumerable<T> data, int total, string actionName, string controllerName, object routeValues = null) where T : class, new()
        {
            var result = new PagedResult<T>(query.Page, query.PageSize, total, data);
            var values = new RouteValueDictionary(routeValues)
            {
                ["Page"] = query.Page,
                ["PageSize"] = query.PageSize,
                ["Fields"] = query.Fields,
                ["Sort"] = query.Sort
            };

            values["Page"] = 1;
            var first = UrlHelper.AbsoluteAction(actionName, controllerName, values);
            result.Links.Add("first", new Link(first));

            values["Page"] = query.Page - 1;
            if (query.Page - 1 > 0)
            {
                var previous = UrlHelper.AbsoluteAction(actionName, controllerName, values);
                result.Links.Add("prev", new Link(previous));
            }
            
            values["Page"] = query.Page;
            var self = UrlHelper.AbsoluteAction(actionName, controllerName, values);
            result.Links.Add("self", new Link(self));

            values["Page"] = query.Page + 1;
            if (query.Page + 1 < result.Page.TotalPages)
            {
                var next = UrlHelper.AbsoluteAction(actionName, controllerName, values);
                result.Links.Add("next", new Link(next));
            }

            values["Page"] = result.Page.TotalPages;
            var last = UrlHelper.AbsoluteAction(actionName, controllerName, values);
            result.Links.Add("last", new Link(last));

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
        public static PagedResult<T> ToPagedResult<T>(this Query query, IEnumerable<T> data, int total, string routeName, object routeValues = null) where T : class, new()
        {
            var result = new PagedResult<T>(query.Page, query.PageSize, total, data);
            var values = new RouteValueDictionary(routeValues)
            {
                ["Page"] = query.Page,
                ["PageSize"] = query.PageSize,
                ["Fields"] = query.Fields,
                ["Sort"] = query.Sort
            };

            values["Page"] = 1;
            var first = UrlHelper.AbsoluteRoute(routeName, values);
            result.Links.Add("first", new Link(first));

            values["Page"] = query.Page - 1;
            if (query.Page - 1 > 0)
            {
                var previous = UrlHelper.AbsoluteRoute(routeName, values);
                result.Links.Add("prev", new Link(previous));
            }

            values["Page"] = query.Page;
            var self = UrlHelper.AbsoluteRoute(routeName, values);
            result.Links.Add("self", new Link(self));

            values["Page"] = query.Page + 1;
            if (query.Page + 1 < result.Page.TotalPages)
            {
                var next = UrlHelper.AbsoluteRoute(routeName, values);
                result.Links.Add("next", new Link(next));
            }

            values["Page"] = result.Page.TotalPages;
            var last = UrlHelper.AbsoluteRoute(routeName, values);
            result.Links.Add("last", new Link(last));

            return result;
        }
    }
}