using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(GoldDataWeb.Startup))]
namespace GoldDataWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
