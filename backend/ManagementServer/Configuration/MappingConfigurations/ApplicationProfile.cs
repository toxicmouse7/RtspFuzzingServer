using AutoMapper;

namespace ManagementServer.Configuration.MappingConfigurations;

public class ApplicationProfile : Profile
{
    public ApplicationProfile()
    {
        CreateMap<Domain.Models.RtpFuzzingPreset, RtspServer.Domain.Models.Rtp.RtpPacket>()
            .ForCtorParam(nameof(RtspServer.Domain.Models.Rtp.RtpPacket.Header),
                opt => opt.MapFrom(s => s.Header))
            .ForCtorParam(nameof(RtspServer.Domain.Models.Rtp.RtpPacket.ContentHeader),
                opt => opt.MapFrom(s => s.ContentHeader))
            .ForCtorParam(nameof(RtspServer.Domain.Models.Rtp.RtpPacket.Content),
                opt => opt.MapFrom(s => s.Payload))
            .ForAllMembers(opt => opt.Ignore());
    }
}