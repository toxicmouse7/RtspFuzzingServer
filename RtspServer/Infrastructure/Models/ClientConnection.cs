using System.Net;
using System.Net.Sockets;
using MediatR;
using RtspServer.Application.Commands;
using RtspServer.Domain.Models.RtspResponses;

namespace RtspServer.Infrastructure.Models;

public class ClientConnection
{
    private readonly TcpClient _client;
    private readonly ISender _sender;

    public ClientConnection(TcpClient client, ISender sender)
    {
        _client = client;
        _sender = sender;
    }

    public async Task ProcessAsync()
    {
        var stream = _client.GetStream();
        var array = new byte[ushort.MaxValue];

        while (true)
        {
            _ = await stream.ReadAsync(array);

            try
            {
                var parseCommand = new ParseRequestCommand(
                    array, ((IPEndPoint)stream.Socket.RemoteEndPoint!).Address);
                var rtspRequest = await _sender.Send(parseCommand);

                var handleCommand = new HandleRequestCommand(rtspRequest);
                var rtspResponse = await _sender.Send(handleCommand);
                
                var sendResponseCommand = new SendResponseCommand(stream, rtspResponse);
                await _sender.Send(sendResponseCommand);
            }
            catch (Exception e)
            {
                var notImplemented = new RtspNotImplementedResponse();
                await _sender.Send(new SendResponseCommand(stream, notImplemented));
            }
        }
    }
}