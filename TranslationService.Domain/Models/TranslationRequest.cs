using System.ComponentModel.DataAnnotations;

namespace TranslationService.Domain.Models;

public class TranslationRequest
{
    [Required]
    public string Text { get; set; }              
    [Required]
    public string FromLanguage { get; set; }    
    [Required]
    public string ToLanguage { get; set; }
}