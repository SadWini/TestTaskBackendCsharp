using Grpc.Core;
using TranslationService.Domain.Interfaces;
using TranslationService.Domain.Models;
using TranslationService.Generated;
using System.Linq;
using FluentValidation;

namespace TranslationService.Grpc;

public class GrpcService : Generated.TranslationService.TranslationServiceBase
{
    private readonly ITranslationService _translationService;
    private readonly IValidator<TranslateRequest> _translateRequestValidator;
    public GrpcService(ITranslationService translationService, 
        IValidator<TranslateRequest> translateRequestValidator)
    {
        _translationService = translationService;
        _translateRequestValidator = translateRequestValidator;
    }

    public override async Task<TranslateResponses> Translate(TranslateRequests requests, ServerCallContext context)
    {
        IList<TranslationRequest> requestsConv = requests.Requests.Select(request =>
        {
            var result = _translateRequestValidator.Validate(request);
            if (!result.IsValid)
            {
                throw new FormatException($"Invalid TranslateRequest {request}");
            }
            return GrpcMapper.Map(request);
        }).ToList();
        var response = await _translationService.TranslateAsync(requestsConv);
        var responseConv = response.Select(x => GrpcMapper.Map(x)).ToList();
        return new TranslateResponses 
        {
            Responses =  {  responseConv}
        };
    }

    public override async Task<InfoResponse> GetInfo(InfoRequest request, ServerCallContext context)
    {
        var response = await _translationService.GetInfoAsync();
        return GrpcMapper.Map(response);
    }
}