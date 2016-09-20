using System.Linq;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.Extensions.Options;

namespace Digipolis.Web.Guidelines.Versioning
{
    public class VersionConstraint : IActionConstraint
    {
        private readonly string[] _acceptedVersions;
        public VersionConstraint(string[] acceptedVersions)
        {
            Order = -1;
            _acceptedVersions = acceptedVersions;
        }

        public int Order { get; set; }

        public bool Accept(ActionConstraintContext context)
        {
            var options = (DigipolisOptions)context.RouteContext.HttpContext.RequestServices.GetService(typeof(DigipolisOptions));
            if (!options.EnableVersioning) return true;

            var versionValue = context.RouteContext.RouteData.Values["apiVersion"];
            if (versionValue == null) return false;

            return _acceptedVersions.Contains(versionValue.ToString());
        }
    }
}