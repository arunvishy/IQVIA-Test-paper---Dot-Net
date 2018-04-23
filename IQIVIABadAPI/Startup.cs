using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IQVIABadAPI.Startup))]
namespace IQVIABadAPI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
