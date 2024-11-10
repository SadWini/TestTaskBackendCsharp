using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TranslationService.Caching.DbContexts;
using TranslationService.Caching.Services;
using TranslationService.Grpc.Validators;
using TranslationService.Domain.Interfaces;
using FluentValidation;
using TranslationService.Generated;
using TranslationService.Grpc.Interceptors;

namespace TranslationService.Grpc;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services.AddGrpc(op=>{
            op.Interceptors.Add<LoggerInterceptor>();
            op.Interceptors.Add<ExceptionInterceptor>();
        });
        services.AddGrpcReflection();

        services.AddSingleton<ITranslationService, Domain.Services.TranslationService>();
        services.AddDbContext<CacheContext>(options =>
            options.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"))); 
        services.AddSingleton<ICacheService, EfCacheService>();
        services.AddScoped<GrpcService>();
        services.AddValidatorsFromAssemblyContaining<TranslateRequest>();
    }
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGrpcService<GrpcService>();
            endpoints.MapGrpcReflectionService();
        });
    }
}