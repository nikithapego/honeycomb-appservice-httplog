using System;
using Honeycomb.AppService;
using Honeycomb.Models;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Startup))]
namespace Honeycomb.AppService
{
    public class Startup : FunctionsStartup
    {

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            var context = builder.GetContext();

            builder.ConfigurationBuilder
                .AddEnvironmentVariables();
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddOptions();
            builder.Services.AddLogging();
            builder.Services.AddHttpClient();
            builder.Services
                .AddOptions<HoneycombApiSettings>()
                .Configure<IConfiguration>((settings, configuration) => {
                    configuration.GetSection(typeof(HoneycombApiSettings).Name).Bind(settings);
                });
            builder.Services.AddTransient<IHoneycombService, HoneycombService>();
        }
    }
}