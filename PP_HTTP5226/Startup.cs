using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PP_HTTP5226.Startup))]
namespace PP_HTTP5226
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
