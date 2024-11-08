using Grpc.Core;
using Grpc.Net.Client;
using TranslationService.Domain.Interfaces;
using TranslationService.Domain.Models;
using TranslationService.Generated;
using System.Linq;
namespace TranslationService.Grpc;

public class GrpcService : Generated.TranslationService.TranslationServiceBase
{
    private readonly ITranslationService _translationService;
    public GrpcService(ITranslationService translationService)
    {
        _translationService = translationService;
    }

    public override async Task<TranslateResponses> Translate(TranslateRequests requests, ServerCallContext context)
    {
        IList<TranslationRequest> requestsConv = requests.Requests.Select(x => GrpcMapper.Map(x)).ToList();
        var response = await _translationService.TranslateAsync(requestsConv);
        var responseConv = response.Select(x => GrpcMapper.Map(x)).ToList();
        return new TranslateResponses{
            Responses =  {  responseConv}
        };
    }

    public override async Task<InfoResponse> GetInfo(InfoRequest request, ServerCallContext context)
    {
        var response = await _translationService.GetInfoAsync();
        return GrpcMapper.Map(response);
    }
}