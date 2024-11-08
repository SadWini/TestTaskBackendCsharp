using Microsoft.EntityFrameworkCore;
using TranslationService.Caching.Entities;

namespace TranslationService.Caching.DbContexts;

public class CacheContext : DbContext 
{
    public DbSet<TranslationCache> TranslationCaches { get; set; }
    
    public CacheContext(DbContextOptions<CacheContext> options) : base(options)
    {
    }
}