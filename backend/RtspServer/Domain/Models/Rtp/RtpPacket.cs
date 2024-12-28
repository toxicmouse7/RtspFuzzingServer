using RtspServer.Domain.Models.Abstract;
using RtspServer.Extensions;

namespace RtspServer.Domain.Models.Rtp;

public record RtpPacket(RtpHeader Header, RtpContentHeader ContentHeader, byte[] Content)
{
    public byte[] Serialize()
    {
        var header = SerializePacketHeader(Header);
        var contentHeader = ContentHeader switch
        {
            RtpJpegHeader jpegHeader => SerializeJpegHeader(jpegHeader),
            _ => throw new ArgumentOutOfRangeException(
                nameof(ContentHeader), ContentHeader.GetType(), "Unsupported content header type")
        };

        return header.Concat(contentHeader).Concat(Content).ToArray();
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

    public static RtpPacket Deserialize(byte[] data)
    {
        // Deserialize RTP Header
        var header = DeserializePacketHeader(data);
        
        int offset = 12 + (header.Extension ? 4 : 0);

        // Deserialize JPEG Header
        var jpegHeader = DeserializeJpegHeader(data, offset);
        offset += 8; // JPEG header is 8 bytes long

        // Extract Content
        var content = data[offset..];

        return new RtpPacket(header, jpegHeader, [])
        {
            Header = header,
            ContentHeader = jpegHeader,
            Content = content
        };
    }
    
    private static RtpHeader DeserializePacketHeader(byte[] data)
    {
        var header = new RtpHeader(
            (data[0] & 0x20) != 0,
            (data[0] & 0x10) != 0,
            (byte)(data[0] & 0x0F),
            (data[1] & 0x80) != 0,
            (byte)(data[1] & 0x7F),
            BitConverter.ToUInt16(data, 2),
            BitConverter.ToInt32(data, 4),
            BitConverter.ToUInt32(data, 8),
            [],
            BitConverter.ToUInt16(data, 14)
        );

        return header;
    }
    
    private static RtpJpegHeader DeserializeJpegHeader(byte[] data, int offset)
    {
        return new RtpJpegHeader
        (
            data[offset],
            data[(offset + 1)..(offset + 4)].ToArray(),
            data[offset + 4],
            data[offset + 5],
            data[offset + 6],
            data[offset + 7]
        );
    }
}