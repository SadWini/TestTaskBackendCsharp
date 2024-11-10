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
    private readonly IValidator<TranslationRequest> _translationRequestValidator;
    public GrpcService(ITranslationService translationService, 
        IValidator<TranslationRequest> translationRequestValidator)
    {
        _translationService = translationService;
        _translationRequestValidator = translationRequestValidator;
    }

    public override async Task<TranslateResponses> Translate(TranslateRequests requests, ServerCallContext context)
    {
        IList<TranslationRequest> requestsConv = requests.Requests.Select(request =>
        {
            var requestConv = GrpcMapper.Map(request);
            var result = _translationRequestValidator.Validate(requestConv);
            if (!result.IsValid)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid TranslateRequest Item"));
            }
            return requestConv;
        }).ToList();
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