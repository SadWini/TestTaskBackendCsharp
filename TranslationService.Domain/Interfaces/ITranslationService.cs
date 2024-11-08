using TranslationService.Domain.Models;

namespace TranslationService.Domain.Interfaces;

public interface ITranslationService
{
    Task<TranslationResponse> TranslateAsync(TranslationRequest request);
    Task<ServiceInfo> GetInfoAsync();
}