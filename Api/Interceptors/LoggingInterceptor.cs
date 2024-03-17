using Grpc.Core;
using Grpc.Core.Interceptors;
using System.Threading.Tasks;


namespace Api.Interceptors
{
    using Grpc.Core;
    using Grpc.Core.Interceptors;
    using System.Threading.Tasks;

    public class LoggingInterceptor : Interceptor
    {
        private readonly ILogger<LoggingInterceptor> _logger;

        public LoggingInterceptor(ILogger<LoggingInterceptor> logger)
        {
            _logger = logger;
        }

        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
            TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            _logger.LogInformation($"Request: {context.Method} {request}");
            try
            {
                var response = await continuation(request, context);
                _logger.LogInformation($"Response: {context.Method} {response}");
                return response;
            }
            catch (RpcException ex)
            {
                _logger.LogError($"gRPC Error: {ex.Status.Detail}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Server Error: {ex.Message}");
                throw new RpcException(new Status(StatusCode.Internal, "Internal server error"));
            }
        }
    }

}
