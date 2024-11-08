using System.ComponentModel.DataAnnotations;

namespace TranslationService.Caching.Entities;

public class TranslationCache
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string OriginalText { get; set; }
    [Required]
    public string TranslatedText { get; set; }
    [Required]
    public string FromLanguage { get; set; }
    [Required]
    public string ToLanguage { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}