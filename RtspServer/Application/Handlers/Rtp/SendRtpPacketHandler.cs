using System.Net.Sockets;
using MediatR;
using RtspServer.Application.Commands;
using RtspServer.Domain.Models.Rtp;
using RtspServer.Extensions;

namespace RtspServer.Application.Handlers.Rtp;

public class SendRtpPacketHandler : IRequestHandler<SendRtpPacketCommand>
{
    private readonly UdpClient _client;

    public SendRtpPacketHandler(UdpClient client)
    {
        _client = client;
    }

    public async Task Handle(SendRtpPacketCommand request, CancellationToken cancellationToken)
    {
        var (packet, endpoint) = request;

        await _client.SendAsync(SerializePacket(packet), endpoint, cancellationToken);
    }

    private static byte[] SerializePacket(RtpPacket packet)
    {
        var header = SerializePacketHeader(packet.Header);
        var contentHeader = packet.ContentHeader switch
        {
            RtpJpegHeader jpegHeader => SerializeJpegHeader(jpegHeader),
            _ => throw new ArgumentOutOfRangeException(
                nameof(packet), packet.ContentHeader.GetType(), "Unsupported content header type")
        };

        return header.Concat(contentHeader).Concat(packet.Content).ToArray();
    }

    private static byte[] SerializeJpegHeader(RtpJpegHeader header)
    {
        return
        [
            header.TypeSpecific, ..header.FragmentOffset, header.Type, header.Quantization, header.Width, header.Height
        ];
    }

    private static byte[] SerializePacketHeader(RtpHeader header)
    {
        var serializedData = new byte[12 + (header.Extension ? 4 : 0)];

        serializedData[0] = (byte)((uint)(RtpHeader.Version << 6
                                          | ((header.Padding ? 1 : 0) << 5)
                                          | ((header.Extension ? 1 : 0) << 4))
                                   | header.CSRCCount);

        serializedData[1] = (byte)((uint)((header.Marker ? 1 : 0) << 7)
                                   | (header.PayloadType));

        header.SequenceNumber.ToBigEndian().CopyTo(serializedData, 2);
        header.Timestamp.ToBigEndian().CopyTo(serializedData, 4);
        header.SSRCIdentifier.ToBigEndian().CopyTo(serializedData, 8);

        if (header.Extension)
        {
            RtpHeader.ExtensionsHeaderId.ToBigEndian().CopyTo(serializedData, 12);
            header.HeaderExtensionLength.ToBigEndian().CopyTo(serializedData, 14);
        }

        return serializedData;
    }
}