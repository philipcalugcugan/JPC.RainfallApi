using JPC.Application.RainfallService;
using JPC.Application.Shared.Rainfall.Configuration;
using JPC.RainfallApi.Middlewares;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace JPC.RainfallApi
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
            // Configure services here, similar to what's in the CreateBuilder block in Program.cs
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Rainfall API", Version = "v1" });
                options.SwaggerDoc("notifier", new OpenApiInfo { Title = "Rainfall API", Version = "v1" });
                options.DocInclusionPredicate((docName, apiDesc) =>
                {
                    if (docName == "v1" && string.IsNullOrEmpty(apiDesc.GroupName))
                    {
                        return true;
                    }
                    return apiDesc.GroupName == docName;
                });
            });

            // Add your services here
            services.Configure<RainfallApiConfiguration>(Configuration.GetSection("RainfallApi"));
            services.AddTransient(provider => provider.GetRequiredService<IOptions<RainfallApiConfiguration>>().Value);
            services.AddTransient<IRainfallService, RainfallService>();
            services.AddHttpClient();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Configure the request pipeline

            if (env.IsDevelopment() || env.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
