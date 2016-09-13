using System.Linq;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace Digipolis.Web.Guidelines
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
            var versionValue = context.RouteContext.RouteData.Values["apiVersion"];
            if (versionValue == null) return false;

            return _acceptedVersions.Contains(versionValue.ToString());
        }
    }
}