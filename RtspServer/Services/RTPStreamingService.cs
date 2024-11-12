using System.Net;
using System.Net.Sockets;
using RtspServer.Abstract;
using RtspServer.Domain.Models;
using RtspServer.Infrastructure.RTP;

namespace RtspServer.Services;

public class RTPStreamingService : IRTPStreamingService
{
    private readonly IDataSource _dataSource;
    private readonly Dictionary<Session, CancellationTokenSource> _stoppingTokens = new();
    
    public RTPStreamingService(IDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public void StartRTPStream(Session session)
    {
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        
        Task.Factory.StartNew(async () =>
        {
            var endpoint = new IPEndPoint(IPAddress.Parse(session.Ip), (ushort)session.RtpPort);
            using var udpClient = new UdpClient();
            short packetsSend = 0;

            var x = 0;
            while (!cts.IsCancellationRequested)
            {
                var unixTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                var rtpHeader = new RTPHeader((int)x, packetsSend);
                var rtpPacket = new RTPPacket(
                    rtpHeader,
                    await _dataSource.GetStreamableDataAsync());
                var data = rtpPacket.ToByteArray();
                udpClient.Send(data, data.Length, endpoint);
                packetsSend++;
                x += 6000;
            }
        }, TaskCreationOptions.LongRunning);
        
        _stoppingTokens.Add(session, cts);
    }

    public void StopRTPStream(Session session)
    {
        var stoppingToken = _stoppingTokens[session];
        stoppingToken.Cancel();
        _stoppingTokens.Remove(session);
    }
}