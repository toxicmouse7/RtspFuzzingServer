using System.Threading.Channels;
using ManagementServer.Settings;
using Microsoft.Extensions.Options;
using RtspServer.Domain.Abstract;
using RtspServer.Domain.Models.Rtp;
using RtspServer.Domain.Models.Sessions;

namespace ManagementServer.Infrastructure;

public class RtpPacketSource : IRtpPacketSource
{
    private readonly byte[] _staticJpeg;
    private readonly Channel<RtpPacket> _rtpPacketsChannel;
    private readonly Task _packetsGenerationTask;
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
    private readonly CancellationToken _stoppingToken;

    private int _timstamp;
    private ushort _sequenceNumber;

    public RtpPacketSource(RtspSession session, IOptions<DataSourceSettings> settings)
    {
        _staticJpeg = File.ReadAllBytes(settings.Value.JpegPath);
        _stoppingToken = session.Token;
        _rtpPacketsChannel = Channel.CreateBounded<RtpPacket>(new BoundedChannelOptions(settings.Value.Fps)
        {
            FullMode = BoundedChannelFullMode.DropOldest
        });
        
        _packetsGenerationTask = Task.Run(async () =>
        {
            try
            {
                await GeneratePackets(settings.Value.Fps);
            }
            catch
            {
            }
        });
    }

    private int Timestamp
    {
        get
        {
            var value = _timstamp;
            _timstamp += 6000;
            return value;
        }
    }

    private ushort Sequence
    {
        get
        {
            var value = _sequenceNumber;
            _sequenceNumber++;
            return value;
        }
    }
    
    public async Task<RtpPacket> GetPacketAsync()
    {
        return await _rtpPacketsChannel.Reader.ReadAsync(_stoppingToken);
    }

    public async Task AppendPacketAsync(RtpPacket rtpPacket, AppendSettings appendSettings)
    {
        await _semaphore.WaitAsync(_stoppingToken);
        
        if (appendSettings.UseOriginalPayload)
        {
            rtpPacket = rtpPacket with { Content = _staticJpeg };
        }

        if (appendSettings.UseOriginalTimestamp)
        {
            rtpPacket = rtpPacket with { Header = rtpPacket.Header with { Timestamp = Timestamp }};
        }

        if (appendSettings.UseOriginalSequence)
        {
            rtpPacket = rtpPacket with { Header = rtpPacket.Header with { SequenceNumber = Sequence }};
        }

        try
        {
            await _rtpPacketsChannel.Writer.WriteAsync(rtpPacket, _stoppingToken);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private async Task GeneratePackets(double fps)
    {
        var interval = TimeSpan.FromMilliseconds(1000 / fps);
        
        while (!_stoppingToken.IsCancellationRequested)
        {
            await _semaphore.WaitAsync(_stoppingToken);
            var rtpHeader = new RtpHeader(false, false, 0, true, 26, Sequence, Timestamp, 0x12121212, [], 0);
            var rtpJpegHeader = new RtpJpegHeader(0, [0, 0, 0], 1, 99, 90, 60);
            var rtpPacket = new RtpPacket(rtpHeader, rtpJpegHeader, _staticJpeg);

            await _rtpPacketsChannel.Writer.WriteAsync(rtpPacket, _stoppingToken);
            _semaphore.Release();
            
            await Task.Delay(interval, _stoppingToken);
        }
    }
}