using System.Net.Sockets;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RtspServer.Attributes;
using RtspServer.RtspController;
using RtspServer.RtspResponses;

namespace RtspServer.Rtsp;

public sealed class RtspConnectionContext
{
    private readonly TcpClient _client;
    private readonly CancellationToken _stoppingToken;
    private readonly ILogger<RtspConnectionContext> _logger;
    private readonly RtspRequestParser _requestParser;
    private readonly Type _controllerType;
    private readonly IServiceScopeFactory _scopeFactory;

    public RtspConnectionContext(
        TcpClient client,
        CancellationToken stoppingToken,
        ILogger<RtspConnectionContext> logger,
        RtspRequestParser requestParser,
        IServiceScopeFactory scopeFactory)
    {
        _client = client;
        _logger = logger;
        _requestParser = requestParser;
        _scopeFactory = scopeFactory;
        _stoppingToken = stoppingToken;
        
        _controllerType = Assembly.GetExecutingAssembly().GetTypes()
            .Single(t => t.BaseType == typeof(RtspControllerBase));
    }
    
    public delegate RtspConnectionContext Factory(TcpClient client, CancellationToken stoppingToken);

    public async Task ServeAsync()
    {
        var stream = _client.GetStream();
        
        while (!_stoppingToken.IsCancellationRequested)
        {
            var request = await _requestParser.ParseStreamAsync(stream, _stoppingToken);
            _logger.LogDebug("Received request:\n{request}", request.ToString().Trim());
            
            await using var scope = _scopeFactory.CreateAsyncScope();
            var controller = ActivatorUtilities.CreateInstance(scope.ServiceProvider, _controllerType);

            var suitableMethod = controller.GetType().GetMethods()
                .SingleOrDefault(t => t.GetCustomAttribute<BaseRtspMethodAttribute>()?.SupportedMethod == request.Method);

            if (suitableMethod is null)
            {
                await stream.WriteAsync(RtspControllerBase.NotImplemented().Compile(request), _stoppingToken);
                _logger.LogError("No suitable method found for {method} request", request.Method);
                return;
            }

            if (!suitableMethod.ReturnType.IsAssignableTo(typeof(IRtspResponse)))
            {
                await stream.WriteAsync(RtspControllerBase.NotImplemented().Compile(request), _stoppingToken);
                _logger.LogError("Method must return an IRtspResponse");
                return;
            }

            var args = new object[suitableMethod.GetParameters().Length];

            foreach (var parameterType in suitableMethod.GetParameters().Select(pi => pi.ParameterType))
            {
                if (parameterType == typeof(RtspRequest))
                {
                    args[0] = request;
                }
            }
            
            var response = (IRtspResponse)suitableMethod.Invoke(controller, args.ToArray())!;
            
            await stream.WriteAsync(response.Compile(request), _stoppingToken);
            
            _logger.LogDebug("Send response:\n{response}", Encoding.UTF8.GetString(response.Compile(request)));
        }
    }
}