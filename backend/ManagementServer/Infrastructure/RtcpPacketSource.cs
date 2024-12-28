using System.Threading.Channels;
using RtspServer.Domain.Abstract;
using RtspServer.Domain.Models.Abstract;
using RtspServer.Domain.Models.Sessions;

namespace ManagementServer.Infrastructure;

public class RtcpPacketSource : IRtcpPacketSource
{
    private readonly Channel<RtcpPacket> _packetsChannel;
    public RtcpPacketSource(RtspSession session)
    {
        _packetsChannel = Channel.CreateBounded<RtcpPacket>(new BoundedChannelOptions(10)
        {
            FullMode = BoundedChannelFullMode.DropWrite
        });
    }

    public async Task<RtcpPacket> GetPacketAsync()
    {
        return await _packetsChannel.Reader.ReadAsync();
    }

    public async Task AppendPacketAsync(RtcpPacket rtpPacket)
    {
        await _packetsChannel.Writer.WriteAsync(rtpPacket);
    }
}