using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Swertres.Web.Startup))]
namespace Swertres.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
