using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Reflection;
using yortrip.DbContexts;
using yortrip.Services;

namespace yortrip
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
            services.AddControllers().AddNewtonsoftJson(s =>
            {
                s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            services.AddScoped<ICalendarRepository, CalendarRepository>();
            services.AddScoped<IInviteRepository, InviteRepository>();
            services.AddScoped<IUserRepositroy, UserRepository>();
            services.AddScoped<IUnavailabilityRepository, UnavailabilityRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddDbContext<CalenderContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("ProductionConnection"));
            });

            // Automapper configuration
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Swagger Configuration
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                    new Microsoft.OpenApi.Models.OpenApiInfo
                    {
                        Title = "Swagger YorTrip API",
                        Description = "YorTrip API Swagger",
                        Version = "v1"
                    });

                // Name of the XML to include comments to swagger
                var fileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

                // Directory of the file name for the path
                var filePath = Path.Combine(AppContext.BaseDirectory, fileName);

                // Include the comments to the API and Schema on Swagger UI
                options.IncludeXmlComments(filePath);
            });

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = "https://securetoken.google.com/yortrip-294c6";
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = "https://securetoken.google.com/yortrip-294c6",
                        ValidateAudience = true,
                        ValidAudience = "yortrip-294c6",
                        ValidateLifetime = true
                    };
                });


            string configValue = Configuration.GetValue<string>("CORSComplianceDomains");
            string[] CORSComplianceDomains = configValue.Split("|,|");

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins("https://yortrips-ui.herokuapp.com")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });

                options.AddPolicy("AllowMyOrigin",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors("AllowMyOrigin");
            //app.UseCors(options => options.WithOrigins("https://yortrips-ui.herokuapp.com").AllowAnyHeader().AllowAnyMethod());

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Swagger Configuration
            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger YorTrip API");
                options.RoutePrefix = "";
            });

        }
    }
}
