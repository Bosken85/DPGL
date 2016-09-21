namespace Digipolis.Web.Guidelines.Mvc
{
    /// <summary>
    /// QueryOptions for filtering an Api
    /// </summary>
    public class PageFilter
    {
        /// <summary>
        /// Sort by fields. Multiple values can be specified by comma seperation.
        /// Default sorting is ascending, for descending add a -(minus) sign before the field.
        /// </summary>
        public string[] Sort { get; set; }

        /// <summary>
        /// Retrieve page
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// Size of the page
        /// </summary>
        public int PageSize { get; set; } = 10;
    }
}