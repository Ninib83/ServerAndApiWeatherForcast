using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;

[assembly: OwinStartup(typeof(WeatherForcastApi.Startup))]
[assembly: OwinStartupAttribute(typeof(WeatherForcastApi.Startup))]

namespace WeatherForcastApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Map("/signalr", map =>
            {
                map.UseCors(CorsOptions.AllowAll);
                var hubConfiguration = new HubConfiguration
                {
                    EnableJSONP = true
                };

                map.RunSignalR(hubConfiguration);
            });
        }
    }
}
