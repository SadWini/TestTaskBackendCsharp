using TranslationService.Domain.Models;
using TranslationService.Generated;

namespace TranslationService.Grpc;

public class GrpcMapper
{
    public static TranslateResponse Map(TranslationResponse response)
    {
        return new TranslateResponse()
        {
            TranslatedText = response.TranslatedText
        };
    }

    public static TranslationRequest Map(TranslateRequest request)
    {
        return new TranslationRequest()
        {
            Text = request.Text,
            FromLanguage = request.FromLang,
            ToLanguage = request.ToLang
        };
    }
    
    public static InfoResponse Map(ServiceInfo response)
    {
        return new InfoResponse()
        {
            CacheType = response.CacheType,
            ServiceName = response.ServiceName
        };
    }
}