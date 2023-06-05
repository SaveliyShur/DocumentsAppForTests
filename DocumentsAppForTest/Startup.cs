using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Serilog;
using ShopAppForTest.DbLayer;
using ShopAppForTest.Middlewares;
using ShopAppForTest.Models;
using ShopAppForTest.ServiceLayer.AuthLayer;
using ShopAppForTest.ServiceLayer.DocumentLayer;
using ShopAppForTest.SwaggerFilters;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace ShopAppForTest
{
    public class Startup
    {
        private Serilog.ILogger _logger;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            _logger = Log.Logger;

            var tokenHeader = Configuration.GetSection("TokenHeader").Get<string>();

            services.AddSingleton(_logger);

            services.AddSingleton<UserDbLayer>();
            services.AddSingleton<UserTokenDbLayer>();
            services.AddSingleton<DocumentDbLayer>();

            services.AddSingleton<IAuthLayerService, AuthLayerService>(p => new AuthLayerService(p.GetRequiredService<UserTokenDbLayer>(), p.GetRequiredService<UserDbLayer>()));
            services.AddSingleton<IDocumentLayerService, DocumentLayerService>(p => new DocumentLayerService(p.GetRequiredService<DocumentDbLayer>(), p.GetRequiredService<UserDbLayer>()));

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddControllers().AddJsonOptions(opts =>
            {
                var enumConverter = new JsonStringEnumConverter();
                opts.JsonSerializerOptions.Converters.Add(enumConverter);
                opts.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPI", Version = "v1" });
                c.OperationFilter<MyHeaderFilter>(tokenHeader);
                c.DocumentFilter<BasePathDocumentFilter>();
                c.SchemaFilter<SchemaFilter>();
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider provider)
        {
            var tokenHeader = Configuration.GetSection("TokenHeader").Get<string>();
            var tokenDefault = Configuration.GetSection("DefaultToken").Get<string>();

            app.UseRouting();
            app.MapWhen(context => ( context.Request.Path.StartsWithSegments("/api/User") || context.Request.Path.StartsWithSegments("/api/Auth")) 
                && context.Request.Method == HttpMethod.Post.Method, appBuilder =>
            {
                appBuilder.UseRouting();
                appBuilder.UseMiddleware<CheckRegisterTokenMiddleware>(tokenHeader, tokenDefault);
                appBuilder.UseEndpoints(endpoints => endpoints.MapControllers());
            });

            app.MapWhen(context => context.Request.Path.StartsWithSegments("/api"), appBuilder =>
            {
                appBuilder.UseRouting();
                appBuilder.UseMiddleware<CheckTokenMiddleware>(provider.GetRequiredService<IAuthLayerService>(), tokenHeader);
                appBuilder.UseEndpoints(endpoints => endpoints.MapControllers());
            });

            app.MapWhen(context => context.Request.Path.StartsWithSegments("/health"), appBuilder =>
            {
                appBuilder.UseRouting();
                appBuilder.UseEndpoints(endpoints => endpoints.MapControllers());
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI");
            });
        }
    }
}
