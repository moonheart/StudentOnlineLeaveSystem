using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LeaveSystem.Web.Startup))]
namespace LeaveSystem.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
