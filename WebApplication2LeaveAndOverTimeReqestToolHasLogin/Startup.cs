using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebApplication2LeaveAndOverTimeReqestToolHasLogin.Startup))]
namespace WebApplication2LeaveAndOverTimeReqestToolHasLogin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
