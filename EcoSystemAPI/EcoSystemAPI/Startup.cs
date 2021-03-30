using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecosystem.util.helpers;
using EcosystemAPI.util.services;
using EcoSystemAPI.Context.Models;
using EcoSystemAPI.Data;
using EcoSystemAPI.Data.Context;
using EcoSystemAPI.Services;
using EcoSystemAPI.uow.Interfaces;
using EcoSystemAPI.uow.Repositories;
using EcoSystemAPI.util.services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore;

namespace EcoSystemAPI
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
            services.Configure<ApplicationSettings>(Configuration.GetSection("ApplicationSettings"));
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.AddDbContext<EcosystemContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("EcosystemAppCon")));
            //Enable Cors




            services.AddCors(options =>
            {
                options.AddPolicy("EnableCORS", builder =>
                {
                    builder.AllowAnyOrigin()
                       .AllowAnyHeader()
                       .AllowAnyMethod();
                });
            });

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAccountsRepo, SqlAccountsRepo>();
            services.AddScoped<IMerchandiseRepo, SqlMerchandiseRepo>();
            services.AddScoped<ITeamsRepo, SqlTeamsRepo>();
            services.AddScoped<IArticlesRepo, SqlArticlesRepo>();
            services.AddScoped<IAuthenticationRepos, SqlAuthenticationRepo>();
            services.AddTransient<IMailService, SendGridMailService>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.ConfigureCors();
            services.ConfigureIISIntegration();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });


            //JSON Serializer
            services.AddControllers()
                .AddNewtonsoftJson(s => {
                    s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();

            app.UseCors("EnableCORS");
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();

            app.UseAuthorization();
            app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Photos")),
                RequestPath = "/Photos"
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "My API V1");
            });
        }
    }
}
    