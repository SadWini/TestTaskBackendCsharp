using System.ComponentModel.DataAnnotations;

namespace TranslationService.Domain.Models;

public class ServiceInfo
{
    [Required]
    public string ServiceName { get; set; }      
    [Required]
    public string CacheType { get; set; }
}