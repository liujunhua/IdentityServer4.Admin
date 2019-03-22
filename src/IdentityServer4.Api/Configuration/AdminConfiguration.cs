using Skoruba.IdentityServer4.Admin.Configuration.Interfaces;

namespace Skoruba.IdentityServer4.Admin.Configuration
{
    public class AdminConfiguration : IAdminConfiguration
    {
        public string IdentityAdminBaseUrl { get; set; } = "http://localhost:9999";// "http://192.168.10.133:7001";

        public string IdentityAdminRedirectUri { get; set; } = "http://localhost:9999/signin-oidc";// "http://192.168.10.133:7001/signin-oidc";

        public string IdentityServerBaseUrl { get; set; } = "http://192.168.10.133:7000";
    }
}
