using ApiCoreEcommerce.Infrastructure.Extensions;
using AspNetCore.RouteAnalyzer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSwag;
using NSwag.Generation.Processors.Security;

namespace ApiCoreEcommerce
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDb(Configuration);
            services.AddJwtAuthentication(Configuration);
            services.AddMvcCoreFramework(Configuration);
            services.AddAppServices(Configuration);
            services.AddAppAuthorization(Configuration);
            services.AddRouteAnalyzer();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //Register the Swagger services      
            //services.AddOpenApiDocument(c =>
            //{
            //    c.Title = "Ecommerce API";
            //});

            services.AddOpenApiDocument(doc =>
            {
                doc.AddSecurity("bearer", new NSwag.OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Description = "Bearer token authorization header",
                });

                doc.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("bearer"));
            });


            // services.AddMemoryCache();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseCors("AllowAll");
            app.UseMvc(routes =>
            {
                routes.MapRouteAnalyzer("/routes");
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            //Register the Swagger generator      
            app.UseOpenApi();
            app.UseSwaggerUi3();

        }
    }
}