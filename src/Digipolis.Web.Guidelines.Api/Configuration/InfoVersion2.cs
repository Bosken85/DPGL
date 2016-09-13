using Swashbuckle.Swagger.Model;

namespace Digipolis.Web.Guidelines.Api.Configuration
{
    /// <summary>
    /// Contains all information for V2 of the API
    /// </summary>
    public class InfoVersion2 : Info
    {
        public InfoVersion2() : base()
        {
            this.Version = Settings.Versions.V2;
            this.Title = "API V2";
            this.Description = "Description for V2 of the API";
            this.Contact = new Contact { Email = "info@digipolis.be", Name = "Digipolis", Url = "https://www.digipolis.be" };
            this.Extensions.Add("Kevin", "Gelukt!");
            this.TermsOfService = "https://www.digipolis.be/tos";
            this.License = new License
            {
                Name = "My License",
                Url = "https://www.digipolis.be/licensing"
            };
        }
    }
}