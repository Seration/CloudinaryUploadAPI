using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using CloudinaryMediaUpload.API.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using CloudinaryMediaUpload.API.Filters;
namespace CloudinaryMediaUpload.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }


        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings"));


            services.AddCors();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("CoreSwagger", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Title = "Swagger on ASP.NET Core",
                    Version = "1.0.0",
                    Description = "Try Swagger on (ASP.NET Core 2.1)",
                    Contact = new Contact()
                    {
                        Name = "Swagger Implementation Bora kasmer",
                        Url = "http://borakasmer.com",
                        Email = "bora@borakasmer.com"
                    },
                    TermsOfService = "http://swagger.io/terms/"
                });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", new string[] { } }
                });

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey",
                });
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddScoped<TokenFilter>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseSwagger()
            .UseSwaggerUI(c =>
            {
                //TODO: Either use the SwaggerGen generated Swagger contract (generated from C# classes)
                c.SwaggerEndpoint("/swagger/CoreSwagger/swagger.json", "Swagger Test .Net Core");

                //TODO: Or alternatively use the original Swagger contract that's included in the static files
                // c.SwaggerEndpoint("/swagger-original.json", "Swagger Petstore Original");
            });

            app.UseHttpsRedirection();
            app.UseMvc();

            app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials());

        }
    }
}
