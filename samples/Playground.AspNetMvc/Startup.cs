using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Playground.AspNetMvc.Startup))]
namespace Playground.AspNetMvc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
