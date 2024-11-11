using Grpc.Core;
using Grpc.Net.Client;
using TranslationService.Generated;
using TranslationService.Grpc;

class GrpcClientApp
{
    static async Task Main(string[] args)
    {
        using var channel = GrpcChannel.ForAddress("http://localhost:5001", new GrpcChannelOptions
        {
            Credentials = ChannelCredentials.Insecure
        });
        
        var client = new TranslationService.Generated.TranslationService.TranslationServiceClient(channel);

        while (true)
        {
            try
            {
                var translateRequest = CreateRequest();
                var translateRequests = new TranslateRequests
                {
                    Requests = { translateRequest }
                };
                
                var translateResponse = await client.TranslateAsync(translateRequests);
                
                var infoResponse = await client.GetInfoAsync(new InfoRequest());

                WriteResult(translateResponse, infoResponse);
            }
            catch (RpcException ex)
            {
                Console.WriteLine($"gRPC error: {ex.Status.Detail}");
            }
        }
    }

    private static TranslateRequest CreateRequest()
    {
        Console.WriteLine("Введите текст для перевода:");
        var text = Console.ReadLine();

        Console.WriteLine("Введите исходный язык (например, 'en'):");
        var fromLang = Console.ReadLine();

        Console.WriteLine("Введите язык перевода (например, 'ru'):");
        var toLang = Console.ReadLine();

        return new TranslateRequest
        {
            Text = text,
            FromLang = fromLang,
            ToLang = toLang
        };
    }

    private static void WriteResult(TranslateResponses translateResponses, InfoResponse infoResponse)
    {
        foreach (var response in translateResponses.Responses)
        {
            Console.WriteLine($"Перевод: {response.TranslatedText}");
        }
        Console.WriteLine($"Текущий размер кэша: {infoResponse.CacheSize}");
        Console.WriteLine($"Имя сервиса перевода: {infoResponse.ServiceName}");
    }
}