using System.Data;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TranslationService.Caching.DbContexts;
using TranslationService.Caching.Entities;
using TranslationService.Caching.Util;
using TranslationService.Domain.Interfaces;
using TranslationService.Domain.Models;

namespace TranslationService.Caching.Services;

public class EfCacheService : ICacheService
{
    private readonly CacheContext _context;

    public EfCacheService(CacheContext context)
    {
        _context = context;
    }

    public async Task CacheTranslationAsync(TranslationResponse response)
    {
        _context.TranslationCaches.Add(Converter.Map(response));
        await _context.SaveChangesAsync();
        
    }

    public async Task<string?> GetCachedTranslationAsync(TranslationRequest request)
    {
        var cacheEntry = await _context.TranslationCaches
            .FirstOrDefaultAsync(t => t.OriginalText == request.Text 
                                      && t.FromLanguage == request.FromLanguage
                                      && t.ToLanguage == request.ToLanguage);
        return cacheEntry?.TranslatedText;
    }
    
    public async Task<string> GetCacheSizeAsync()
    {
        const string query = "SELECT pg_size_pretty(pg_database_size(current_database())) AS database_size;";
        var result = _context.Database.SqlQueryRaw<string>(query);

        return result.ToList().First();
    }
}