using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Skoruba.IdentityServer4.Admin.EntityFramework.DbContexts;
using Skoruba.IdentityServer4.Admin.EntityFramework.Identity.Entities.Identity;
using Skoruba.IdentityServer4.STS.Identity.Filters;
using Skoruba.IdentityServer4.STS.Identity.Helpers;
using StackExchange.Redis;
using Swashbuckle.AspNetCore.Swagger;
using System;
using WebApiClient.Extensions.DependencyInjection;

namespace Skoruba.IdentityServer4.STS.Identity
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }
        public ILogger Logger { get; set; }

        public Startup(IHostingEnvironment environment, ILoggerFactory loggerFactory)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            if (environment.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            Configuration = builder.Build();
            Environment = environment;
            Logger = loggerFactory.CreateLogger<Startup>();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //var redisDefaultConnection = Configuration["Redis:ConnectionStrings:DefaultConnection"];
            //var redis = ConnectionMultiplexer.Connect(redisDefaultConnection);
            //services.AddDataProtection().SetApplicationName("session_application_name").PersistKeysToStackExchangeRedis(redis, "DataProtection-Keys");
            //services.AddDistributedServiceStackRedisCache(options =>
            //{
            //    options.Host = "172.16.1.245";
            //    options.Port = 6379;
            //    //options.InstanceName = "redis_session";
            //});

            services.AddDbContexts<AdminDbContext>(Configuration);
            services.AddAuthenticationServices<AdminDbContext, UserIdentity, UserIdentityRole>(Environment, Configuration, Logger);
            services.AddMvcLocalization();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 4;
                options.Password.RequiredUniqueChars = 1;
            });
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder.AllowAnyHeader().AllowAnyMethod().AllowCredentials().AllowAnyOrigin();
                    });
            });
            services.AddSession(options => { options.IdleTimeout = TimeSpan.FromSeconds(1000); });

            services.AddHttpApi<IUserService>().ConfigureHttpApiConfig((c, p) =>
            {
                c.HttpHost = new Uri(Configuration["ApiGateway:BaseUrl"]);
                c.FormatOptions.DateTimeFormat = "yyyy-MM-dd HH:mm:ss.fff";
                c.LoggerFactory = p.GetRequiredService<ILoggerFactory>();
                c.GlobalFilters.Add(new TokenFilter($"{Configuration["ApiGateway:BaseUrl"]}/connect/token", Configuration["Service:Client:Id"], Configuration["Service:Client:Secret"]));
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("IdentityService", new Info { Title = "IdentityService API", Version = "v1" });
                //c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Skoruba.IdentityServer4.STS.Identity.xml"));
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseCors("AllowAllOrigins");
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "{documentName}/swagger.json";
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/IdentityService/swagger.json", "IdentityServiceApi");
            });

            app.AddLogging(loggerFactory, Configuration);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSession();
            app.UseSecurityHeaders();
            app.UseStaticFiles();
            app.UseIdentityServer();
            app.UseMvcLocalizationServices();
            app.UseMvcWithDefaultRoute();
        }
    }
}
