using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Ycode.TestRailClient.Test.ApiMock
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'ver. 'V";
                options.SubstituteApiVersionInUrl = true;
            });

            services.AddOpenApiDocument(settings =>
            {
                settings.DocumentName = "v2";
                settings.Version = "2";
                settings.Title = "TestRail API Mock";
                settings.ApiGroupNames = new[] { "ver. 2" };
            });

            services.AddSingleton<IAccessLogV2>(provider => new AccessLogV2());
            services.AddSingleton<AccessLogV2>(provider => (AccessLogV2)provider.GetService<IAccessLogV2>());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.Use(async (context, next) =>
            {
                if (context.Request.Path.Value.StartsWith("/index.php"))
                {
                    context.Request.Path = context.Request.Query.First().Key;
                }
                await next();
            });

            if (env.IsDevelopment())
            {
                app.UseExceptionHandler("/api/error/dev");
            }
            else
            {
                app.UseExceptionHandler("/api/error");
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseApiVersioning();
            app.UseOpenApi();
            app.UseSwaggerUi3();
        }
    }
}
