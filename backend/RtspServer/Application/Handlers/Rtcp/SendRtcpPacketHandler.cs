using System.Net.Sockets;
using MediatR;
using RtspServer.Application.Commands;
using RtspServer.Domain.Models.Abstract;
using RtspServer.Domain.Models.Rtcp;
using RtspServer.Extensions;

namespace RtspServer.Application.Handlers.Rtcp;

public class SendRtcpPacketHandler : IRequestHandler<SendRtcpPacketCommand>
{
    private readonly UdpClient _udpClient;

    public SendRtcpPacketHandler(UdpClient udpClient)
    {
        _udpClient = udpClient;
    }

    public async Task Handle(SendRtcpPacketCommand request, CancellationToken cancellationToken)
    {
        var (packet, endpoint) = request;

        await _udpClient.SendAsync(SerializePacket(packet), endpoint, cancellationToken);
    }

    private byte[] SerializePacket(RtcpPacket packet)
    {
        return packet switch
        {
            SenderReport senderReport => SerializeSenderReport(senderReport),
            _ => throw new ArgumentOutOfRangeException(nameof(packet))
        };
    }

    private byte[] SerializeSenderReport(SenderReport senderReport)
    {
        var serializedData = new byte[28];

        serializedData[0] = (byte)(uint)(RtcpPacket.Version << 6
                                         | ((senderReport.Padding ? 1 : 0) << 5)
                                         | senderReport.ReceptionCount);

        serializedData[1] = senderReport.PayloadType;
        
        senderReport.Length.ToBigEndian().CopyTo(serializedData, 2);
        senderReport.SSRC.ToBigEndian().CopyTo(serializedData, 4);
        
        senderReport.NtpTimestamp.ToBigEndian().CopyTo(serializedData, 8);
        senderReport.RTS.ToBigEndian().CopyTo(serializedData, 16);
        senderReport.SPC.ToBigEndian().CopyTo(serializedData, 20);
        senderReport.SOC.ToBigEndian().CopyTo(serializedData, 24);

        return serializedData;
    }
}