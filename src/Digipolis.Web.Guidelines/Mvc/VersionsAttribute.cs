using System;
using Digipolis.Web.Guidelines.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace Digipolis.Web.Guidelines.Mvc
{
    public class VersionsAttribute : Attribute, IActionConstraintFactory
    {
        public VersionsAttribute(params string[] acceptedVersions)
        {
            AcceptedVersions = acceptedVersions;
        }

        public string[] AcceptedVersions { get; }

        public bool IsReusable => true;

        public IActionConstraint CreateInstance(IServiceProvider services)
        {
            return new VersionConstraint(AcceptedVersions);
        }
    }
}
