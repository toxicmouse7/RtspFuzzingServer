using System.Net;
using System.Net.Sockets;
using RtspServer.Abstract;
using RtspServer.Domain.Models;
using RtspServer.Infrastructure.RTP;

namespace RtspServer.Services;

public class RTPStreamingService : IRTPStreamingService
{
    private readonly byte[] _jpeg;
    private readonly Dictionary<Session, CancellationTokenSource> _stoppingTokens = new();
    
    public RTPStreamingService()
    {
        _jpeg = File.ReadAllBytes("Resources/image.jpeg");
    }

    public void StartRTPStream(Session session)
    {
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        
        Task.Run(() =>
        {
            var endpoint = new IPEndPoint(IPAddress.Parse(session.Ip), (ushort)session.RtpPort);
            using var udpClient = new UdpClient();
            short packetsSend = 0;
        
            while (!cts.IsCancellationRequested)
            {
                var rtpPacket = new RTPPacket(
                    (int)DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                    packetsSend,
                    _jpeg);
                Console.WriteLine(udpClient.Send(rtpPacket.ToByteArray(), endpoint));
                packetsSend++;
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
        }, cts.Token);
        
        _stoppingTokens.Add(session, cts);
    }

    public void StopRTPStream(Session session)
    {
        var stoppingToken = _stoppingTokens[session];
        stoppingToken.Cancel();
        _stoppingTokens.Remove(session);
    }
}