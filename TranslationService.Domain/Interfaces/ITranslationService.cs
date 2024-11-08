using TranslationService.Domain.Models;

namespace TranslationService.Domain.Interfaces;

public interface ITranslationService
{
    Task<IList<TranslationResponse>> TranslateAsync(IList<TranslationRequest> requests);
    Task<ServiceInfo> GetInfoAsync();
}