using TranslationService.Domain.Models;

namespace TranslationService.Domain.Interfaces;

public interface ICacheService
{
    Task CacheTranslationAsync(TranslationResponse response);
    Task<string?> GetCachedTranslationAsync(TranslationRequest request);
    public Task<string> GetCacheSizeAsync();
}