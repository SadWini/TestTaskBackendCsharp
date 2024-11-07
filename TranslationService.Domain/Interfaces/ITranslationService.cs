namespace TranslationService.Domain.Interfaces;

public interface ITranslationService
{
    string TranslateText(string text, string sourceLanguage, string targetLanguage);
    
}