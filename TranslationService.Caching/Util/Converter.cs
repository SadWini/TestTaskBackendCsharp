using TranslationService.Caching.Entities;
using TranslationService.Domain.Models;

namespace TranslationService.Caching.Util;

public static class Converter
{
    public static TranslationCache Map(TranslationResponse response)
    {
        return new TranslationCache()
        {
            OriginalText = response.OriginalText,
            TranslatedText = response.TranslatedText,
            FromLanguage = response.FromLanguage,
            ToLanguage = response.ToLanguage
        };
    }
}