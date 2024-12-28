using System.Net;
using System.Net.Sockets;
using MediatR;
using Microsoft.Extensions.Logging;
using RtspServer.Application.Commands;
using RtspServer.Application.Queries;
using RtspServer.Domain.Models.RtspResponses;

namespace RtspServer.Infrastructure.Models;

public class ClientConnection
{
    private readonly TcpClient _client;
    private readonly ISender _sender;
    private readonly ILogger<ClientConnection> _logger;
    private long? _sessionId;

    public ClientConnection(TcpClient client, ISender sender, ILogger<ClientConnection> logger)
    {
        _client = client;
        _sender = sender;
        _logger = logger;
    }

    public async Task ProcessAsync()
    {
        var stream = _client.GetStream();
        var array = new byte[ushort.MaxValue];

        while (true)
        {
            _ = await stream.ReadAsync(array);


            var parseCommand = new ParseRequestCommand(
                array, ((IPEndPoint)stream.Socket.RemoteEndPoint!).Address);
            var rtspRequest = await _sender.Send(parseCommand);

            var handleCommand = new HandleRequestCommand(rtspRequest);
            var rtspResponse = await _sender.Send(handleCommand);

            if (rtspResponse is RtspSetupResponse setupResponse && _sessionId is null)
            {
                _sessionId = setupResponse.SessionId;
            }

            var sendResponseCommand = new SendResponseCommand(stream, rtspResponse);
            await _sender.Send(sendResponseCommand);
        }
    }

    public async Task CloseAsync()
    {
        if (_sessionId is null) return;

        var session = await _sender.Send(new GetSessionQuery(_sessionId.Value));
        if (session is not null)
        {
            await session.CloseAsync();
        }
    }
}