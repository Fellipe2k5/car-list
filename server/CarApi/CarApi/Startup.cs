using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CarApi.Model.Context;
using CarApi.Business;
using CarApi.Business.Implementations;
using CarApi.Repository;
using Serilog;
using System;
using System.Collections.Generic;
using CarApi.Repository.Generic;
using Microsoft.Net.Http.Headers;
using CarApi.Hypermedia.Filters;
using CarApi.Hypermedia.Enricher;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Rewrite;
using CarApi.Services;
using CarApi.Services.Implementations;
using CarApi.Configurations;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Http;

namespace CarApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var tokenConfigurations = new TokenConfiguration();

            new ConfigureFromConfigurationOptions<TokenConfiguration>(
                    Configuration.GetSection("TokenConfigurations")
                )
                .Configure(tokenConfigurations);

            services.AddSingleton(tokenConfigurations);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = tokenConfigurations.Issuer,
                    ValidAudience = tokenConfigurations.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfigurations.Secret))
                };
            });

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });

            services.AddCors(options => options.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            }));

            services.AddControllers();

            var connection = Configuration["MySQLConnection:MySQLConnectionString"];
            services.AddDbContext<MySQLContext>(options => options.UseMySql(connection, new MySqlServerVersion(new Version(8, 0, 15))));

            if (Environment.IsDevelopment())
            {
                MigrateDatabase(connection);
            }

            services.AddMvc(options =>
            {
                options.RespectBrowserAcceptHeader = true;

                options.FormatterMappings.SetMediaTypeMappingForFormat("xml", MediaTypeHeaderValue.Parse("application/xml"));
                options.FormatterMappings.SetMediaTypeMappingForFormat("json", MediaTypeHeaderValue.Parse("application/json"));
            })
            .AddXmlSerializerFormatters();

            var filterOptions = new HyperMediaFilterOptions();   
            filterOptions.ContentResponseEnricherList.Add(new CarEnricher());

            services.AddSingleton(filterOptions);

            //Versioning API
            services.AddApiVersioning();

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "REST API",
                        Version = "v1",
                        Description = "API RESTful",
                        Contact = new OpenApiContact
                        {
                            Name = "Fellipe Anselmo Gemio",
                            Url = new Uri("https://github.com/Fellipe2k5")
                        }
                    });
            });

            //Dependency Injection

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
                     
            services.AddScoped<ICarBusiness, CarBusinessImplementation>();
            services.AddScoped<ILoginBusiness, LoginBusinessImplementation>();


            services.AddTransient<ITokenService, TokenService>();

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseSwagger();

            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json",
                    "REST API");
            });

            var option = new RewriteOptions();
            option.AddRedirect("^$", "swagger");
            app.UseRewriter(option);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute("DefaultApi", "{controller=values}/{id?}");
            });
        }
        private void MigrateDatabase(string connection)
        {
            try
            {
                var evolveConnection = new MySqlConnector.MySqlConnection(connection);
                var evolve = new Evolve.Evolve(evolveConnection, msg => Log.Information(msg))
                {
                    Locations = new List<string> { "db/migrations", "db/dataset" },
                    IsEraseDisabled = true,
                };
                evolve.Migrate();
            }
            catch (Exception ex)
            {
                Log.Error("Database migration failed", ex);
                throw;
            }
        }
    }
}
