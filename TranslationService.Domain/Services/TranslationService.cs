using TranslationService.Domain.Interfaces;
using TranslationService.Domain.Models;

namespace TranslationService.Domain.Services;

public class TranslationService : ITranslationService
{
    private readonly ICacheService _cacheService;
    private readonly string _apiServiceName = "Google Translate API";  // TO DO: use real API

    public TranslationService(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    public async Task<TranslationResponse> TranslateAsync(TranslationRequest request)
    {
        var cachedTranslation = await _cacheService.GetCachedTranslationAsync(request);
        if (!string.IsNullOrEmpty(cachedTranslation))
            return new TranslationResponse()
            {
                TranslatedText = cachedTranslation
            };
        
        var translatedText = $"{request.Text} - translated"; // TO DO: use real API
        TranslationResponse response = new TranslationResponse()
        {
            FromLanguage = request.FromLanguage,
            OriginalText = request.Text,
            ToLanguage = request.ToLanguage,
            TranslatedText = translatedText
        };
        await _cacheService.CacheTranslationAsync(response);
        return new TranslationResponse()
        {
            TranslatedText = translatedText
        };
    }

    public async Task<ServiceInfo> GetInfoAsync()
    {
        var cacheSize = await _cacheService.GetCacheSizeAsync();
        return new ServiceInfo
        {
            ServiceName = _apiServiceName,
            CacheSize = cacheSize
        };
    }
}