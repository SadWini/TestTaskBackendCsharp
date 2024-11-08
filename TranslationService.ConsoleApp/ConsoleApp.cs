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
            Console.WriteLine("Введите текст для перевода:");
            var text = Console.ReadLine();

            Console.WriteLine("Введите исходный язык (например, 'en'):");
            var fromLang = Console.ReadLine();

            Console.WriteLine("Введите язык перевода (например, 'ru'):");
            var toLang = Console.ReadLine();
            TranslationRequest req = new TranslationRequest()
            {
                Text = text,
                FromLanguage = fromLang,
                ToLanguage = toLang
            };
            var translatedText = await translationService.TranslateAsync(req);
            var info = await translationService.GetInfoAsync();
            Console.WriteLine($"Перевод: {translatedText} + " +
                              $"{Environment.NewLine}" +
                              $"{info.CacheType}" + 
                              $"{info.ServiceName})");
        }
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