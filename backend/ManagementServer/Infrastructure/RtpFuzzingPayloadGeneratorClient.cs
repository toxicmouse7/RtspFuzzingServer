using AutoMapper;
using Flurl;
using Flurl.Http;
using ManagementServer.Domain.Abstract;
using ManagementServer.Domain.Models;
using RtspServer.Domain.Models.Rtp;

namespace ManagementServer.Infrastructure;

public class RtpFuzzingPayloadGeneratorClient : IRtpFuzzingPayloadGenerator
{
    private readonly IMapper _mapper;

    public RtpFuzzingPayloadGeneratorClient(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<IReadOnlyCollection<RawFuzzingData>> GenerateRtpPayloadsAsync(
        RtpFuzzingPreset preset, TimeSpan generateFor)
    {
        var url = "http://localhost:8080/BinaryPayload/generate"
            .AppendQueryParam("genTimeSec", generateFor.Seconds);
        var packet = _mapper.Map<RtpPacket>(preset);

        
        var response = await url.PostJsonAsync(new BinaryData
        {
            Data = Convert.ToBase64String((packet with { Content = [] }).Serialize())
        });

        var rawData = await response.GetJsonAsync<IReadOnlyCollection<BinaryData>>();

        return rawData.Select(d => new RawFuzzingData(preset, Convert.FromBase64String(d.Data))).ToList();
    }

    private class BinaryData
    {
        public string Data { get; init; } = null!;
    }
}