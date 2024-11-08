using Grpc.Core;
using Grpc.Net.Client;
using TranslationService.Domain.Interfaces;
using TranslationService.Domain.Models;
using TranslationService.Generated;

namespace TranslationService.Grpc;

public class GrpcService : Generated.TranslationService.TranslationServiceBase
{
    private readonly ITranslationService _translationService;
    public GrpcService(ITranslationService translationService)
    {
        _translationService = translationService;
    }

    public override async Task<TranslateResponse> Translate(TranslateRequest request, ServerCallContext context)
    {
        var response = await _translationService.TranslateAsync(GrpcMapper.Map(request));
        return GrpcMapper.Map(response);
    }

    public override async Task<InfoResponse> GetInfo(InfoRequest request, ServerCallContext context)
    {
        var response = await _translationService.GetInfoAsync();
        return GrpcMapper.Map(response);
    }
}