using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BoardGameClub.Startup))]
namespace BoardGameClub
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
