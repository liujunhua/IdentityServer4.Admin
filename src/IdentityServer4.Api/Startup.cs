using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Skoruba.IdentityServer4.Admin.BusinessLogic.Extensions;
using Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Dtos.Identity;
using Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Extensions;
using Skoruba.IdentityServer4.Admin.Configuration.Interfaces;
using Skoruba.IdentityServer4.Admin.EntityFramework.DbContexts;
using Skoruba.IdentityServer4.Admin.EntityFramework.Identity.Entities.Identity;
using Skoruba.IdentityServer4.Admin.Helpers;
using Swashbuckle.AspNetCore.Swagger;

namespace IdentityServer4.Api
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="env"></param>
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                    .SetBasePath(env.ContentRootPath)
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables();
            Configuration = builder.Build();

            HostingEnvironment = env;
        }

        /// <summary>
        /// 
        /// </summary>
        public IConfigurationRoot Configuration { get; }

        /// <summary>
        /// 
        /// </summary>
        public IHostingEnvironment HostingEnvironment { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureRootConfiguration(Configuration);
            var rootConfiguration = services.BuildServiceProvider().GetService<IRootConfiguration>();

            services.AddDbContexts<AdminDbContext>(HostingEnvironment, Configuration);
            services.AddAuthenticationServices<AdminDbContext, UserIdentity, UserIdentityRole>(HostingEnvironment, rootConfiguration.AdminConfiguration);
            services.AddAuthentication(Configuration["IdentityService:DefaultScheme"])
            .AddIdentityServerAuthentication(options =>
            {
                options.Authority = Configuration["IdentityService:Authority"];
                options.RequireHttpsMetadata = Convert.ToBoolean(Configuration["IdentityService:RequireHttpsMetadata"]);
                options.ApiName = Configuration["IdentityService:ApiName"];
            }); 
            //services.AddMvcExceptionFilters();

            services.AddAdminServices<AdminDbContext>();

            services.AddAdminAspNetIdentityServices<AdminDbContext, UserDto<int>, int, RoleDto<int>, int, int, int,
                                UserIdentity, UserIdentityRole, int, UserIdentityUserClaim, UserIdentityUserRole,
                                UserIdentityUserLogin, UserIdentityRoleClaim, UserIdentityUserToken>();
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
            });
            services.AddMvcLocalization();
            //services.AddAuthorizationPolicies();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("IdentityAdminService", new Info { Title = "IdentityService API", Version = "v1" });
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "IdentityServer4.Api.xml"));
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    In = "header",
                    Description = "请输入带有Bearer的Token",
                    Name = "Authorization",
                    Type = "apiKey"
                });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> {
                    {
                        "Bearer",
                        Enumerable.Empty<string>()
                    }
                });
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            app.UseSwagger(c =>
            {
                c.RouteTemplate = "{documentName}/swagger.json";
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/IdentityAdminService/swagger.json", "IdentityAdminServiceApi");
            });
            //app.AddLogging(loggerFactory, Configuration);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseSecurityHeaders();
            app.UseStaticFiles();
            app.ConfigureAuthentificationServices(env);
            app.ConfigureLocalization();

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
