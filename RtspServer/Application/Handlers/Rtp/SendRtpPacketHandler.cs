using System.Net.Sockets;
using MediatR;
using RtspServer.Application.Commands;

namespace RtspServer.Application.Handlers.Rtp;

public class SendRtpPacketHandler : IRequestHandler<SendDataCommand>
{
    private readonly UdpClient _client;

    public SendRtpPacketHandler(UdpClient client)
    {
        _client = client;
    }

    public async Task Handle(SendDataCommand request, CancellationToken cancellationToken)
    {
        var (data, endpoint) = request;

        await _client.SendAsync(data, endpoint, cancellationToken);
    }
}