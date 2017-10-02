using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BoardgameClub.Startup))]
namespace BoardgameClub
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
