using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Ycode.TestRailClient.Test.ApiMock
{
    public class Program
    {
        public static async Task Main(params string[] args)
        {
            var program = new Program(args);
            using (var tokenSource = new CancellationTokenSource())
            {
                await program.Start(tokenSource.Token);
            }
        }

        private string[] _args;
        private IHost _host;

        public IAccessLogV2 AccessLog => (IAccessLogV2)_host.Services.GetService(typeof(IAccessLogV2));

        public Program(params string[] args)
        {
            _args = args;
            ConfigureLogger();
        }

        public async Task Start(CancellationToken cancellationToken)
        {
            try
            {
                Log.Information("Starting web host...");
                _host = CreateHostBuilder(_args).Build();
                await _host.RunAsync(cancellationToken);
            }
            catch (Exception e)
            {
                Log.Fatal(e, "Web host was terminated unexpectedly.");
                throw e;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseSerilog();

        private void ConfigureLogger()
        {
            var configBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json");
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                configBuilder.AddJsonFile("appsettings.Development.json", true);
            else
                configBuilder.AddJsonFile("appsettings.Production.json", true);
            var configuration = configBuilder.Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }
    }
}
