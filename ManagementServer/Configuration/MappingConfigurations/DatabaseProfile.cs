using AutoMapper;
using ManagementServer.Infrastructure.Persistence.Models;
using Newtonsoft.Json;

namespace ManagementServer.Configuration.MappingConfigurations;

public class DatabaseProfile : Profile
{
    public DatabaseProfile()
    {
        CreateMap<RtpFuzzingPreset, Domain.Models.RtpFuzzingPreset>()
            .ForCtorParam(nameof(Domain.Models.RtpFuzzingPreset.ContentHeader),
                opt => opt.MapFrom(s => MapContentHeader(s.ContentHeaderType, s.ContentHeader)))
            .ReverseMap()
            .ForMember(d => d.ContentHeader, opt => opt.MapFrom(s => JsonConvert.SerializeObject(s.ContentHeader)))
            .ForMember(d => d.ContentHeaderType, opt => opt.MapFrom(s => MapContentHeader(s.ContentHeader)));
    }

    private RtspServer.Domain.Models.Abstract.RtpContentHeader MapContentHeader(HeaderType type, string serializedData)
    {
        return type switch
        {
            HeaderType.Jpeg => JsonConvert.DeserializeObject<RtspServer.Domain.Models.Rtp.RtpJpegHeader>(serializedData)!,
            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };
    }

    private HeaderType MapContentHeader(RtspServer.Domain.Models.Abstract.RtpContentHeader header)
    {
        return header switch
        {
            RtspServer.Domain.Models.Rtp.RtpJpegHeader => HeaderType.Jpeg,
            _ => throw new ArgumentOutOfRangeException(nameof(header))
        };
    }
}