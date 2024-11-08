using System.ComponentModel.DataAnnotations;

namespace TranslationService.Domain.Models;

public class TranslationResponse
{
    [Required]
    public string OriginalText { get; set; }      
    [Required]
    public string TranslatedText { get; set; }   
    [Required]
    public string FromLanguage { get; set; }      
    [Required]
    public string ToLanguage { get; set; }
}