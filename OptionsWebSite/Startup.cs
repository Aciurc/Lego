using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OptionsWebSite.Startup))]
namespace OptionsWebSite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
