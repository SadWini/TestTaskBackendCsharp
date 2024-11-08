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

    public async Task<IList<TranslationResponse>> TranslateAsync(IList<TranslationRequest> requests)
    {
        IList<TranslationResponse> answer = new List<TranslationResponse>();
        foreach (var request in requests)
        {
            var cachedTranslation = await _cacheService.GetCachedTranslationAsync(request);
            if (!string.IsNullOrEmpty(cachedTranslation))
            {
                answer.Add(new TranslationResponse()
                {
                    TranslatedText = cachedTranslation
                });
                continue;
            }

            var translatedText = $"{request.Text} - translated"; // TO DO: use real API
            TranslationResponse response = new TranslationResponse()
            {
                FromLanguage = request.FromLanguage,
                OriginalText = request.Text,
                ToLanguage = request.ToLanguage,
                TranslatedText = translatedText
            };
            await _cacheService.CacheTranslationAsync(response);
            answer.Add(new TranslationResponse()
            {
                TranslatedText = translatedText
            });
        }

        return answer;
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