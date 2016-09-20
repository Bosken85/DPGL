using Swashbuckle.SwaggerGen.Application;

namespace Digipolis.Web.Guidelines.Swagger
{
    public abstract class SwaggerSettings<TResponseGuidelines> where TResponseGuidelines : ResponseGuidelines
    {
        public void Configure(SwaggerGenOptions options)
        {
            options.OperationFilter<TResponseGuidelines>();
            Configuration(options);
        }

        protected abstract void Configuration(SwaggerGenOptions options);
    }
}