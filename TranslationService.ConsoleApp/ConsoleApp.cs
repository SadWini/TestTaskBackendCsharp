using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TranslationService.Caching.DbContexts;
using TranslationService.Caching.Services;
using TranslationService.Domain.Interfaces;
using TranslationService.Domain.Models;

class ConsoleApp
{
    static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        
        using var scope = host.Services.CreateScope();
        var serviceProvider = scope.ServiceProvider;
        var translationService = serviceProvider.GetRequiredService<ITranslationService>();
        while (true)
        {
            var translatedText = await translationService.TranslateAsync(
                new List<TranslationRequest>{CreateRequest()});
            var info = await translationService.GetInfoAsync();
            WriteResult(translatedText[0], info);
        }
    }

    private static TranslationRequest CreateRequest()
    {
        Console.WriteLine("Введите текст для перевода:");
        var text = Console.ReadLine();

        Console.WriteLine("Введите исходный язык (например, 'en'):");
        var fromLang = Console.ReadLine();

        Console.WriteLine("Введите язык перевода (например, 'ru'):");
        var toLang = Console.ReadLine();
        return new TranslationRequest()
        {
            Text = text,
            FromLanguage = fromLang,
            ToLanguage = toLang
        };
    }

    private static void WriteResult(TranslationResponse req, ServiceInfo info)
    {
        Console.WriteLine($"Перевод: {req.TranslatedText} " +
                          $"{Environment.NewLine}" +
                          $"Current size of cache is {info.CacheSize}" + 
                          $"{Environment.NewLine}"+
                          $"{info.ServiceName})");
    }
    
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(config =>
            {
                config.SetBasePath(Directory.GetCurrentDirectory()) 
                    .AddJsonFile($"appsettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureServices((context, services) =>
            {
                var connectionString = context.Configuration.GetConnectionString("DefaultConnection");
                
                services.AddDbContext<CacheContext>(options =>
                    options.UseNpgsql(connectionString));
                
                services.AddSingleton<ICacheService, EfCacheService>();
                services.AddSingleton<ITranslationService, TranslationService.Domain.Services.TranslationService>();
            });
}