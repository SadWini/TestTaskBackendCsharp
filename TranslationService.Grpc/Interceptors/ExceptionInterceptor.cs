using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;

namespace TranslationService.Grpc.Interceptors;

public class ExceptionInterceptor:Interceptor
{
    private readonly ILogger<ExceptionInterceptor> _logger;
    
    public ExceptionInterceptor(ILogger<ExceptionInterceptor> logger)
    {
        _logger = logger;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request,
        ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }
        catch (FormatException ex)
        {
            _logger.LogError(ex, $"Probaly missing field on translateRequest");
            throw new RpcException(new Status(StatusCode.InvalidArgument, ex.Message));
        }
        catch (RpcException ex)
        {
            _logger.LogError(ex, $"got status: {ex.Status}" +
                                  $"{Environment.NewLine} on method {context.Method}");
            throw;
        }
    }
}