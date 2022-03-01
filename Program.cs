using System;
using Figgle;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Steeltoe.Extensions.Configuration.ConfigServer;
using Steeltoe.Extensions.Configuration.Placeholder;

namespace TwilioPOC
{
    public static class Program
    {
        private static readonly string envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        private static readonly string rootDir = Environment.CurrentDirectory;

        private static readonly Func<IConfigurationBuilder, IConfigurationBuilder> _configBuider = (x) => x
                 .SetBasePath(rootDir)
                 .AddJsonFile($"Configuration/appsettings.json", optional: true)
                 .AddJsonFile($"Configuration/appsettings{(string.IsNullOrEmpty(envName) ? string.Empty : $".{envName}")}.json", optional: true)
                 .AddEnvironmentVariables();

        public static void Main(string[] args)
        {
            Console.WriteLine(FiggleFonts.Standard.Render("Configurator Service, N5."));

            IConfiguration configuration = _configBuider(new ConfigurationBuilder()).BuildAndReplacePlaceholders();

            if (configuration.GetSection("Serilog").Value != null)
            {
                Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(configuration)
                    .CreateLogger();
            }
            else
            {
                Log.Logger = new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .WriteTo.Console()
                    .CreateLogger();
            }

            try
            {
                Log.Information("Starting up");
                var host = CreateHostBuilder(args, configuration).Build();
                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHostBuilder CreateHostBuilder(string[] args, IConfiguration configuration) =>
            WebHost.CreateDefaultBuilder(args)
            .UseKestrel()
            .UseIISIntegration()
            .UseSerilog()
            .UseConfiguration(configuration)
            .UseDefaultServiceProvider((context, options) => { })
            .AddSpringCloudConfigService()
            .UseStartup<Startup>();

        private static IWebHostBuilder AddSpringCloudConfigService(this IWebHostBuilder webHostBuilder)
            => string.IsNullOrEmpty(Environment.GetEnvironmentVariable("Spring:Cloud:Config:Uri")) && string.IsNullOrEmpty(Environment.GetEnvironmentVariable("Spring__Cloud__Config__Uri"))
                ? webHostBuilder
                : webHostBuilder.AddConfigServer()
                                .ConfigureAppConfiguration((hostingContext, config) =>
                                {
                                    config.AddEnvironmentVariables();
                                    config.AddPlaceholderResolver();
                                });
    }
}