using System.Diagnostics;
using ManagementServer.Settings;
using Microsoft.Extensions.Options;
using RtspServer.Abstract;
using RtspServer.Infrastructure.RTP;

namespace ManagementServer.Infrastructure;

public sealed class StaticDataSource : IDataSource
{
    private readonly StaticDataSourceSettings _settings;
    private readonly byte[] _streamableData;
    private readonly double _interframeInterval;
    private long _lastReturnTicks;

    public StaticDataSource(
        IOptions<StaticDataSourceSettings> settings)
    {
        _settings = settings.Value;
        _streamableData = File.ReadAllBytes(_settings.JpegPath);
        _interframeInterval = 1000.0 / _settings.Fps;
    }

    public async Task<byte[]> GetStreamableDataAsync()
    {
        var passedFromLastReturn = TimeSpan.FromTicks(DateTimeOffset.UtcNow.Ticks - _lastReturnTicks);
        
        if (passedFromLastReturn.TotalMilliseconds < _interframeInterval)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(_interframeInterval - passedFromLastReturn.TotalMilliseconds));
        }
        
        _lastReturnTicks = DateTimeOffset.UtcNow.Ticks;

        var rtpJpegHeader = new RTPJpegHeader
        {
            Type = 1,
            FragmentOffset = [0, 0, 0],
            Height = 60,
            Width = 90,
            Q = 99,
            TypeSpecific = 0,
        };
        
        return rtpJpegHeader.ToByteArray().Concat(_streamableData).ToArray();
    }
}